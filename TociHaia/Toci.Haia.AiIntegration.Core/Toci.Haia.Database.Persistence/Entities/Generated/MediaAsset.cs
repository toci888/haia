using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class MediaAsset
{
    public Guid MediaAssetId { get; set; }

    public string StorageKey { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public long ByteSize { get; set; }

    public string Sha256Hash { get; set; } = null!;

    public int? WidthPx { get; set; }

    public int? HeightPx { get; set; }

    public string ProcessingStatus { get; set; } = null!;

    public string RightsStatus { get; set; } = null!;

    public string ModerationStatus { get; set; } = null!;

    public string? AltText { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<CandidateVersionMedium> CandidateVersionMedia { get; set; } = new List<CandidateVersionMedium>();

    public virtual ICollection<MemeCalibrationSession> MemeCalibrationSessions { get; set; } = new List<MemeCalibrationSession>();
}
