using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class HumorDimensionModelMember
{
    public Guid HumorDimensionModelMemberId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid HumorDimensionDefinitionId { get; set; }

    public decimal Weight { get; set; }

    public decimal MinSupportedValue { get; set; }

    public decimal MaxSupportedValue { get; set; }

    public string MemberStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ClassificationMeasureProjectionRule> ClassificationMeasureProjectionRules { get; set; } = new List<ClassificationMeasureProjectionRule>();

    public virtual ICollection<ClassificationValueProjectionRule> ClassificationValueProjectionRules { get; set; } = new List<ClassificationValueProjectionRule>();

    public virtual HumorDimensionDefinition HumorDimensionDefinition { get; set; } = null!;

    public virtual HumorDimensionModelVersion HumorDimensionModelVersion { get; set; } = null!;

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();

    public virtual ICollection<InitialHumorSnapshotDimension> InitialHumorSnapshotDimensions { get; set; } = new List<InitialHumorSnapshotDimension>();

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; } = new List<OnboardingPriorHypothesis>();

    public virtual ICollection<OnboardingWorkingHumorDimension> OnboardingWorkingHumorDimensions { get; set; } = new List<OnboardingWorkingHumorDimension>();
}
