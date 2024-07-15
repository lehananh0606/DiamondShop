using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class BidResponse
    {
        public int BidId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public float BiddingPrice { get; set; }
        public int Ratings { get; set; }
        public bool IsTop1 { get; set; }
        public bool IsDeleted { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public int AuctionID { get; set; }
    }
}
