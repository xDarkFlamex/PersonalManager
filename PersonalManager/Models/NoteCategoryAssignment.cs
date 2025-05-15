using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class NoteCategoryAssignment
{
    public int AssignmentId { get; set; }

    public int NoteId { get; set; }

    public int CategoryId { get; set; }

    public virtual NoteCategory Category { get; set; } = null!;

    public virtual Note Note { get; set; } = null!;
}
