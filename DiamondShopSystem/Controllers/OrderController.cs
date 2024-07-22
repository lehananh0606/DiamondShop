using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.Services;
using Service.ViewModels.Request;
using Service.ViewModels.Request.Order;

namespace DiamondShopSystem.Controllers
{
    public class OrderController : BaseController
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 
        /// Get all auctions
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("orders")]
        // get all
        public async Task<IActionResult> GetAll([FromQuery] GetAllOrder request)
        {
            var response = await _orderService.GetAll(request);

            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Get auction by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _orderService.GetById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpPost("order")]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var response = await _orderService.CreateEntity(request);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);

        }

        [HttpPut("order/delete/{id}")]
        public async Task<IActionResult> UserComming(int id)
        {
            var response = await _orderService.DeleteOrder(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
