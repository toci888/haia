using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateVersionMedium
{
    public Guid CandidateVersionMediaId { get; set; }

    public Guid OnboardingCandidateVersionId { get; set; }

    public Guid MediaAssetId { get; set; }

    public string MediaRole { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public string? CaptionOverride { get; set; }

    public string? PresentationMetadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual MediaAsset MediaAsset { get; set; } = null!;

    public virtual OnboardingCandidateVersion OnboardingCandidateVersion { get; set; } = null!;
}
