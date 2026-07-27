using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingCandidateVersion
{
    public Guid OnboardingCandidateVersionId { get; set; }

    public Guid OnboardingCandidateId { get; set; }

    public int VersionNo { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string? Title { get; set; }

    public string? BodyText { get; set; }

    public string? CaptionText { get; set; }

    public string? SituationDescription { get; set; }

    public string RoleInFlow { get; set; } = null!;

    public string EditorialStatus { get; set; } = null!;

    public string ModerationStatus { get; set; } = null!;

    public string SafetyClassification { get; set; } = null!;

    public string RightsStatus { get; set; } = null!;

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool IsSignificantChange { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AiGenerationTarget> AiGenerationTargets { get; set; } = new List<AiGenerationTarget>();

    public virtual ICollection<CandidateActivation> CandidateActivations { get; set; } = new List<CandidateActivation>();

    public virtual CandidateClassification? CandidateClassification { get; set; }

    public virtual ICollection<CandidatePerformanceSnapshot> CandidatePerformanceSnapshots { get; set; } = new List<CandidatePerformanceSnapshot>();

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual ICollection<CandidateVersionMedium> CandidateVersionMedia { get; set; } = new List<CandidateVersionMedium>();

    public virtual ICollection<CohortCandidatePerformanceSnapshot> CohortCandidatePerformanceSnapshots { get; set; } = new List<CohortCandidatePerformanceSnapshot>();

    public virtual Account? CreatedByAccount { get; set; }

    public virtual OnboardingCandidate OnboardingCandidate { get; set; } = null!;

    public virtual ICollection<OnboardingCandidate> OnboardingCandidates { get; set; } = new List<OnboardingCandidate>();

    public virtual ICollection<ReactionPack> ReactionPacks { get; set; } = new List<ReactionPack>();

    public virtual ICollection<SelectionDecisionAlternative> SelectionDecisionAlternatives { get; set; } = new List<SelectionDecisionAlternative>();

    public virtual ICollection<SelectionDecision> SelectionDecisions { get; set; } = new List<SelectionDecision>();
}
