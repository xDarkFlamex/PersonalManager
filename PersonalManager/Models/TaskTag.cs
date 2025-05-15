using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TaskTag
{
    public int TaskTagId { get; set; }

    public int TaskId { get; set; }

    public int TagId { get; set; }

    public virtual Tag Tag { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
