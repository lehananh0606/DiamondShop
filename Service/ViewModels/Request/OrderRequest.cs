using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class OrderRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0.")]
        public float Total { get; set; }

       


        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must be 10 characters long.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Invalid phone number format. 0[3|5|7|8|9] + 8 digits.")]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public string CreateBy { get; set; }

        public string ModifiedBy { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public bool IsExpired { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ExpiredAt { get; set; }

        [Required]
        public string AuctionTitle { get; set; }

        [Required]
        public string AuctionName { get; set; }

        [Required]
        public string AuctionCode { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        public string Note { get; set; }

        [Required]
        public string UserName { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int AuctionID { get; set; }
    }
}
