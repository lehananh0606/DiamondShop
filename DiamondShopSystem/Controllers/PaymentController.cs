using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Service.Commons;
using Service.IServices;
using Service.ViewModels.Request;

namespace DiamondShopSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid payment request model.");
            }

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);

            return Ok(new { Url = paymentUrl });
        }

        [HttpGet("payment-execute")]
        public IActionResult PaymentExecute()
        {
            var query = Request.Query;
            var paymentResponse = _vnPayService.PaymentExecute(query);

            if (!paymentResponse.Success)
            {
                return BadRequest("Invalid payment response.");
            }

            return Ok(paymentResponse);
        }
        [HttpGet("payment/payment-callback")] // Unique route for PaymentCallBack
       
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction(nameof(PaymentFail));
            }

            // save payment
            //var responsePayment = _vnPayService.DepositPayment(response).Result;

            //if (responsePayment.IsError)
            //{
            //    return RedirectToAction(nameof(PaymentFail));
            //}

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
                Message = "Nạp tiền thành công!"
            };
            return Ok(response);
        }
    }
}
