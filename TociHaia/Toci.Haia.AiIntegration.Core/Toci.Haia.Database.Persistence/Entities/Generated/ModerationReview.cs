using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ModerationReview
{
    public Guid ModerationReviewId { get; set; }

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Typed target pointer; enforced by target_type and application workflow, no single relational FK available.
    /// </summary>
    public Guid TargetId { get; set; }

    public string ModerationStatus { get; set; } = null!;

    public Guid? ModeratorAccountId { get; set; }

    public string? RiskLevel { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<ModerationDecision> ModerationDecisions { get; set; } = new List<ModerationDecision>();

    public virtual Account? ModeratorAccount { get; set; }
}
