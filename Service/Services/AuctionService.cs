using AutoMapper;
using LinqKit;
using Microsoft.Extensions.Logging;
using Service.Commons;
using Service.Exceptions;
using Service.IServices;
using ShopRepository.Models;

using Service.Utils;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Response;
using ShopRepository.Enums;
using ShopRepository.Models;
using ShopRepository.Repositories.UnitOfWork;
using System.Data.Entity.Infrastructure;

namespace Service.Services;

public class AuctionService : IAuctionService
{
    private readonly ILogger<AuctionService> _logger;
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public AuctionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuctionService> logger)
    {
        _unitOfWork = (UnitOfWork)unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<OperationResult<List<AuctionResponse>>> GetAll(GetAllAuctions request)
    {
        var result = new OperationResult<List<AuctionResponse>>();

        var pagin = new Pagination();

        var filter = request.GetExpressions();

        try
        {
            var entities = _unitOfWork.AuctionRepository.Get(
                filter: request.GetExpressions(),
                pageIndex: request.PageNumber,
                orderBy: request.GetOrder()
            );
            var listResponse = _mapper.Map<List<AuctionResponse>>(entities);

            if (listResponse == null || !listResponse.Any())
            {
                result.AddResponseStatusCode(StatusCode.Ok, "List Auction is Empty!", listResponse);
                return result;
            }

            pagin.PageSize = request.PageSize;
            pagin.TotalItemsCount = listResponse.Count();

            result.AddResponseStatusCode(StatusCode.Ok, "Get List Auctions Done.", listResponse, pagin);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in getAll Service Method");
            throw;
        }
    }

    public async Task<OperationResult<AuctionResponse>> GetById(int id)
    {
        var result = new OperationResult<AuctionResponse>();
        try
        {
            var entity = await _unitOfWork.AuctionRepository.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddError(StatusCode.NotFound, "Auction Id",$"Can't found Auction with Id: {id}");
            }else 
            
            if((bool)entity.IsActived)
            {
                var productResponse = _mapper.Map<AuctionResponse>(entity);
                result.AddResponseStatusCode(StatusCode.Ok, $"Get Auction by Id: {id} Success!", productResponse);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred in Get Auction By Id service method for ID: {id}");
            throw;
        }
    }

    public async Task<OperationResult<bool>> CreateEntity(CreateAuctionRequest request)
    {
        var result = new OperationResult<bool>();

        try
        {
            var entityAuction = _mapper.Map<Auction>(request);

            entityAuction.Status = (int)AuctionEnums.Status.PENDING;

            entityAuction = await _unitOfWork.AuctionRepository.AddAsync(entityAuction);

            if (entityAuction.ProductImages == null)
            {
                entityAuction.ProductImages = new List<ProductImage>();
            }

            foreach (var productImage in request.ProductImageRequests
                         .Select(imageRequest => _mapper.Map<ProductImage>(imageRequest)))
            {
                entityAuction.ProductImages.Add(productImage);
            }

            var checkResult = _unitOfWork.Save();

            if (checkResult > 0)
            {
                result.AddResponseStatusCode(StatusCode.Created, "Add Auction Success!", true);
            }
            else
            {
                result.AddError(StatusCode.BadRequest, "Create Auction", "Add Auction Failed!"); ;
            }

            return result;

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occurred in Create Auction service method");
            throw;
        }

    }
        public async Task<OperationResult<bool>> StaffUpdate(int id, StaffUpdate request)
        {
            var result = new OperationResult<bool>();

            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);

                if (auction == null)
                {
                    result.AddError(StatusCode.NotFound, "Auction Id", $"Cannot find Auction with Id: {id}");
                    return result;
                }

                if (auction.Status != (int)AuctionEnums.Status.PENDING)
                {
                    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                }

                // Update auction fields using ReflectionUtils
                ReflectionUtils.UpdateFields(request, auction);

                // Set the status to EVALUATE
                auction.Status = (int?)AuctionEnums.Status.EVALUATE;

                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Auction Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Auction", "Update Auction Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Auction service method for ID: {id}");
                throw;
            }
        }


        public async Task<OperationResult<bool>> AdminAprrove(int id, AdminApproveRequest request)
        {
            var result = new OperationResult<bool>();

            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);

                if (auction == null)
                {
                    result.AddError(StatusCode.NotFound, "Auction Id", $"Cannot find Auction with Id: {id}");
                    return result;
                }

                if (auction.Status != (int)AuctionEnums.Status.CONFIRM)
                {
                    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                }

                // Update auction fields using ReflectionUtils
                ReflectionUtils.UpdateFields(request, auction);

                // Set the status to EVALUATE
                auction.Status = (int?)AuctionEnums.Status.APPROVE;

                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Auction Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Auction", "Update Auction Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Auction service method for ID: {id}");
                throw;
            }
        }

        public async Task<OperationResult<bool>> StaffConfirm(int id, StaffConfirmRequest request)
        {
            var result = new OperationResult<bool>();

            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);

                if (auction == null)
                {
                    result.AddError(StatusCode.NotFound, "Auction Id", $"Cannot find Auction with Id: {id}");
                    return result;
                }

                if (auction.Status != (int)AuctionEnums.Status.WAITING)
                {
                    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                }

                // Update auction fields using ReflectionUtils
                ReflectionUtils.UpdateFields(request, auction);

                // Set the status to EVALUATE
                auction.Status = (int?)AuctionEnums.Status.CONFIRM;
                auction.EndDate = request.StartDate.AddMinutes((double)auction.Duration);

                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Auction Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Auction", "Update Auction Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Auction service method for ID: {id}");
                throw;
            }
        }

        public async Task<OperationResult<bool>> UserWaiting(int id)
        {
            var result = new OperationResult<bool>();

            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);

                if (auction == null)
                {
                    result.AddError(StatusCode.NotFound, "Auction Id", $"Cannot find Auction with Id: {id}");
                    return result;
                }

                if (auction.Status != (int)AuctionEnums.Status.EVALUATE)
                {
                    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                }

                

                // Set the status to WAITING
                auction.Status = (int)AuctionEnums.Status.WAITING;

                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                var checkResult = await _unitOfWork.SaveChangesAsync();  // Assuming SaveAsync() returns a Task<int>

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Auction Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Auction", "Update Auction Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Auction service method for ID: {id}");
                throw;
            }
        }


        public async Task<OperationResult<bool>> UserComming(int id)
        {
            var result = new OperationResult<bool>();

            try
            {
                var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);

                if (auction == null)
                {
                    result.AddError(StatusCode.NotFound, "Auction Id", $"Cannot find Auction with Id: {id}");
                    return result;
                }

                if (auction.Status != (int)AuctionEnums.Status.APPROVE)
                {
                    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                }

            

                // Set the status to EVALUATE
                auction.Status = (int?)AuctionEnums.Status.COMMING;
                auction.IsActived = true;


                await _unitOfWork.AuctionRepository.UpdateAsync(auction);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Auction Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Auction", "Update Auction Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Auction service method for ID: {id}");
                throw;
            }
        }

    public async Task<OperationResult<bool>> UpdateAuction(Auction auction)
    {
        var result = new OperationResult<bool>();

        try
        {
            // Fetch the existing auction
            var existingAuction = await _unitOfWork.AuctionRepository.GetByIdAsync(auction.AuctionId);
            if (existingAuction == null)
            {
                // Auction not found, return error response
                result.AddError(StatusCode.NotFound, "AuctionId", "Auction not found.");
                return result;
            }

            // Map the updates from the provided auction to the existing auction
            // Ensure only necessary fields are updated
            existingAuction.BiddingPrice = auction.BiddingPrice;
            existingAuction.UpdateAt = DateTime.UtcNow; // Update timestamp or any other fields if needed

            // Save the updated auction
            await _unitOfWork.AuctionRepository.UpdateAsync(existingAuction);
            var checkResult = _unitOfWork.Save(); // Ensure Save is awaited

            // Check if the update was successful
            if (checkResult > 0)
            {
                result.AddResponseStatusCode(StatusCode.Ok, "Auction updated successfully", true);
            }
            else
            {
                result.AddError(StatusCode.BadRequest, "AuctionUpdate", "Failed to update the auction.");
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency issues
            _logger.LogError(ex, "Concurrency error occurred while updating the auction.");
            result.AddError(StatusCode.ServerError, "AuctionUpdate", "Data is updated by another user!");
        }
        catch (DbUpdateException ex)
        {
            // Handle data integrity issues
            _logger.LogError(ex, "Data integrity error occurred while updating the auction.");
            result.AddError(StatusCode.BadRequest, "AuctionUpdate", "Data integrity violation.");
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            _logger.LogError(ex, "An unexpected error occurred.");
            result.AddUnknownError("AuctionUpdate", "An unexpected error occurred.");
        }

        return result;
    }


}