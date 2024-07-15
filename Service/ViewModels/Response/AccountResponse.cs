using Service.ViewModels.AccountToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class AccountResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public AccountTokenResponse Tokens { get; set; }
    }
}
