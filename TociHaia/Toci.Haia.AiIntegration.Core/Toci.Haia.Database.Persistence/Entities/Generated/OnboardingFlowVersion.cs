using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class OnboardingFlowVersion
{
    public Guid OnboardingFlowVersionId { get; set; }

    public string FlowKey { get; set; } = null!;

    public string VersionLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Config { get; set; } = null!;

    public DateTime? ActivatedAt { get; set; }

    public DateTime? RetiredAt { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account? CreatedByAccount { get; set; }

    public virtual ICollection<OnboardingSession> OnboardingSessions { get; set; } = new List<OnboardingSession>();
}
