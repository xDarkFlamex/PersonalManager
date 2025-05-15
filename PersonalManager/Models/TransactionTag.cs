using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TransactionTag
{
    public int TransactionTagId { get; set; }

    public int TransactionId { get; set; }

    public int TagId { get; set; }

    public virtual Tag Tag { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
