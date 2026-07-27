using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class RecommendationDecision
{
    public Guid RecommendationDecisionId { get; set; }

    public Guid LearningRecommendationId { get; set; }

    public Guid? DecidedByAccountId { get; set; }

    public string DecisionStatus { get; set; } = null!;

    public string? DecisionReason { get; set; }

    public string? ExecutedAction { get; set; }

    public DateTime DecidedAt { get; set; }

    public virtual Account? DecidedByAccount { get; set; }

    public virtual LearningRecommendation LearningRecommendation { get; set; } = null!;
}
