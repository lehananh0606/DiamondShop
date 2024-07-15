using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class ProductImageResponse
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int AuctionID { get; set; }
    }
}
