using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int UserId { get; set; }

    public string AccountName { get; set; } = null!;

    public string? AccountType { get; set; }

    public decimal Balance { get; set; }

    public string? Currency { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<RecurringTransaction> RecurringTransactions { get; } = new List<RecurringTransaction>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
