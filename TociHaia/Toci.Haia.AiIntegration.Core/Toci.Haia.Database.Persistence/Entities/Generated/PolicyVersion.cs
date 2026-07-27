using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class PolicyVersion
{
    public Guid PolicyVersionId { get; set; }

    public Guid PolicyDefinitionId { get; set; }

    public string VersionLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Config { get; set; } = null!;

    public DateTime? ActivatedAt { get; set; }

    public DateTime? RetiredAt { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual Account? CreatedByAccount { get; set; }

    public virtual ICollection<OnboardingFunnelSnapshot> OnboardingFunnelSnapshots { get; set; } = new List<OnboardingFunnelSnapshot>();

    public virtual ICollection<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; } = new List<OnboardingPriorHypothesis>();

    public virtual ICollection<OnboardingSession> OnboardingSessions { get; set; } = new List<OnboardingSession>();

    public virtual PolicyDefinition PolicyDefinition { get; set; } = null!;

    public virtual ICollection<RecoveryEvent> RecoveryEvents { get; set; } = new List<RecoveryEvent>();

    public virtual ICollection<SelectionDecision> SelectionDecisions { get; set; } = new List<SelectionDecision>();
}
