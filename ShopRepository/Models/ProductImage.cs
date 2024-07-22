using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

[FirestoreData]
public partial class ProductImage
{
    [FirestoreProperty]
    public int ProductImageId { get; set; }
    [FirestoreProperty]
    public string? ImageUrl { get; set; }
    [FirestoreProperty]
    public string? ImageCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int AuctionId { get; set; }

    public virtual Auction Auction { get; set; } = null!;
}
