using Service.ViewModels.AccountToken;
using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response.User
{
    public class UserResponse
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Dob { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public bool IsBanned { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int WalletId {  get; set; }
        public int ModifiedVersion { get; set; }
        public AccountTokenResponse Tokens { get; set; }
    }
}
