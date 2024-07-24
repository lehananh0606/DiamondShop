using Quartz;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Service.IServices;
using ShopRepository.Models;
using Service.Contants;
using Google.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Service.Exceptions;
using System.ComponentModel.DataAnnotations;
using ShopRepository.Enums;
using Service.Utils;

namespace Service.Quartz
{
    public class EveryMinute : IJob
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseService<Auction> _firebaseAuctionService;
        public EveryMinute(IUnitOfWork unitOfWork, IFirebaseService<Auction> firebaseService)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _firebaseAuctionService = firebaseService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("intro the EveryMinute");

            var now = DateTime.UtcNow;

            try
            {
                List<Auction> list = await _firebaseAuctionService.GetAuctions("test", 5, now);
            }
            catch (Exception ex)
            {
                throw;
            }
            
            
            //Console.WriteLine("list-----", list);
            //var timeThresholdBefore = DateTime.Now.AddHours(-48);

            await ExpiredCommingAuction();
            await ExpiredBiddingAuction();

        }
        private async Task ExpiredCommingAuction()
        {
            var timeThreshold = DateTime.Now;

            var statuses = new List<int?> { 5 };

            try
            {
                var auctions = _unitOfWork.AuctionRepository.Get(
                   filter: u => statuses.Contains(u.Status)
                   && u.IsActived == true
                   && u.IsRejected == false
                   && u.StartDate <= timeThreshold,
                   includeProperties: "Bids,ProductImages",
                   pageSize: -1
                );

                foreach (var auction in auctions)
                {

                    string msg = "update auction " + auction.AuctionId
                        + " statusfrom: " + auction.Status;

                    auction.Status = auction.Status + 1;

                    msg += " statusTo: " + auction.Status;

                    auction.UpdateAt = DateTime.Now;

                    Console.WriteLine(msg);

                    auction.EndDate = auction.StartDate.Value.AddMinutes(auction.Duration.HasValue ? (double)auction.Duration.Value : AUCTIONCONSTANT.VALUEAUCTION.MINDURATION);


                    await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                    await _firebaseAuctionService.SaveAuction(auction, auction.AuctionId, AUCTIONCONSTANT.COLLECTIONFIREBASE.AUCTIONS);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: ", e.Message);
            }

            await _unitOfWork.SaveChangesAsync();

        }

        private async Task ExpiredBiddingAuction()
        {
            var timeThreshold = DateTime.Now;
            var statuses = new List<int?> { 6 };

            try
            {
                var auctions = _unitOfWork.AuctionRepository.Get(
                    filter: u => statuses.Contains(u.Status)
                    && u.IsActived == true
                    && u.IsRejected == false
                    && u.EndDate <= timeThreshold,
                    includeProperties: "Bids,ProductImages",
                    pageSize: -1
                );

                foreach (var auction in auctions)
                {
                    await ProcessAuction(auction);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private async Task ProcessAuction(Auction auction)
        {
            string msg = "update auction " + auction.AuctionId
                         + " status from: " + auction.Status;

            auction.Status += 1;

            msg += " status to: " + auction.Status;

            auction.UpdateAt = DateTime.Now;

            Console.WriteLine(msg);

            var top1Bid = await _unitOfWork.BidRepository.FindTop1ByAuctionId(auction.AuctionId);
            auction.EndPrice = top1Bid?.BiddingPrice;

            if (top1Bid != null)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync((int)top1Bid.UserId);
                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }

                // Check if an order already exists for this AuctionID
                var existingOrder = await _unitOfWork.OrderRepository.GetByAuctionIdAsync(auction.AuctionId);
                if (existingOrder == null)
                {
                    var order = new Order
                    {
                        Total = auction.EndPrice,
                        Phone = user.Phone,
                        Address = user.Address,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        CreateBy = "System",
                        ModifiedBy = "System",
                        PaymentMethod = "Wallet",
                        IsExpired = false,
                        ExpiredAt = DateTime.Now.AddHours(24),
                        AuctionTitle = auction.Title,
                        AuctionName = auction.ProductName,
                        AuctionCode = auction.ProductCode,
                        Quantity = auction.Quantity,
                        UserName = user.Name,
                        IsDeleted = false,
                        UserId = top1Bid.UserId,
                        AuctionId = top1Bid.AuctionId,
                        Status = (int?)OrderEnums.Status.PENDING
                    };

                    try
                    {
                        await _unitOfWork.OrderRepository.AddAsync(order);
                        Console.WriteLine($"Order created successfully for AuctionID: {auction.AuctionId}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating order for AuctionID: {auction.AuctionId} - {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Order already exists for AuctionID: {auction.AuctionId}");
                }
            }

            var top2 = await _unitOfWork.BidRepository.FindNotTop1ByAuctionId(auction.AuctionId);
            if (top2 != null)
            {
                var userTop2 = await _unitOfWork.UserRepository.GetByIdAsync((int)top2.UserId);
                if (userTop2 != null)
                {
                    var wallet = await _unitOfWork.WalletRepository.GetWalletByAccountIdAsync(userTop2.UserId);
                    var tranCode = GenerateCodeUtils.GenerateCode4Transaction(
                        TypeTrans.HT, auction.ProductCode, userTop2.UserId);

                    var transaction = new ShopRepository.Models.Transaction
                    {
                        Wallet = wallet,
                        Amount = auction.StartPrice,
                        Status = OrderEnums.Status.APPROVE.ToString(),
                        Resource = "System",
                        PaymentMethod = "Wallet",
                        Content = "Refund deposit to register auction",
                        CreatedBy = "System",
                        TransactionType = PaymentMethod.DEPOSIT.ToString(),
                        TransactionCode = tranCode
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    wallet.Balance += auction.StartPrice;
                    await _unitOfWork.WalletRepository.UpdateAsync(wallet);
                }
            }

            await _unitOfWork.AuctionRepository.UpdateAsync(auction);
            await _firebaseAuctionService.SaveAuction(auction, auction.AuctionId, AUCTIONCONSTANT.COLLECTIONFIREBASE.AUCTIONS);
        }


    }
}
