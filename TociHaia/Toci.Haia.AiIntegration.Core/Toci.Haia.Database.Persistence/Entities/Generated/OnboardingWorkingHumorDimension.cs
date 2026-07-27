using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingWorkingHumorDimension
{
    public Guid OnboardingWorkingHumorDimensionId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public decimal EstimatedValue { get; set; }

    public decimal Confidence { get; set; }

    public int PositiveEvidenceCount { get; set; }

    public int NegativeEvidenceCount { get; set; }

    public string? LastUpdateSource { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual OnboardingSession OnboardingSessionNavigation { get; set; } = null!;
}
