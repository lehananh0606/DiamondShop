using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Auction
{
    public int AuctionId { get; set; }

    public bool? IsActived { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Duration { get; set; }

    public DateTime? StartDate { get; set; }

    public int? Status { get; set; }

    public float? DepositPrice { get; set; }

    public int? Quantity { get; set; }

    public bool? IsRejected { get; set; }

    public string? RejecrReason { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ProductName { get; set; }

    public string? ProductCode { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public float? StartPrice { get; set; }

    public float? EndPrice { get; set; }

    public DateTime? RemindAt { get; set; }

    public string? Title { get; set; }

    public float? BiddingPrice { get; set; }

    public float? Valuation { get; set; }

    public bool? IsExpired { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public string? ResponsibleBy { get; set; }

    public bool? IsPaused { get; set; }

    public string? PauseReason { get; set; }

    public int? PauseDuration { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Order? Order { get; set; }

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
