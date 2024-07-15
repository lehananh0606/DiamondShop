using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Request;

public class AuctionRequest
{
    [Required] public bool IsActived { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime EndDate { get; set; }

    [Required] public int Duration { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }


    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Deposit price must be greater than 0.")]
    public float DepositPrice { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    public bool IsRejected { get; set; }

    public string RejectReason { get; set; }

    public bool IsDeleted { get; set; }

    [Required] public string ProductName { get; set; }

    [Required] public string ProductCode { get; set; }

    public string Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreateAt { get; set; } = DateTime.Now;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime UpdateAt { get; set; } = DateTime.Now;

    [Required] public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }


    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Start price must be greater than 0.")]
    public float StartPrice { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "End price must be greater than 0.")]
    public float EndPrice { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime RemindAt { get; set; }

    [Required] public string Title { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Bidding price must be greater than 0.")]
    public float BiddingPrice { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Valuation must be greater than 0.")]
    public float Valuation { get; set; }

    public bool IsWon { get; set; }


    public bool IsExpired { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime ExpiredAt { get; set; }

    public string ResponsibleBy { get; set; }

    public bool IsPaused { get; set; }

    public string PauseReason { get; set; }

    public int PauseDuration { get; set; }

    public List<int> ProductImageIds { get; set; } // IDs of the associated product images
    public List<int> BidIds { get; set; } // IDs of the associated bids
    public int? OrderId { get; set; } // ID of the associated order, if any
}