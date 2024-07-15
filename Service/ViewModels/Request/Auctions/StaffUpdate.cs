using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Auctions
{
    public class StaffUpdate
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Valuation must be greater than 0.")]
        public float Valuation { get; set; }


        [Required] public int Duration { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Deposit price must be greater than 0.")]
        public float DepositPrice { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Start price must be greater than 0.")]
        public float StartPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Bidding price must be greater than 0.")]
        public float BiddingPrice { get; set; }

        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }

}
