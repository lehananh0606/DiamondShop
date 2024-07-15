using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? Title { get; set; }

    public string? Msg { get; set; }

    public bool? IsReaded { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
