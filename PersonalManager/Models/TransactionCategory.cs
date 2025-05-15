using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TransactionCategory
{
    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public string? Type { get; set; }

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public virtual ICollection<Budget> Budgets { get; } = new List<Budget>();

    public virtual ICollection<TransactionCategory> InverseParent { get; } = new List<TransactionCategory>();

    public virtual TransactionCategory? Parent { get; set; }

    public virtual ICollection<RecurringTransaction> RecurringTransactions { get; } = new List<RecurringTransaction>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
