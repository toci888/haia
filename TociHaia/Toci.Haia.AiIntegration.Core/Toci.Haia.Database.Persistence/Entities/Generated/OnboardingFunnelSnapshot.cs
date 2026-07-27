using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingFunnelSnapshot
{
    public Guid OnboardingFunnelSnapshotId { get; set; }

    public Guid? PolicyVersionId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public string? SourceRegistration { get; set; }

    public long StartedCount { get; set; }

    public long CompletedCount { get; set; }

    public long AbandonedCount { get; set; }

    public decimal? AvgTimeToCompleteSec { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual PolicyVersion? PolicyVersion { get; set; }
}
