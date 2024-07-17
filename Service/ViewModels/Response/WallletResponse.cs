using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Response
{
    public class WallletResponse
    {
        public int UserId { get; set; }

        public float Balance { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
