using Service.ViewModels.AccountToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class AccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
