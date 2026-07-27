using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class RecoveryEvent
{
    public Guid RecoveryEventId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public string TriggerType { get; set; } = null!;

    public string? TriggerDetails { get; set; }

    public string RecoveryStrategy { get; set; } = null!;

    public Guid? SelectionDecisionId { get; set; }

    public string? ResultType { get; set; }

    public bool? RetainedInFlow { get; set; }

    public bool? ImprovementDetected { get; set; }

    public Guid? RecoveryPolicyVersionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;

    public virtual PolicyVersion? RecoveryPolicyVersion { get; set; }

    public virtual SelectionDecision? SelectionDecision { get; set; }
}
