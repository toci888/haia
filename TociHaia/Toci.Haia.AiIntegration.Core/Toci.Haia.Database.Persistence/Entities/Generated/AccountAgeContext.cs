using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Opcjonalny Age Context Seed do personalizacji; odseparowany od compliance.
/// </summary>
public partial class AccountAgeContext
{
    public Guid AccountAgeContextId { get; set; }

    public Guid AccountId { get; set; }

    public Guid AgeRangeId { get; set; }

    public string Source { get; set; } = null!;

    public decimal StartWeight { get; set; }

    public decimal Confidence { get; set; }

    public bool IsActive { get; set; }

    public DateTime ProvidedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? RemovedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual AgeRange AgeRange { get; set; } = null!;

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; } = new List<OnboardingPriorHypothesis>();
}
