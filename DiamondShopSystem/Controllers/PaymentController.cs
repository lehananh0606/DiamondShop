using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Service.Commons;
using Service.IServices;
using Service.Utils;
using Service.ViewModels.Request;

namespace DiamondShopSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IUserService _userService;
        private readonly IAuctionService _auctionService;
        private readonly IOrderService _orderService;

        public PaymentController(IVnPayService vnPayService, IUserService userService, IAuctionService auctionService, IOrderService orderService)
        {
            _vnPayService = vnPayService;
            _userService = userService;
            _auctionService = auctionService;
            _orderService = orderService;
        }
        [HttpPost("create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid payment request model.");
            }

            try
            {
                var paymentUrl = await _vnPayService.CreatePaymentUrl(HttpContext, model);

                return Ok(new { Url = paymentUrl });
            }
            catch (Exception ex)
            {
                string error = ErrorUtil.GetErrorString("Exception", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }




        [HttpGet("payment-execute")]
        public async Task<IActionResult> PaymentExecute()
        {
            var query = Request.Query;
            var paymentResponse = await _vnPayService.PaymentExecute(query);

            if (!paymentResponse.Success)
            {
                return BadRequest("Invalid payment response.");
            }

            return Ok(paymentResponse);
        }

        [HttpGet("payment/payment-callback")] // Unique route for PaymentCallBack
       
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query).Result;

            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction(nameof(PaymentFail));
            }

            //save payment
            var responsePayment = _vnPayService.DepositPayment(response).Result;

            if (responsePayment.IsError)
            {
                return RedirectToAction(nameof(PaymentFail));
            }

            return RedirectToAction(nameof(PaymentSuccess));
        }
        [HttpGet("payment/fail")]
       
        public IActionResult PaymentFail()
        {
            var response = new OperationResult<bool>()
            {
                StatusCode = Service.Commons.StatusCode.BadRequest,
                Payload = false,
                Message = "Nạp tiền thất bại!"
            };
            return BadRequest(response);
        }

        [HttpGet("payment/success")]
        
        public IActionResult PaymentSuccess()
        {
            var response = new OperationResult<bool>()
            {
                StatusCode = Service.Commons.StatusCode.Ok,
                Payload = true,
                Message = "Thanh toán thành công!"
            };
            return Ok(response);
        }
    }
}
