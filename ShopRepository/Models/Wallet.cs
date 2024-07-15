﻿using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public float? Balance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
