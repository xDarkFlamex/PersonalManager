using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalManager.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int ListId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual TodoList TodoList { get; set; } = null!;

    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    public virtual ICollection<TaskComment> TaskComments { get; } = new List<TaskComment>();

    public virtual ICollection<TaskSubtask> TaskSubtasks { get; } = new List<TaskSubtask>();

    public virtual ICollection<TaskTag> TaskTags { get; } = new List<TaskTag>();

    [Column("is_overdue")]
    public bool IsOverdue { get; set; }
}
