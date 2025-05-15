using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int UserId { get; set; }

    public string ReportType { get; set; } = null!;

    public string? Parameters { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastViewed { get; set; }

    public virtual User User { get; set; } = null!;
}
