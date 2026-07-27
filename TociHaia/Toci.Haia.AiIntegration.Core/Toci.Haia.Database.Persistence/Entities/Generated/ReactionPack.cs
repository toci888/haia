using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionPack
{
    public Guid ReactionPackId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public string PackKey { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string SourceType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;

    public virtual ReactionPackVersion? ReactionPackVersion { get; set; }
}
