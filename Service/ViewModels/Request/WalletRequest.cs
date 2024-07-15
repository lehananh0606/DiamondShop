using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class WalletRequest
    {
        [Required]
        public int UserID { get; set; }
        
        public float Balance { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
    }
}
