using System;
using System.Collections.Generic;

namespace ShopRepository.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string? Resource { get; set; }

    public float? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public string? Content { get; set; }

    public string? TransactionCode { get; set; }

    public string? FailedReason { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public int? WalletId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Wallet? Wallet { get; set; }

    public string? TransactionType {  get; set; }
}
