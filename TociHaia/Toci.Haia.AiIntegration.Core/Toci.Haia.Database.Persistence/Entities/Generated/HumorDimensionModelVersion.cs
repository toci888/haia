using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class HumorDimensionModelVersion
{
    public Guid HumorDimensionModelVersionId { get; set; }

    public string ModelKey { get; set; } = null!;

    public string VersionLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Config { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<HumorDimensionModelMember> HumorDimensionModelMembers { get; set; } = new List<HumorDimensionModelMember>();

    public virtual ICollection<InitialHumorSnapshot> InitialHumorSnapshots { get; set; } = new List<InitialHumorSnapshot>();

    public virtual ICollection<OnboardingSession> OnboardingSessions { get; set; } = new List<OnboardingSession>();
}
