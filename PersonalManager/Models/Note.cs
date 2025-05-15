using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsPinned { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<NoteAttachment> NoteAttachments { get; } = new List<NoteAttachment>();

    public virtual ICollection<NoteCategoryAssignment> NoteCategoryAssignments { get; } = new List<NoteCategoryAssignment>();

    public virtual ICollection<NoteTag> NoteTags { get; } = new List<NoteTag>();

    public virtual User User { get; set; } = null!;
}
