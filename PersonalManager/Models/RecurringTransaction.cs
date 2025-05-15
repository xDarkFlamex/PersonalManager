using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class RecurringTransaction
{
    public int RecurringId { get; set; }

    public int UserId { get; set; }

    public int? AccountId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string? Frequency { get; set; }

    public DateOnly NextDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual TransactionCategory? Category { get; set; }

    public virtual User User { get; set; } = null!;
}
