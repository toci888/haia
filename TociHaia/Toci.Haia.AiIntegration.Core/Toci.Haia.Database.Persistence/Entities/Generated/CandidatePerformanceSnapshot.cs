using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidatePerformanceSnapshot
{
    public Guid CandidatePerformanceSnapshotId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public int SampleSize { get; set; }

    public long Exposures { get; set; }

    public long UniqueUsers { get; set; }

    public decimal? AvgRating { get; set; }

    public decimal? MedianRating { get; set; }

    public decimal? SkipRate { get; set; }

    public decimal? NotMyStyleRate { get; set; }

    public decimal? CompletionEffect { get; set; }

    public decimal? AbandonmentEffect { get; set; }

    public decimal? PredictedVsActualError { get; set; }

    public decimal? Confidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;
}
