using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Bid
{
    public int BidId { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public float? BiddingPrice { get; set; }

    public int? Ratings { get; set; }

    public bool? IsTop1 { get; set; }

    public bool? IsDeleted { get; set; }

    public string? UserName { get; set; }

    public int? UserId { get; set; }

    public int? AuctionId { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual User? User { get; set; }
}
