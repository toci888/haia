using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AccountProfessionContext
{
    public Guid AccountProfessionContextId { get; set; }

    public Guid AccountId { get; set; }

    public Guid? IndustryId { get; set; }

    public Guid? ProfessionId { get; set; }

    public string? FreeTextContext { get; set; }

    public string Source { get; set; } = null!;

    public decimal Confidence { get; set; }

    public bool IsActive { get; set; }

    public string Visibility { get; set; } = null!;

    public DateTime ProvidedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? RemovedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Industry? Industry { get; set; }

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; } = new List<OnboardingPriorHypothesis>();

    public virtual Profession? Profession { get; set; }
}
