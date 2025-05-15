using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class TodoList
{
    public int ListId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual User User { get; set; } = null!;
}
