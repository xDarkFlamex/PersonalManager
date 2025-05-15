using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string? Period { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? CurrentSpent { get; set; }

    public virtual TransactionCategory? Category { get; set; }

    public virtual User User { get; set; } = null!;
}
