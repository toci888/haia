using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Wyłącznie hash hasła; brak jawnych sekretów.
/// </summary>
public partial class PasswordCredential
{
    public Guid PasswordCredentialId { get; set; }

    public Guid AccountId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string HashAlgorithm { get; set; } = null!;

    public string HashFormatVersion { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ChangedAt { get; set; }

    public bool ForceChangeRequired { get; set; }

    public bool IsActive { get; set; }

    public virtual Account Account { get; set; } = null!;
}
