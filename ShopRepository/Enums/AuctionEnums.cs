using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRepository.Enums
{
    public class AuctionEnums
    {
        public enum Status
        {
            PENDING = 0,
            EVALUATE = 1,
            WAITING = 2,
            CONFIRM = 3,
            APPROVE = 4,
            COMMING = 5,
            BIDDING = 6,
            END = 7
        }
    }
}
