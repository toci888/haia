using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingPriorHypothesis
{
    public Guid OnboardingPriorHypothesisId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public string SourceType { get; set; } = null!;

    public Guid? AccountAgeContextId { get; set; }

    public Guid? AccountProfessionContextId { get; set; }

    public Guid? CohortDefinitionVersionId { get; set; }

    public decimal EstimatedValue { get; set; }

    public decimal Confidence { get; set; }

    public decimal AppliedWeight { get; set; }

    public Guid? PolicyVersionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AccountAgeContext? AccountAgeContext { get; set; }

    public virtual AccountProfessionContext? AccountProfessionContext { get; set; }

    public virtual CohortDefinitionVersion? CohortDefinitionVersion { get; set; }

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual OnboardingSession OnboardingSessionNavigation { get; set; } = null!;

    public virtual PolicyVersion? PolicyVersion { get; set; }
}
