using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Request.Auctions
{
    public class RegisterAuctionDTO
    {
    [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}
