using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateFeedback
{
    public Guid CandidateFeedbackId { get; set; }

    public Guid CandidatePresentationId { get; set; }

    public Guid AccountId { get; set; }

    public int FeedbackRevisionNo { get; set; }

    public short? RatingValue { get; set; }

    public short? LaughScore { get; set; }

    public bool? SkipFlag { get; set; }

    public bool? NotMyStyleFlag { get; set; }

    public bool? PredictableFlag { get; set; }

    public bool? TooLongFlag { get; set; }

    public bool? TooObviousFlag { get; set; }

    public bool? TooIntenseFlag { get; set; }

    public bool? DryFlag { get; set; }

    public string? FeedbackReason { get; set; }

    public string? ExplicitTextFeedback { get; set; }

    public DateTime SubmittedAt { get; set; }

    public bool IsCurrent { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CandidatePresentation CandidatePresentation { get; set; } = null!;

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();
}
