using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class NotificationRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Msg { get; set; }
        
        public bool IsReaded { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
