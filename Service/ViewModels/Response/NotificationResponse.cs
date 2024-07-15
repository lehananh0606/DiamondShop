using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
        public bool IsReaded { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserID { get; set; }
    }
}
