using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
