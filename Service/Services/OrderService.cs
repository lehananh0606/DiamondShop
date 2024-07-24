using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Commons;
using Service.Exceptions;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Request.Order;
using Service.ViewModels.Response;
using ShopRepository.Enums;
using ShopRepository.Models;
using ShopRepository.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> CreateEntity(CreateOrderRequest request)
        {

            var result = new OperationResult<bool>();

            try
            {
                var entityOrder = _mapper.Map<Order>(request);

                entityOrder.Status = (int)OrderEnums.Status.PENDING;

                entityOrder = await _unitOfWork.OrderRepository.AddAsync(entityOrder);

                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Created, "Create Order Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Create Order", "Add Order Failed!"); ;
                }

                return result;

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Create Order service method");
                throw;
            }
        }

        public async Task<OperationResult<List<OrderResponse>>> GetAll(GetAllBidRequest request)
        {
            var result = new OperationResult<List<OrderResponse>>();

            var pagin = new Pagination();

            var filter = request.GetExpressions();

            try
            {
                var entities = _unitOfWork.OrderRepository.Get(
                    filter: request.GetExpressions(),
                    pageIndex: request.PageNumber,
                    orderBy: request.GetOrder()
                );
                var listResponse = _mapper.Map<List<OrderResponse>>(entities);

                if (listResponse == null || !listResponse.Any())
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "List Order is Empty!", listResponse);
                    return result;
                }

                pagin.PageSize = request.PageSize;
                pagin.TotalItemsCount = listResponse.Count();

                result.AddResponseStatusCode(StatusCode.Ok, "Get List Orders Done.", listResponse, pagin);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred in getAll Service Method");
                throw;
            }
        }

        public async Task<OperationResult<OrderResponse>> GetById(int id)
        {
            var result = new OperationResult<OrderResponse>();
            try
            {
                var entity = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Order Id", $"Can't found Order with Id: {id}");
                }
                else

                if (entity.IsDeleted == false)
                {
                    var productResponse = _mapper.Map<OrderResponse>(entity);
                    result.AddResponseStatusCode(StatusCode.Ok, $"Get Order by Id: {id} Success!", productResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred in Get Order By Id service method for ID: {id}");
                throw;
            }
        }



        public async Task<OperationResult<bool>> UpdateOrder(int id, UpdateOrderRequest request)
        {
            var result = new OperationResult<bool>();

            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

                if (order == null)
                {
                    result.AddError(StatusCode.NotFound, "Order Id", $"Cannot find Order with Id: {id}");
                    return result;
                }

                //if (auction.Status != (int)OrderEnums.Status.PENDING)
                //{
                //    throw new BadRequestException("Auction is closed for edit because it is live and cannot be edited!");
                //}

                // Update auction fields using ReflectionUtils
                ReflectionUtils.UpdateFields(request, order);

                // Set the status to EVALUATE
                //auction.Status = (int?)OrderEnums.Status.EVALUATE;

                await _unitOfWork.OrderRepository.UpdateAsync(order);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Order Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Order", "Update Order Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Order service method for ID: {id}");
                throw;
            }
        }

        public async Task<OperationResult<bool>> DeleteOrder(int id)
        {
            var result = new OperationResult<bool>();

            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

                if (order == null)
                {
                    result.AddError(StatusCode.NotFound, "Order Id", $"Cannot find Order with Id: {id}");
                    return result;
                }

                // Set the status to EVALUATE
                order.IsDeleted = true;
               
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                var checkResult = _unitOfWork.Save();

                if (checkResult > 0)
                {
                    result.AddResponseStatusCode(StatusCode.Ok, "Update Order Success!", true);
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Order", "Update Order Failed!");
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred in Update Order service method for ID: {id}");
                throw;
            }
        }

    }
}
