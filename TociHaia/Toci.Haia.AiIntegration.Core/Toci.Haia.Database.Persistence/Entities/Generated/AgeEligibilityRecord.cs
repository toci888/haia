using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Warstwa compliance wieku. Nie służy do modelowania preferencji humoru.
/// </summary>
public partial class AgeEligibilityRecord
{
    public Guid AgeEligibilityRecordId { get; set; }

    /// <summary>
    /// Legal retention: ON DELETE RESTRICT, rekord compliance nie może znikać przez CASCADE konta.
    /// </summary>
    public Guid AccountId { get; set; }

    public string PolicyVersion { get; set; } = null!;

    public string EligibilityStatus { get; set; } = null!;

    public string DeterminationSource { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public DateTime DeterminedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Account Account { get; set; } = null!;
}
