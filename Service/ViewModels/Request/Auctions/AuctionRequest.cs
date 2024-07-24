using Google.Cloud.Firestore;
using ShopRepository.Models;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Request;
[FirestoreData]
public class AuctionRequest
{
    [FirestoreProperty]
    public int AuctionId { get; set; }

    public bool? IsActived { get; set; } = false;
    [FirestoreProperty]
    public string? EndDate { get; set; }
    [FirestoreProperty]
    public int? Duration { get; set; } = 0;
    [FirestoreProperty]
    public string? StartDate { get; set; }
    [FirestoreProperty]
    public int? Status { get; set; } = 0;
    [FirestoreProperty]
    public float? DepositPrice { get; set; }
    [FirestoreProperty]
    public int? Quantity { get; set; }

    public bool? IsRejected { get; set; } = false;

    public string? RejecrReason { get; set; }

    public bool? IsDeleted { get; set; }
    [FirestoreProperty]
    public string? ProductName { get; set; }

    public string? ProductCode { get; set; }
    [FirestoreProperty]
    public string? Description { get; set; }

    public string? CreateAt { get; set; }

    public string? UpdateAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public float? StartPrice { get; set; }

    public float? EndPrice { get; set; }

    public string? RemindAt { get; set; }
    [FirestoreProperty]
    public string? Title { get; set; }
    [FirestoreProperty]
    public float? BiddingPrice { get; set; }

    public float? Valuation { get; set; }

    public bool? IsExpired { get; set; } = false;

    [FirestoreProperty]
    public string? ExpiredAt { get; set; }

    public string? ResponsibleBy { get; set; }
    [FirestoreProperty]
    public bool? IsPaused { get; set; } = false;
    [FirestoreProperty]
    public string? PauseReason { get; set; }
    [FirestoreProperty]
    public int? PauseDuration { get; set; }
    [FirestoreProperty]
    public virtual ICollection<ShopRepository.Models.Bid> Bids { get; set; } = new List<ShopRepository.Models.Bid>();

    [FirestoreProperty]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}