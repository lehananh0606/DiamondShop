using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Bid
{
    public class UpdateBidRequest
    {
        [Required]
        public float BiddingPrice { get; set; }
       
    }
}
