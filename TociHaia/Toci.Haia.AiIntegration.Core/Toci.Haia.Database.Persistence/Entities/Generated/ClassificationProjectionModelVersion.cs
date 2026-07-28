using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ClassificationProjectionModelVersion
{
    public Guid ClassificationProjectionModelVersionId { get; set; }

    public string ProjectionModelKey { get; set; } = null!;

    public int VersionNo { get; set; }

    public string Status { get; set; } = null!;

    public Guid ClassificationModelVersionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public string? Description { get; set; }

    public string Config { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? RetiredAt { get; set; }

    public virtual ICollection<ClassificationMeasureProjectionRule> ClassificationMeasureProjectionRules { get; set; } = new List<ClassificationMeasureProjectionRule>();

    public virtual ClassificationModelVersion ClassificationModelVersion { get; set; } = null!;

    public virtual ICollection<ClassificationValueProjectionRule> ClassificationValueProjectionRules { get; set; } = new List<ClassificationValueProjectionRule>();

    public virtual HumorDimensionModelVersion HumorDimensionModelVersion { get; set; } = null!;
}
