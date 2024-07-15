
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response.User
{
    public class GetUserResponse
    {
        public int TotalPages { get; set; }
        public int NumberItems { get; set; }
        public List<UserResponse> Users { get; set; }
    }
}
