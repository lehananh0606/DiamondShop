using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IServices;
using Service.Library;
using Service.ViewModels.Request;
using Service.ViewModels.Response;
using ShopRepository.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var time = DateTime.Now;
            var tick = time.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", time.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utilss.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", model.WalletId.ToString());
            vnpay.AddRequestData("vnp_OrderType", PaymentMethod.DEPOSIT.ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel { Success = false };
            }

            return new VnPaymentResponseModel
            {
                Success = true,
                WalletId = int.Parse(vnpay.GetResponseData("vnp_OrderInfo")),
                BankCode = vnpay.GetResponseData("vnp_BankCode"),
                BankTranNo = vnpay.GetResponseData("vnp_BankTranNo"),
                CardType = vnpay.GetResponseData("vnp_CardType"),
                Amount = Convert.ToDouble(vnpay.GetResponseData("vnp_Amount")) / 100,
                Token = vnp_SecureHash,
                VnPayResponseCode = vnpay.GetResponseData("vnp_ResponseCode")
            };
        }
    }
}
