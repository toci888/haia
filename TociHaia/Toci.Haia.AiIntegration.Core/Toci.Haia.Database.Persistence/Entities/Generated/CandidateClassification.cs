using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateClassification
{
    public Guid CandidateClassificationId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public string? FormatKey { get; set; }

    /// <summary>
    /// LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=intensity).
    /// </summary>
    public short? Intensity { get; set; }

    /// <summary>
    /// LEGACY/DEPRECATED: zastępowane przez predicted_dryness_scale_level_id + predicted_dryness_confidence.
    /// </summary>
    public short? Dryness { get; set; }

    /// <summary>
    /// LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=complexity).
    /// </summary>
    public short? Complexity { get; set; }

    /// <summary>
    /// LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=universality).
    /// </summary>
    public decimal? UniversalityScore { get; set; }

    /// <summary>
    /// LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=rescue_potential).
    /// </summary>
    public decimal? ReactionRescuePotential { get; set; }

    /// <summary>
    /// LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.
    /// </summary>
    public decimal? AgeHintStrength { get; set; }

    /// <summary>
    /// LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.
    /// </summary>
    public decimal? ProfessionHintStrength { get; set; }

    /// <summary>
    /// LEGACY/raw AI payload. Kanoniczny safety model jest relacyjny w candidate_classification_sensitivity.
    /// </summary>
    public string? SafetyFlags { get; set; }

    /// <summary>
    /// Pozostaje dla rzadkich/eksperymentalnych metadanych (nie kanoniczna taksonomia).
    /// </summary>
    public string? AdditionalTags { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public int RevisionNo { get; set; }

    public string SourceType { get; set; } = null!;

    public string ClassificationStatus { get; set; } = null!;

    public decimal? OverallConfidence { get; set; }

    public Guid? AiOperationExecutionId { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public Guid? ReviewedByAccountId { get; set; }

    public Guid? SupersedesCandidateClassificationId { get; set; }

    public string? EditorialNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? SupersededAt { get; set; }

    /// <summary>
    /// Predykcja poziomu Sucharka dla materiału (nie mylić z realną oceną usera w humor.dryness_rating).
    /// </summary>
    public Guid? PredictedDrynessScaleLevelId { get; set; }

    public decimal? PredictedDrynessConfidence { get; set; }

    public virtual AiOperationExecution? AiOperationExecution { get; set; }

    public virtual ICollection<CandidateClassificationMeasure> CandidateClassificationMeasures { get; set; } = new List<CandidateClassificationMeasure>();

    public virtual ICollection<CandidateClassificationSensitivity> CandidateClassificationSensitivities { get; set; } = new List<CandidateClassificationSensitivity>();

    public virtual ICollection<CandidateClassificationValue> CandidateClassificationValues { get; set; } = new List<CandidateClassificationValue>();

    public virtual ClassificationModelVersion ClassificationModelVersion { get; set; } = null!;

    public virtual Account? CreatedByAccount { get; set; }

    public virtual ICollection<CandidateClassification> InverseSupersedesCandidateClassification { get; set; } = new List<CandidateClassification>();

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;

    public virtual DrynessScaleLevel? PredictedDrynessScaleLevel { get; set; }

    public virtual Account? ReviewedByAccount { get; set; }

    public virtual CandidateClassification? SupersedesCandidateClassification { get; set; }
}
