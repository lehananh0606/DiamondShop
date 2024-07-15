using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class TransactionResponse
    {
        public int TransactionId { get; set; }
        public string Resource { get; set; }
        public float Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string TransactionCode { get; set; }
        public string FailedReason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? WalletID { get; set; }
        public int? OrderID { get; set; }
    }
}
