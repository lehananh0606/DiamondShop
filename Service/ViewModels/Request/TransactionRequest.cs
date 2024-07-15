using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class TransactionRequest
    {
        [Required]
        public string Resource { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public float Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string Status { get; set; }

        public string Content { get; set; }

        [Required]
        public string TransactionCode { get; set; }

        public string FailedReason { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? WalletID { get; set; }

        public int? OrderID { get; set; }
    }
}
