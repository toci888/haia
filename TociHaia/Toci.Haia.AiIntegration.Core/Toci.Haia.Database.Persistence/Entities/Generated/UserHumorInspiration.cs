using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class UserHumorInspiration
{
    public Guid UserHumorInspirationId { get; set; }

    public Guid? OnboardingSessionId { get; set; }

    public Guid AccountId { get; set; }

    public string InspirationType { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string ContentText { get; set; } = null!;

    public string PrivacyStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? RemovedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual OnboardingSession? OnboardingSession { get; set; }

    public virtual ICollection<UserHumorInspirationAnalysis> UserHumorInspirationAnalyses { get; set; } = new List<UserHumorInspirationAnalysis>();
}
