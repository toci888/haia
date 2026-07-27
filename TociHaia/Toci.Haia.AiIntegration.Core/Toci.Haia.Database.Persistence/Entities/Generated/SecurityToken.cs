using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Tokeny bezpieczeństwa przechowywane jako hash; wspiera idempotencję aktywacji i resend.
/// </summary>
public partial class SecurityToken
{
    public Guid SecurityTokenId { get; set; }

    public Guid AccountId { get; set; }

    public Guid? AccountEmailId { get; set; }

    public string TokenPurpose { get; set; } = null!;

    public string TokenHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime? InvalidatedAt { get; set; }

    public int ResendSequence { get; set; }

    public string? RequestCorrelationId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual AccountEmail? AccountEmail { get; set; }
}
