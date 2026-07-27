using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingSession
{
    public Guid OnboardingSessionId { get; set; }

    public Guid AccountId { get; set; }

    public Guid OnboardingFlowVersionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public string SourceRegistration { get; set; } = null!;

    public string CurrentStepKey { get; set; } = null!;

    public string SessionStatus { get; set; } = null!;

    public bool OptionalContextSkipped { get; set; }

    public bool MemeCalibrationSkipped { get; set; }

    public bool HumorInspirationSkipped { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? InterruptedAt { get; set; }

    public DateTime? ResumedAt { get; set; }

    public DateTime LastActivityAt { get; set; }

    public string? ResumeTokenHash { get; set; }

    public int? LastCompletedPosition { get; set; }

    public Guid? ScoringPolicyVersionId { get; set; }

    public long RowVersion { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual HumorDimensionModelVersion HumorDimensionModelVersion { get; set; } = null!;

    public virtual ICollection<HumorEvidence> HumorEvidenceOnboardingSessionNavigations { get; set; } = new List<HumorEvidence>();

    public virtual ICollection<HumorEvidence> HumorEvidenceOnboardingSessions { get; set; } = new List<HumorEvidence>();

    public virtual InitialHumorSnapshot? InitialHumorSnapshot { get; set; }

    public virtual ICollection<MemeCalibrationSession> MemeCalibrationSessions { get; set; } = new List<MemeCalibrationSession>();

    public virtual OnboardingFlowVersion OnboardingFlowVersion { get; set; } = null!;

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypothesisOnboardingSessionNavigations { get; set; } = new List<OnboardingPriorHypothesis>();

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypothesisOnboardingSessions { get; set; } = new List<OnboardingPriorHypothesis>();

    public virtual ICollection<OnboardingStepState> OnboardingStepStates { get; set; } = new List<OnboardingStepState>();

    public virtual ICollection<OnboardingWorkingHumorDimension> OnboardingWorkingHumorDimensionOnboardingSessionNavigations { get; set; } = new List<OnboardingWorkingHumorDimension>();

    public virtual ICollection<OnboardingWorkingHumorDimension> OnboardingWorkingHumorDimensionOnboardingSessions { get; set; } = new List<OnboardingWorkingHumorDimension>();

    public virtual ICollection<RecoveryEvent> RecoveryEvents { get; set; } = new List<RecoveryEvent>();

    public virtual PolicyVersion? ScoringPolicyVersion { get; set; }

    public virtual ICollection<SelectionDecision> SelectionDecisions { get; set; } = new List<SelectionDecision>();

    public virtual ICollection<UserHumorInspiration> UserHumorInspirations { get; set; } = new List<UserHumorInspiration>();
}
