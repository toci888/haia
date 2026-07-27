using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionPerformanceSnapshot
{
    public Guid ReactionPerformanceSnapshotId { get; set; }

    public Guid ReactionVersionId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public int SampleSize { get; set; }

    public long Exposures { get; set; }

    public long Clicks { get; set; }

    public decimal? Ctr { get; set; }

    public long? SecondPunchlineViews { get; set; }

    public long? ExplicitPositiveFeedback { get; set; }

    public long? ReactionRescueCases { get; set; }

    public decimal? ReactionRescueRate { get; set; }

    public decimal? Confidence { get; set; }

    public decimal? FatigueIndex { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ReactionVersion ReactionVersion { get; set; } = null!;
}
