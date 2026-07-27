using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateClassification
{
    public Guid CandidateClassificationId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public string? FormatKey { get; set; }

    public short? Intensity { get; set; }

    public short? Dryness { get; set; }

    public short? Complexity { get; set; }

    public decimal? UniversalityScore { get; set; }

    public decimal? ReactionRescuePotential { get; set; }

    public decimal? AgeHintStrength { get; set; }

    public decimal? ProfessionHintStrength { get; set; }

    public string? SafetyFlags { get; set; }

    public string? AdditionalTags { get; set; }

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;
}
