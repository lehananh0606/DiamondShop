using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public int WalletId { get; set; }
        public string BankCode { get; set; }
        public string BankTranNo { get; set; }
        public string CardType { get; set; }
        public double Amount { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
    public class VnPaymentRequestModel
    {
        public int WalletId { get; set; }
        public double Amount { get; set; }

        public DateTime CreatedDate
        {
            get => DateTime.Now;
            init { }
        }
    }

    public record PaymentRequestCreateModel
    {
        public int? WalletId { get; init; }
        public FromToWallet From { get; init; }
        public FromToWallet To { get; init; }
        public decimal Amount { get; init; }
        public PaymentMethod Type { get; init; }
    };
}
