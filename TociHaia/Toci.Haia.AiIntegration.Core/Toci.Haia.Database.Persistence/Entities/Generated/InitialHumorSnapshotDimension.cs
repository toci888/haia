using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class InitialHumorSnapshotDimension
{
    public Guid InitialHumorSnapshotDimensionId { get; set; }

    public Guid InitialHumorSnapshotId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public decimal EstimatedValue { get; set; }

    public decimal Confidence { get; set; }

    public string ConfirmationLevel { get; set; } = null!;

    public int EvidenceCount { get; set; }

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;

    public virtual InitialHumorSnapshot InitialHumorSnapshot { get; set; } = null!;

    public virtual InitialHumorSnapshot InitialHumorSnapshotNavigation { get; set; } = null!;
}
