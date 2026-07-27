using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingCandidate
{
    public Guid OnboardingCandidateId { get; set; }

    public string CandidateKey { get; set; } = null!;

    public string ContentFormat { get; set; } = null!;

    public string CandidateStatus { get; set; } = null!;

    public Guid? CreatedByAccountId { get; set; }

    public Guid? ApprovedVersionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual OnboardingCandidateVersion? ApprovedVersion { get; set; }

    public virtual Account? CreatedByAccount { get; set; }

    public virtual ICollection<OnboardingCandidateVersion> OnboardingCandidateVersions { get; set; } = new List<OnboardingCandidateVersion>();
}
