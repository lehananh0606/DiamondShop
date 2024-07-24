using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Commons;
using Service.Exceptions;
using Service.Exeptions;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Request.Bid;
using Service.ViewModels.Request.Order;
using Service.ViewModels.Response;
using ShopRepository.Enums;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.Repository;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BidService : IBidService
    {
        private readonly ILogger<BidService> _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IAuctionService _auctionService;
        private readonly IUserService _userService;
        private readonly IBidRepository _bidRepository;
        

        public BidService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BidService> logger, IAuctionService auctionService, IUserService userService, IBidRepository bidRepository)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _auctionService = auctionService;
            _userService = userService;
            _bidRepository = bidRepository;
            
        }

        public async Task<OperationResult<bool>> CreateEntity(CreateBidRequest request)
        {
            var result = new OperationResult<bool>();

            try
            {
                // Fetch the auction
                var auctionResult = await _auctionService.GetById(request.AuctionId);
                if (auctionResult.IsError || auctionResult.Payload == null)
                {
                    throw new NotFoundException("Auction not found.");
                }
                var auctionResponse = auctionResult.Payload;
                var auction = _mapper.Map<Auction>(auctionResponse); // Use AutoMapper

                // Check if auction is in bidding status
                var auctionStatus = (AuctionEnums.Status)auction.Status;
                if (auctionStatus != AuctionEnums.Status.BIDDING)
                {
                    throw new BadRequestException("Auction not available for bidding.");
                }


                // Fetch the user
                var userResult = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId); // Assuming this should be _userService, not _auctionService
                if (userResult == null)
                {
                    throw new NotAcceptableStatusException("User not found.");
                }

                // Check if the user has registered for the auction
                var userBid = await _bidRepository.FindByUserIdAndAuctionId(request.UserId, request.AuctionId);
                if (userBid == null)
                {
                    throw new NotAcceptableStatusException("User must register to bid.");
                }

                // Fetch the top bid
                var top1Bid = await _bidRepository.FindTop1ByAuctionId(request.AuctionId);

                if (top1Bid == null)
                {
                    // First bid, must be greater than the start price
                    if (request.BiddingPrice >= auction.StartPrice)
                    {
                        userBid.IsTop1 = true;
                        userBid.Ratings = 1;
                        auction.BiddingPrice = request.BiddingPrice;
                    }
                    else
                    {
                        throw new BadRequestException("Bidding price must be greater than the start price.");
                    }
                }
                else
                {
                    // Existing bids, compare with top1 bid
                    if (request.BiddingPrice >= top1Bid.BiddingPrice + auction.DepositPrice)
                    {
                        if (top1Bid.BidId != userBid.BidId)
                        {
                            // Update the previous top1 bid
                            top1Bid.IsTop1 = false;
                            userBid.IsTop1 = true;
                        }

                        userBid.Ratings = top1Bid.Ratings + 1;
                        auction.BiddingPrice = request.BiddingPrice;
                    }
                    else
                    {
                        throw new BadRequestException("Bidding price must be greater than the current top bid plus deposit.");
                    }
                }

                // Save the changes
                await _auctionService.UpdateAuction(auction);
                await _bidRepository.UpdateEntityAsync(userBid); // Update userBid
                if (top1Bid != null && top1Bid.BidId != userBid.BidId)
                {
                    await _bidRepository.UpdateEntityAsync(top1Bid); // Update previous top1Bid
                }

                result.Payload = true;
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred in CreateEntity method");
                throw;
            }
        }




        public async Task<OperationResult<List<BidResponse>>> GetAll(ViewModels.Request.Bid.GetAllBidRequest request)
        {
            var result = new OperationResult<List<BidResponse>>();

            var pagin = new Pagination();

            var filter = request.GetExpressions();

            try
            {
                _logger.LogInformation($"Fetching Bids with PageIndex: {request.PageNumber}, PageSize: {request.PageSize}");

                var entities = _unitOfWork.BidRepository.Get(
                    filter: request.GetExpressions(),
                    pageIndex: request.PageNumber,
                    orderBy: request.GetOrder()
                ).ToList();

                _logger.LogInformation($"Fetched {entities.Count} Bids from the database.");

                var listResponse = _mapper.Map<List<BidResponse>>(entities);

                if (listResponse == null || !listResponse.Any())
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "List Bid is Empty!", listResponse);
                    return result;
                }

                pagin.PageSize = request.PageSize;
                pagin.TotalItemsCount = entities.Count; // Correctly count the total items

                result.AddResponseStatusCode(StatusCode.Ok, "Get List Bids Done.", listResponse, pagin);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred in getAll Service Method");
                throw;
            }
        }

        public async Task<OperationResult<BidResponse>> GetById(int id)
        {
            var result = new OperationResult<BidResponse>();
            try
            {
                var entity = await _unitOfWork.BidRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Bid Id", $"Can't found Bid with Id: {id}");
                }
                else

                if (entity.IsDeleted == false)
                {
                    var productResponse = _mapper.Map<BidResponse>(entity);
                    result.AddResponseStatusCode(StatusCode.Ok, $"Get Bid by Id: {id} Success!", productResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred in Get Bid By Id service method for ID: {id}");
                throw;
            }
        }

        public async Task<OperationResult<BidResponse>> UpdateBid(int id, UpdateBidRequest request)
        {
            var result = new OperationResult<BidResponse>();

            try
            {
              
               

                // Fetch the existing bid without tracking
                var existingBid = await _bidRepository.GetByIdAsync(id);
                if (existingBid == null)
                {
                    result.AddError(StatusCode.NotFound, "Bid", $"Bid '{id}' not found.");
                    return result;
                }

                // Fetch the auction without tracking
                var auctionResponse = await _unitOfWork.AuctionRepository.GetByIdAsync((int)existingBid.AuctionId);
                // Fetch the top bid without tracking
                var top1Bid = await _bidRepository.FindTop1ByAuctionId((int)existingBid.AuctionId);

                // Check if auction is in bidding status
                
                if (auctionResponse.Status != (int)AuctionEnums.Status.BIDDING)
                {
                    throw new BadRequestException("Auction not available for bidding.");
                }

                // Fetch the user
                var userResult = await _unitOfWork.UserRepository.GetByIdAsync((int)existingBid.UserId);
                if (userResult == null)
                {
                    throw new NotAcceptableStatusException("User not found.");
                }

                // Check if the user has registered for the auction
                var userBid = await _bidRepository.FindByUserIdAndAuctionId((int)existingBid.UserId, (int)existingBid.AuctionId);
                if (userBid == null)
                {
                    throw new NotAcceptableStatusException("User must register to bid.");
                }

                // Handle bid update logic
                if (top1Bid == null)
                {
                    // First bid, must be greater than the start price
                    if (request.BiddingPrice >= auctionResponse.StartPrice)
                    {
                        userBid.IsTop1 = true;
                        userBid.Ratings = 1;
                        existingBid.BiddingPrice = request.BiddingPrice;
                    }
                    else
                    {
                        throw new BadRequestException("Bidding price must be greater than the start price.");
                    }
                }
                else
                {
                    // Existing bids, compare with top1 bid
                    if (request.BiddingPrice >= top1Bid.BiddingPrice + auctionResponse.DepositPrice)
                    {
                        if (top1Bid.BidId != userBid.BidId)
                        {
                            // Update the previous top1 bid
                            top1Bid.IsTop1 = false;
                            userBid.IsTop1 = true;
                        }

                        userBid.Ratings = top1Bid.Ratings + 1;
                        existingBid.BiddingPrice = request.BiddingPrice;
                    }
                    else
                    {
                        throw new BadRequestException("Bidding price must be greater than the current top bid plus deposit.");
                    }
                }

                // Update the bid entity
                existingBid.BiddingPrice = request.BiddingPrice;
                existingBid.UpdateAt = DateTime.UtcNow; // Update timestamp
                auctionResponse.BiddingPrice = (float)existingBid.BiddingPrice;
                await _unitOfWork.AuctionRepository.UpdateAsync(auctionResponse);


                // Save changes
                await _unitOfWork.BidRepository.UpdateAsync(existingBid);
                if (top1Bid != null && top1Bid.BidId != userBid.BidId)
                {
                    await _unitOfWork.BidRepository.UpdateEntityAsync(top1Bid);
                }
                await _unitOfWork.BidRepository.UpdateEntityAsync(userBid);

                // Update the auction's BiddingPrice if the bid update is successful
                
                
               

                var bidResponse = _mapper.Map<BidResponse>(existingBid); // Mapping if using AutoMapper
                result.Payload = bidResponse;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateBid method");
                result.AddError(StatusCode.ServerError, "Error", "An error occurred while updating the bid.");
                return result;
            }
        }




        public async Task<OperationResult<bool>> DeleteBid(int id)
        {
            var result = new OperationResult<bool>();

            try
            {
                var order = await _unitOfWork.BidRepository.GetByIdAsync(id);

                if (order == null)
                {
                    result.AddError(StatusCode.NotFound, "Bid Id", $"Cannot find Bid with Id: {id}");
                    return result;
                }

                // Set the status to EVALUATE
                order.IsDeleted = true;

                await _unitOfWork.BidRepository.UpdateAsync(order);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Bid Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Bid", "Update Bid Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Bid service method for ID: {id}");
                throw;
            }
        }

        public async Task<bool> RegisterAuctionAsync(int id, RegisterAuctionDTO dto)
        {
            try
            {
                // Retrieve user by ID
                var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                {
                    return false; // User not found
                }

                // Retrieve wallet by user ID
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(user.UserId);
                if (wallet == null)
                {
                    return false; // Wallet not found
                }

                // Retrieve auction by ID
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);
                if (auction == null)
                {
                    return false; // Auction not found
                }

                // Check if the auction status is COMING (5) and user has not placed a bid
                if (auction.Status.HasValue && auction.Status.Value == (int)AuctionEnums.Status.COMMING &&
                    !await _unitOfWork.BidRepository.ExistsBidByAuctionIdAndUserIdAsync(auction.AuctionId, user.UserId))
                {
                    // Check if the wallet balance is sufficient
                    if (wallet.Balance >= auction.StartPrice)
                    {
                        var tranCode = GenerateCodeUtils.GenerateCode4Transaction(
                            TypeTrans.RT, auction.ProductCode, dto.UserId);

                        var transaction = new Transaction
                        {
                            Wallet = wallet,
                            Amount = auction.StartPrice,
                            Status = OrderEnums.Status.APPROVE.ToString(),
                            Resource = "Wallet",
                            PaymentMethod = "Wallet",
                            Content = "Deposit to register auction",
                            CreatedBy = user.Name,
                            IsDeleted = false,
                            TransactionType = PaymentMethod.DEPOSIT.ToString(),
                            TransactionCode = tranCode
                        };
                        await _unitOfWork.TransactionRepository.AddAsync(transaction);

                        wallet.Balance -= auction.StartPrice;
                        await _unitOfWork.WalletRepository.UpdateAsync(wallet);

                        var bid = new Bid
                        {
                            Auction = auction,
                            User = user,
                            UserName = user.Name,
                            CreateAt = DateTime.UtcNow,
                            UpdateAt = DateTime.UtcNow,
                            IsDeleted = false,
                            IsTop1 = false,
                            Ratings = 0
                        };
                        await _unitOfWork.BidRepository.AddAsync(bid);

                        // Ensure changes are saved
                        await _unitOfWork.SaveChangesAsync();

                        return true; // Registration successful
                    }
                    else
                    {
                        return false; // Insufficient balance
                    }
                }
                else
                {
                    return false; // Auction not COMING or user already registered
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.Message);
                return false; // Exception occurred
            }
        }


    }
}
