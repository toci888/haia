using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CohortDefinitionVersion
{
    public Guid CohortDefinitionVersionId { get; set; }

    public Guid CohortDefinitionId { get; set; }

    public string VersionLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string DefinitionRules { get; set; } = null!;

    public int MinimumSampleSize { get; set; }

    public DateTime? ActiveFrom { get; set; }

    public DateTime? ActiveTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidateActivation> CandidateActivations { get; set; } = new List<CandidateActivation>();

    public virtual ICollection<CohortCandidatePerformanceSnapshot> CohortCandidatePerformanceSnapshots { get; set; } = new List<CohortCandidatePerformanceSnapshot>();

    public virtual CohortDefinition CohortDefinition { get; set; } = null!;

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; } = new List<OnboardingPriorHypothesis>();
}
