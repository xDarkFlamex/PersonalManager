using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class NoteTag
{
    public int NoteTagId { get; set; }

    public int NoteId { get; set; }

    public int TagId { get; set; }

    public virtual Note Note { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
