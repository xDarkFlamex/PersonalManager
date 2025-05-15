using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TaskComment
{
    public int CommentId { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
