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

                    string msg = "update auction " + auction.AuctionId
                        + " statusfrom: " + auction.Status;

                    auction.Status = auction.Status + 1;

                    msg += " statusTo: " + auction.Status;

                    auction.UpdateAt = DateTime.Now;

                    Console.WriteLine(msg);

                    await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                    await _firebaseAuctionService.SaveAuction(auction, auction.AuctionId, AUCTIONCONSTANT.COLLECTIONFIREBASE.AUCTIONS);
                    
                }
            }
            catch( Exception e)
            {
                Console.WriteLine("Exception: ", e.Message);
            }

            await _unitOfWork.SaveChangesAsync();

        }

    }
}
