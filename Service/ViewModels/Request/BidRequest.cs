using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class BidRequest
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateAt { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdateAt { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bidding price must be greater than 0.")]
        public float BiddingPrice { get; set; }

        [Required]
        public int Ratings { get; set; }

        public bool IsTop1 { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int AuctionID { get; set; }
    }
}
