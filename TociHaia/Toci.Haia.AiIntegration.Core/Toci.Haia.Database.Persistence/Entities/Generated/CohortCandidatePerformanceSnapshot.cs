using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CohortCandidatePerformanceSnapshot
{
    public Guid CohortCandidatePerformanceSnapshotId { get; set; }

    public Guid CohortDefinitionVersionId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public int SampleSize { get; set; }

    public long Exposures { get; set; }

    public decimal? AvgRating { get; set; }

    public decimal? SkipRate { get; set; }

    public decimal? CompletionRate { get; set; }

    public decimal? Confidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CohortDefinitionVersion CohortDefinitionVersion { get; set; } = null!;

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;
}
