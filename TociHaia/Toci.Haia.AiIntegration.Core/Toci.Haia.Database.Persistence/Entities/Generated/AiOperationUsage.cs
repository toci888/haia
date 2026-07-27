using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiOperationUsage
{
    public Guid AiOperationUsageId { get; set; }

    public Guid AiOperationExecutionId { get; set; }

    public int? InputTokens { get; set; }

    public int? OutputTokens { get; set; }

    public int? TotalTokens { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiOperationExecution AiOperationExecution { get; set; } = null!;
}
