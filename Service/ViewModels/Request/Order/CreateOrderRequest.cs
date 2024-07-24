using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Order
{
    public class CreateOrderRequest
    {
        
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string AuctionTitle { get; set; }
        [Required]
        public string AuctionName { get; set; }
        [Required]
        public string AuctionCode { get; set; }
        [Required]
        public int Quantity { get; set; }
        public float Total {  get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }

        public bool IsExpired { get; set; }
        public string Note { get; set; }

        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpiredAt { get; set; } 

    }
}
