using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ModerationDecision
{
    public Guid ModerationDecisionId { get; set; }

    public Guid ModerationReviewId { get; set; }

    public string DecisionType { get; set; } = null!;

    public string? DecisionReason { get; set; }

    public Guid? ModeratorAccountId { get; set; }

    public DateTime DecidedAt { get; set; }

    public virtual ModerationReview ModerationReview { get; set; } = null!;

    public virtual Account? ModeratorAccount { get; set; }
}
