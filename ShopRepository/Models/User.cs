using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? Dob { get; set; }

    public int? Status { get; set; }

    public bool? IsBanned { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ModifiedVersion { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual Wallet? Wallet { get; set; }
}
