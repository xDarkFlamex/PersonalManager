using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<NoteTag> NoteTags { get; } = new List<NoteTag>();

    public virtual ICollection<TaskTag> TaskTags { get; } = new List<TaskTag>();

    public virtual ICollection<TransactionTag> TransactionTags { get; } = new List<TransactionTag>();

    public virtual User User { get; set; } = null!;
}
