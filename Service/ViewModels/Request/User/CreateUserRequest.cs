using ShopRepository.Enums;
using ShopRepository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.User
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone must start with 0 and be 11 digits long.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone must be 11 characters long.")]
        public string Phone {  get; set; }
        [Required]
        public string Address { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Dob { get; set; }



        public bool IsBanned { get; set; } = false;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddYears(1);
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public int Status { get; set; }

        
        public int ModifiedVersion { get; set; }

      
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime ExpirationAt { get; set; } = DateTime.Now.AddYears(1);
    }
}
