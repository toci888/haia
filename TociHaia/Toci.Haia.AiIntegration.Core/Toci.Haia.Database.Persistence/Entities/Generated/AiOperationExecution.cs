using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiOperationExecution
{
    public Guid AiOperationExecutionId { get; set; }

    public Guid? AiGenerationTargetId { get; set; }

    public string OperationName { get; set; } = null!;

    public string? CorrelationId { get; set; }

    public string Provider { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string? ProviderResponseId { get; set; }

    public Guid? AiPromptTemplateVersionId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int? DurationMs { get; set; }

    public string ExecutionStatus { get; set; } = null!;

    public int RetryCount { get; set; }

    public int RepairCount { get; set; }

    public string? ErrorCode { get; set; }

    public string? ImageSha256 { get; set; }

    public string? SourceMaterialRef { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AiGeneratedOutput> AiGeneratedOutputs { get; set; } = new List<AiGeneratedOutput>();

    public virtual AiGenerationTarget? AiGenerationTarget { get; set; }

    public virtual AiOperationUsage? AiOperationUsage { get; set; }

    public virtual AiPromptTemplateVersion? AiPromptTemplateVersion { get; set; }

    public virtual ICollection<HumorVerdict> HumorVerdicts { get; set; } = new List<HumorVerdict>();

    public virtual ICollection<MemeCalibrationVariant> MemeCalibrationVariants { get; set; } = new List<MemeCalibrationVariant>();

    public virtual ICollection<UserHumorInspirationAnalysis> UserHumorInspirationAnalyses { get; set; } = new List<UserHumorInspirationAnalysis>();
}
