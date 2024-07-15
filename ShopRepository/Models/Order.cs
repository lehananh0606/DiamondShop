using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public float? Total { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? CreateBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? PaymentMethod { get; set; }

    public bool? IsExpired { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public string? AuctionTitle { get; set; }

    public string? AuctionName { get; set; }

    public string? AuctionCode { get; set; }

    public int? Quantity { get; set; }

    public string? Note { get; set; }

    public string? UserName { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UserId { get; set; }

    public int? AuctionId { get; set; }

    public int? Status { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
