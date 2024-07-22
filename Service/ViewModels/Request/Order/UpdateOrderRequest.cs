using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Order
{
    public class UpdateOrderRequest
    {
        [Required]
        public string AuctionTitle { get; set; }
        [Required]
        public string AuctionName { get; set; }
        [Required]
        public string AuctionCode { get; set; }
        [Required]
        public int Quantity { get; set; }

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

