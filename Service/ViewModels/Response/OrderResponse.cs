using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public float Total { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string AuctionTitle { get; set; }
        public string AuctionName { get; set; }
        public string AuctionCode { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public int UserID { get; set; }
        public int AuctionID { get; set; }
    }
}
