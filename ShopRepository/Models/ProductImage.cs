using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class ProductImage
{
    public int ProductImageId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int AuctionId { get; set; }

    public virtual Auction Auction { get; set; } = null!;
}
