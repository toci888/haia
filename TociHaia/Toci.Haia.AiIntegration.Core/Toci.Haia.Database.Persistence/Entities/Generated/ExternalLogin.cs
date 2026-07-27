using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Powiązanie konta z providerami social login. Brak tokenów dostępowych.
/// </summary>
public partial class ExternalLogin
{
    public Guid ExternalLoginId { get; set; }

    public Guid AccountId { get; set; }

    public string Provider { get; set; } = null!;

    public string ProviderSubject { get; set; } = null!;

    public string? ProviderEmailHint { get; set; }

    public bool? ProviderEmailVerified { get; set; }

    public DateTime LinkedAt { get; set; }

    public DateTime? LastUsedAt { get; set; }

    public bool IsActive { get; set; }

    public string? Metadata { get; set; }

    public virtual Account Account { get; set; } = null!;
}
