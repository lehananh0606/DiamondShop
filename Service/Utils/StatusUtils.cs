using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public static class StatusUtils
    {
        public static string ChangeUserStatus(int status)
        {
            if (status == (int)CustomerStatus.Status.INACTIVE)
            {
                return char.ToUpper(CustomerStatus.Status.INACTIVE.ToString()[0]) + CustomerStatus.Status.INACTIVE.ToString().ToLower().Substring(1);
            }
            else if (status == (int)CustomerStatus.Status.ACTIVE)
            {
                return char.ToUpper(CustomerStatus.Status.ACTIVE.ToString()[0]) + CustomerStatus.Status.ACTIVE.ToString().ToLower().Substring(1);
            }
            return char.ToUpper(CustomerStatus.Status.DISABLE.ToString()[0]) + CustomerStatus.Status.DISABLE.ToString().ToLower().Substring(1);

        }

        public static string ChangeAuctionStatus(int status)
        {
            if (status == (int)AuctionEnums.Status.PENDING)
            {
                return char.ToUpper(AuctionEnums.Status.PENDING.ToString()[0]) + AuctionEnums.Status.WAITING.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.WAITING)
            {
                return char.ToUpper(AuctionEnums.Status.WAITING.ToString()[0]) + AuctionEnums.Status.CONFIRM.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.WAITING)
            {
                return char.ToUpper(AuctionEnums.Status.WAITING.ToString()[0]) + AuctionEnums.Status.END.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.CONFIRM)
            {
                return char.ToUpper(AuctionEnums.Status.CONFIRM.ToString()[0]) + AuctionEnums.Status.APPROVE.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.APPROVE)
            {
                return char.ToUpper(AuctionEnums.Status.APPROVE.ToString()[0]) + AuctionEnums.Status.COMMING.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.COMMING)
            {
                return char.ToUpper(AuctionEnums.Status.COMMING.ToString()[0]) + AuctionEnums.Status.BIDDING.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.COMMING)
            {
                return char.ToUpper(AuctionEnums.Status.COMMING.ToString()[0]) + AuctionEnums.Status.END.ToString().ToLower().Substring(1);
            }
            else if (status == (int)AuctionEnums.Status.BIDDING)
            {
                return char.ToUpper(AuctionEnums.Status.BIDDING.ToString()[0]) + AuctionEnums.Status.END.ToString().ToLower().Substring(1);
            }
            return char.ToUpper(AuctionEnums.Status.END.ToString()[0]) + AuctionEnums.Status.END.ToString().ToLower().Substring(1);

        }

        public static string ChangeOrderStatus(int status)
        {
            if (status == (int)OrderEnums.Status.PENDING)
            {
                return char.ToUpper(OrderEnums.Status.PENDING.ToString()[0]) + OrderEnums.Status.APPROVE.ToString().ToLower().Substring(1);
            }
            else if (status == (int)OrderEnums.Status.PENDING)
            {
                return char.ToUpper(OrderEnums.Status.PENDING.ToString()[0]) + OrderEnums.Status.CANCEL.ToString().ToLower().Substring(1);
            }
            return char.ToUpper(OrderEnums.Status.CANCEL.ToString()[0]) + OrderEnums.Status.CANCEL.ToString().ToLower().Substring(1);

        }
    }
}
