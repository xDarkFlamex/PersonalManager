using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class UserProfile
{
    public int ProfileId { get; set; }

    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Timezone { get; set; }

    public string? CurrencyPreference { get; set; }

    public virtual User User { get; set; } = null!;
}
