using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class UserHumorInspirationAnalysis
{
    public Guid UserHumorInspirationAnalysisId { get; set; }

    public Guid UserHumorInspirationId { get; set; }

    public string? StyleAnalysis { get; set; }

    public string? HumorDimensionImpact { get; set; }

    public Guid? AiOperationExecutionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiOperationExecution? AiOperationExecution { get; set; }

    public virtual UserHumorInspiration UserHumorInspiration { get; set; } = null!;
}
