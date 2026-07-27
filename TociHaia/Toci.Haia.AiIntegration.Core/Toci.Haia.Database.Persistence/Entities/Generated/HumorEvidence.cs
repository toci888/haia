using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class HumorEvidence
{
    public Guid HumorEvidenceId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public string EvidenceType { get; set; } = null!;

    public Guid? CandidatePresentationId { get; set; }

    public Guid? CandidateFeedbackId { get; set; }

    public Guid? ReactionChoiceId { get; set; }

    public Guid? SecondPunchlineFeedbackId { get; set; }

    public Guid? DrynessRatingId { get; set; }

    public string EvidenceDirection { get; set; } = null!;

    public decimal EvidenceWeight { get; set; }

    public string? Explanation { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CandidateFeedback? CandidateFeedback { get; set; }

    public virtual CandidatePresentation? CandidatePresentation { get; set; }

    public virtual DrynessRating? DrynessRating { get; set; }

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual OnboardingSession OnboardingSessionNavigation { get; set; } = null!;

    public virtual ReactionChoice? ReactionChoice { get; set; }

    public virtual SecondPunchlineFeedback? SecondPunchlineFeedback { get; set; }
}
