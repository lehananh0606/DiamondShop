using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace ShopRepository.Models;
[FirestoreData]
public partial class Bid
{
    [FirestoreProperty]
    public int BidId { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }
    [FirestoreProperty]
    public float? BiddingPrice { get; set; }
    [FirestoreProperty]
    public int? Ratings { get; set; }
    [FirestoreProperty]
    public bool? IsTop1 { get; set; }

    public bool? IsDeleted { get; set; }
    [FirestoreProperty]
    public string? UserName { get; set; }
    [FirestoreProperty]
    public int? UserId { get; set; }

    public int? AuctionId { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual User? User { get; set; }
}
