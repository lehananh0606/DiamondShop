using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service.ViewModels.Request;

public class CreateAuctionRequest
{
    [Required] 
    public string ProductName { get; set; }
    
    [Required] 
    public string Title { get; set; }
    
    [Required] 
    public string Description { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
    
    [JsonIgnore]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreateAt { get; set; } = DateTime.Now;
    
    [JsonIgnore]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public List<ProductImageRequest> ProductImageRequests { get; set; } 
}