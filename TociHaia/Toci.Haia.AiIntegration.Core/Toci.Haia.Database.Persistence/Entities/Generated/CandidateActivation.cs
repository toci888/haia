using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateActivation
{
    public Guid CandidateActivationId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public string ActivationScope { get; set; } = null!;

    public Guid? CohortDefinitionVersionId { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public long? MaxExposures { get; set; }

    public int? MaxFrequencyPerUser { get; set; }

    public string? ExperimentKey { get; set; }

    public bool IsEnabled { get; set; }

    public bool KillSwitch { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CohortDefinitionVersion? CohortDefinitionVersion { get; set; }

    public virtual Account? CreatedByAccount { get; set; }

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;
}
