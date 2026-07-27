using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingStepState
{
    public Guid OnboardingStepStateId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public string StepKey { get; set; } = null!;

    public string StepStatus { get; set; } = null!;

    public DateTime EnteredAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? SkippedAt { get; set; }

    public string? StepPayload { get; set; }

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;
}
