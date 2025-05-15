using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TaskSubtask
{
    public int SubtaskId { get; set; }

    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsCompleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Task Task { get; set; } = null!;
}
