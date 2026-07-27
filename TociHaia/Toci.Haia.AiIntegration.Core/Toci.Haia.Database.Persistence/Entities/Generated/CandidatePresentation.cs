using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidatePresentation
{
    public Guid CandidatePresentationId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid? SelectionDecisionId { get; set; }

    public Guid AccountId { get; set; }

    public int PositionNo { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public Guid? ReactionPackVersionId { get; set; }

    public DateTime DisplayedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public Guid? ScoringPolicyVersionId { get; set; }

    public string? SelectedByReason { get; set; }

    public string? RankingMode { get; set; }

    public decimal? PredictedEnjoyment { get; set; }

    public decimal? PredictedInformationGain { get; set; }

    public string? PriorSources { get; set; }

    public bool IsRecoveryPresentation { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CandidateFeedback? CandidateFeedback { get; set; }

    public virtual ICollection<DrynessRating> DrynessRatings { get; set; } = new List<DrynessRating>();

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual ICollection<ReactionChoice> ReactionChoices { get; set; } = new List<ReactionChoice>();

    public virtual ICollection<ReactionExposure> ReactionExposures { get; set; } = new List<ReactionExposure>();

    public virtual ReactionPackVersion? ReactionPackVersion { get; set; }

    public virtual PolicyVersion? ScoringPolicyVersion { get; set; }

    public virtual SelectionDecision? SelectionDecision { get; set; }
}
