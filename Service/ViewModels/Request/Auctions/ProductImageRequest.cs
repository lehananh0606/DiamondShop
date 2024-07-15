using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.ViewModels.Request
{
    public class ProductImageRequest
    {
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ImageCode { get; set; }
        
        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        
    }
}
