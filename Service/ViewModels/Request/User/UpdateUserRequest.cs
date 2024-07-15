using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.User
{
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        

        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Dob { get; set; }



        public bool IsBanned { get; set; } = false;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddYears(1);
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        
    }
}
