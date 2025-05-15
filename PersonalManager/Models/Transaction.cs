using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionType { get; set; }

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsRecurring { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual TransactionCategory? Category { get; set; }

    public virtual ICollection<TransactionTag> TransactionTags { get; } = new List<TransactionTag>();
}
