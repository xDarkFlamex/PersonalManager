using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class NoteCategory
{
    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<NoteCategoryAssignment> NoteCategoryAssignments { get; } = new List<NoteCategoryAssignment>();

    public virtual User User { get; set; } = null!;
}
