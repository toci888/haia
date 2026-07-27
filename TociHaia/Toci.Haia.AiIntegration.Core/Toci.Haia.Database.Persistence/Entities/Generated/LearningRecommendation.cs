using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class LearningRecommendation
{
    public Guid LearningRecommendationId { get; set; }

    public string RecommendationType { get; set; } = null!;

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Typed target pointer for recommendation scope; generic FK intentionally deferred.
    /// </summary>
    public Guid? TargetId { get; set; }

    public DateTime? EvidenceRangeStart { get; set; }

    public DateTime? EvidenceRangeEnd { get; set; }

    public int? SampleSize { get; set; }

    public decimal? Confidence { get; set; }

    public decimal? PredictedImpact { get; set; }

    public string? RiskLevel { get; set; }

    public string? RecommendationPayload { get; set; }

    public string RecommendationStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RecommendationDecision> RecommendationDecisions { get; set; } = new List<RecommendationDecision>();
}
