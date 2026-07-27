using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class SelectionDecision
{
    public Guid SelectionDecisionId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public int DecisionPositionNo { get; set; }

    public Guid? ScoringPolicyVersionId { get; set; }

    public Guid SelectedOnboardingCandidateVersionId { get; set; }

    public Guid? SelectedReactionPackVersionId { get; set; }

    public string? DecisionReason { get; set; }

    public string RankingMode { get; set; } = null!;

    public bool RecoveryModeEnabled { get; set; }

    public string? PriorSources { get; set; }

    public string? ScoreComponents { get; set; }

    public decimal? PredictedEnjoyment { get; set; }

    public decimal? PredictedInformationGain { get; set; }

    public decimal? Confidence { get; set; }

    public short? TopNCount { get; set; }

    public string? CorrelationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual ICollection<PredictionOutcome> PredictionOutcomes { get; set; } = new List<PredictionOutcome>();

    public virtual ICollection<RecoveryEvent> RecoveryEvents { get; set; } = new List<RecoveryEvent>();

    public virtual PolicyVersion? ScoringPolicyVersion { get; set; }

    public virtual OnboardingCandidateVersion SelectedOnboardingCandidateVersion { get; set; } = null!;

    public virtual ReactionPackVersion? SelectedReactionPackVersion { get; set; }

    public virtual ICollection<SelectionDecisionAlternative> SelectionDecisionAlternatives { get; set; } = new List<SelectionDecisionAlternative>();
}
