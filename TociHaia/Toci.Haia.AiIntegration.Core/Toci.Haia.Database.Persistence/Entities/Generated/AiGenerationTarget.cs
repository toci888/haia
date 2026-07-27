using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiGenerationTarget
{
    public Guid AiGenerationTargetId { get; set; }

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Intentionally polymorphic typed-id. target_type determines concrete aggregate; no generic FK by design.
    /// </summary>
    public Guid? TargetId { get; set; }

    public Guid? OnboardingCandidateVersionId { get; set; }

    public Guid? ReactionPackVersionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AiOperationExecution> AiOperationExecutions { get; set; } = new List<AiOperationExecution>();

    public virtual OnboardingCandidateVersion? OnboardingCandidateVersion { get; set; }

    public virtual ReactionPackVersion? ReactionPackVersion { get; set; }
}
