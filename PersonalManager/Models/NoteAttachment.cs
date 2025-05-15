using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class NoteAttachment
{
    public int AttachmentId { get; set; }

    public int NoteId { get; set; }

    public string FileUrl { get; set; } = null!;

    public string? FileType { get; set; }

    public string? FileName { get; set; }

    public int? FileSize { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Note Note { get; set; } = null!;
}
