using Service.Commons;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Auctions;
using Service.ViewModels.Request.Order;
using Service.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IOrderService
    {
        public Task<OperationResult<List<OrderResponse>>> GetAll(GetAllOrder request);
        public Task<OperationResult<OrderResponse>> GetById(int id);
        
        public Task<OperationResult<bool>> CreateEntity(CreateOrderRequest request);
        public Task<OperationResult<bool>> UpdateOrder(int id, UpdateOrderRequest request);
        public Task<OperationResult<bool>> DeleteOrder(int id);
    }
}
