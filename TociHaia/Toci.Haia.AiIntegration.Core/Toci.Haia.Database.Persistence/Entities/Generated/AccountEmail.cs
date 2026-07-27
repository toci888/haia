using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Adresy e-mail konta. Wspiera zmianę e-mail i case-insensitive uniqueness.
/// </summary>
public partial class AccountEmail
{
    public Guid AccountEmailId { get; set; }

    public Guid AccountId { get; set; }

    public string EmailOriginal { get; set; } = null!;

    public string EmailNormalized { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public DateTime? VerifiedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<SecurityToken> SecurityTokens { get; set; } = new List<SecurityToken>();
}
