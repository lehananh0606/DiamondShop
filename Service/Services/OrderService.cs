using AutoMapper;
using Microsoft.Extensions.Logging;
using Service.Commons;
using Service.IServices;
using Service.ViewModels.Request;
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

        public async Task<OperationResult<List<OrderResponse>>> GetAll(GetAllOrder request)
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

                if ((bool)entity.IsDeleted)
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
    }
}
