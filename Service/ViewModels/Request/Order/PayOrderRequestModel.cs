using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Order
{
    public class PayOrderRequestModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
    }
}
