using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiGeneratedOutput
{
    public Guid AiGeneratedOutputId { get; set; }

    public Guid AiOperationExecutionId { get; set; }

    public string OutputType { get; set; } = null!;

    public string? StructuredOutput { get; set; }

    public string? MappedOutput { get; set; }

    public DateTime? RetentionUntil { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiOperationExecution AiOperationExecution { get; set; } = null!;
}
