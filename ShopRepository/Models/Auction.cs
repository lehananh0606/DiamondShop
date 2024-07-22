using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace ShopRepository.Models;

[FirestoreData]
public partial class Auction
{
    [FirestoreProperty]
    public int AuctionId { get; set; }

    public bool? IsActived { get; set; } = false;
    [FirestoreProperty]
    public DateTime? EndDate { get; set; }
    [FirestoreProperty]
    public int? Duration { get; set; } = 0;
    [FirestoreProperty]
    public DateTime? StartDate { get; set; }
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

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public float? StartPrice { get; set; }

    public float? EndPrice { get; set; }

    public DateTime? RemindAt { get; set; }
    [FirestoreProperty]
    public string? Title { get; set; }
    [FirestoreProperty]
    public float? BiddingPrice { get; set; }

    public float? Valuation { get; set; }

    public bool? IsExpired { get; set; } = false;

    [FirestoreProperty]
    public DateTime? ExpiredAt { get; set; }

    public string? ResponsibleBy { get; set; }
    [FirestoreProperty]
    public bool? IsPaused { get; set; } = false;
    [FirestoreProperty]
    public string? PauseReason { get; set; }
    [FirestoreProperty]
    public int? PauseDuration { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Order? Order { get; set; }
    [FirestoreProperty]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
