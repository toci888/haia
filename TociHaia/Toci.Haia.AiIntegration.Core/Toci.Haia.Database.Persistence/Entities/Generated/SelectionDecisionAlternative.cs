using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class SelectionDecisionAlternative
{
    public Guid SelectionDecisionAlternativeId { get; set; }

    public Guid SelectionDecisionId { get; set; }

    public int RankNo { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public Guid? ReactionPackVersionId { get; set; }

    public decimal? TotalScore { get; set; }

    public string? Explanation { get; set; }

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;

    public virtual ReactionPackVersion? ReactionPackVersion { get; set; }

    public virtual SelectionDecision SelectionDecision { get; set; } = null!;
}
