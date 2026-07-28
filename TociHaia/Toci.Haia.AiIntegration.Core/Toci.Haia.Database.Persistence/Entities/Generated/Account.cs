using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Główna tożsamość konta HAIA. Status wielowartościowy zamiast pojedynczego is_active.
/// </summary>
public partial class Account
{
    public Guid AccountId { get; set; }

    public string AccountStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime StatusChangedAt { get; set; }

    public DateTime? PendingExpiresAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletionReason { get; set; }

    public long RowVersion { get; set; }

    public virtual AccountAgeContext? AccountAgeContext { get; set; }

    public virtual ICollection<AccountDocumentAcceptance> AccountDocumentAcceptances { get; set; } = new List<AccountDocumentAcceptance>();

    public virtual AccountEmail? AccountEmail { get; set; }

    public virtual AccountProfessionContext? AccountProfessionContext { get; set; }

    public virtual ICollection<AccountRoleAssignment> AccountRoleAssignmentAccounts { get; set; } = new List<AccountRoleAssignment>();

    public virtual ICollection<AccountRoleAssignment> AccountRoleAssignmentAssignedByAccounts { get; set; } = new List<AccountRoleAssignment>();

    public virtual ICollection<ActivationChange> ActivationChanges { get; set; } = new List<ActivationChange>();

    public virtual ICollection<AgeEligibilityRecord> AgeEligibilityRecords { get; set; } = new List<AgeEligibilityRecord>();

    public virtual ICollection<AiPromptTemplateVersion> AiPromptTemplateVersions { get; set; } = new List<AiPromptTemplateVersion>();

    public virtual ICollection<AuditEvent> AuditEvents { get; set; } = new List<AuditEvent>();

    public virtual ICollection<CandidateActivation> CandidateActivations { get; set; } = new List<CandidateActivation>();

    public virtual ICollection<CandidateClassification> CandidateClassificationCreatedByAccounts { get; set; } = new List<CandidateClassification>();

    public virtual ICollection<CandidateClassification> CandidateClassificationReviewedByAccounts { get; set; } = new List<CandidateClassification>();

    public virtual ICollection<CandidateFeedback> CandidateFeedbacks { get; set; } = new List<CandidateFeedback>();

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual ICollection<ClassificationModelVersion> ClassificationModelVersions { get; set; } = new List<ClassificationModelVersion>();

    public virtual ICollection<DrynessRating> DrynessRatings { get; set; } = new List<DrynessRating>();

    public virtual ICollection<EditorialReview> EditorialReviews { get; set; } = new List<EditorialReview>();

    public virtual ICollection<ExternalLogin> ExternalLogins { get; set; } = new List<ExternalLogin>();

    public virtual ICollection<InitialHumorSnapshot> InitialHumorSnapshots { get; set; } = new List<InitialHumorSnapshot>();

    public virtual ICollection<MemeCalibrationFeedback> MemeCalibrationFeedbacks { get; set; } = new List<MemeCalibrationFeedback>();

    public virtual ICollection<MemeCalibrationSession> MemeCalibrationSessions { get; set; } = new List<MemeCalibrationSession>();

    public virtual ICollection<ModerationDecision> ModerationDecisions { get; set; } = new List<ModerationDecision>();

    public virtual ICollection<ModerationReview> ModerationReviews { get; set; } = new List<ModerationReview>();

    public virtual ICollection<OnboardingCandidateVersion> OnboardingCandidateVersions { get; set; } = new List<OnboardingCandidateVersion>();

    public virtual ICollection<OnboardingCandidate> OnboardingCandidates { get; set; } = new List<OnboardingCandidate>();

    public virtual ICollection<OnboardingFlowVersion> OnboardingFlowVersions { get; set; } = new List<OnboardingFlowVersion>();

    public virtual ICollection<OnboardingSession> OnboardingSessions { get; set; } = new List<OnboardingSession>();

    public virtual PasswordCredential? PasswordCredential { get; set; }

    public virtual ICollection<PolicyVersion> PolicyVersions { get; set; } = new List<PolicyVersion>();

    public virtual PublicProfile? PublicProfile { get; set; }

    public virtual ICollection<ReactionChoice> ReactionChoices { get; set; } = new List<ReactionChoice>();

    public virtual ICollection<ReactionExposure> ReactionExposures { get; set; } = new List<ReactionExposure>();

    public virtual ICollection<ReactionPackVersion> ReactionPackVersions { get; set; } = new List<ReactionPackVersion>();

    public virtual ICollection<RecommendationDecision> RecommendationDecisions { get; set; } = new List<RecommendationDecision>();

    public virtual ICollection<SecurityToken> SecurityTokens { get; set; } = new List<SecurityToken>();

    public virtual ICollection<StudioComment> StudioComments { get; set; } = new List<StudioComment>();

    public virtual ICollection<UserHumorInspiration> UserHumorInspirations { get; set; } = new List<UserHumorInspiration>();
}
