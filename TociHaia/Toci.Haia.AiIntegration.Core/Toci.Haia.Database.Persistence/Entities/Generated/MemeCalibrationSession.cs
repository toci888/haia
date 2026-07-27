using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class MemeCalibrationSession
{
    public Guid MemeCalibrationSessionId { get; set; }

    public Guid OnboardingSessionId { get; set; }

    public Guid AccountId { get; set; }

    public Guid? MediaAssetId { get; set; }

    public bool? UserDeclaredFunny { get; set; }

    public string? MechanismAnalysis { get; set; }

    public string ModerationStatus { get; set; } = null!;

    public string PrivacyStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual MediaAsset? MediaAsset { get; set; }

    public virtual ICollection<MemeCalibrationVariant> MemeCalibrationVariants { get; set; } = new List<MemeCalibrationVariant>();

    public virtual OnboardingSession OnboardingSession { get; set; } = null!;
}
