using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Wersjonowany model klasyfikacji humoru materiału.
/// </summary>
public partial class ClassificationModelVersion
{
    public Guid ClassificationModelVersionId { get; set; }

    public string ModelKey { get; set; } = null!;

    public int VersionNo { get; set; }

    public string VersionLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>
    /// Konfiguracja niestabilna/eksperymentalna modelu (JSONB).
    /// </summary>
    public string Config { get; set; } = null!;

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? RetiredAt { get; set; }

    public virtual ICollection<CandidateClassification> CandidateClassifications { get; set; } = new List<CandidateClassification>();

    public virtual ICollection<ClassificationModelAxis> ClassificationModelAxes { get; set; } = new List<ClassificationModelAxis>();

    public virtual ICollection<ClassificationProjectionModelVersion> ClassificationProjectionModelVersions { get; set; } = new List<ClassificationProjectionModelVersion>();

    public virtual Account? CreatedByAccount { get; set; }
}
