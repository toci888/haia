using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class HumorVerdict
{
    public Guid HumorVerdictId { get; set; }

    public Guid InitialHumorSnapshotId { get; set; }

    public string VerdictText { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public decimal? Confidence { get; set; }

    public string GeneratedBy { get; set; } = null!;

    public Guid? AiOperationExecutionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiOperationExecution? AiOperationExecution { get; set; }

    public virtual InitialHumorSnapshot InitialHumorSnapshot { get; set; } = null!;
}
