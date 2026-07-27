using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class InitialHumorSnapshot
{
    public Guid InitialHumorSnapshotId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid AccountId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public int SnapshotVersion { get; set; }

    public Guid? SupersedesSnapshotId { get; set; }

    public decimal OverallConfidence { get; set; }

    public string? ConfirmedTraitsSummary { get; set; }

    public string? WeakHypothesisSummary { get; set; }

    public string? UnexploredAreasSummary { get; set; }

    public string? PresentationJson { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual HumorDimensionModelVersion HumorDimensionModelVersion { get; set; } = null!;

    public virtual HumorVerdict? HumorVerdict { get; set; }

    public virtual ICollection<InitialHumorSnapshotDimension> InitialHumorSnapshotDimensionInitialHumorSnapshotNavigations { get; set; } = new List<InitialHumorSnapshotDimension>();

    public virtual ICollection<InitialHumorSnapshotDimension> InitialHumorSnapshotDimensionInitialHumorSnapshots { get; set; } = new List<InitialHumorSnapshotDimension>();

    public virtual ICollection<InitialHumorSnapshot> InverseSupersedesSnapshot { get; set; } = new List<InitialHumorSnapshot>();

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual InitialHumorSnapshot? SupersedesSnapshot { get; set; }
}
