using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class RecoveryPerformanceSnapshot
{
    public Guid RecoveryPerformanceSnapshotId { get; set; }

    public string RecoveryStrategy { get; set; } = null!;

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public long TriggerCount { get; set; }

    public long RetainedInFlowCount { get; set; }

    public long ImprovementDetectedCount { get; set; }

    public decimal? SuccessRate { get; set; }

    public decimal? Confidence { get; set; }

    public int SampleSize { get; set; }

    public DateTime CreatedAt { get; set; }
}
