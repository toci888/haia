using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ClassificationMeasureProjectionRule
{
    public Guid ClassificationMeasureProjectionRuleId { get; set; }

    public Guid ClassificationProjectionModelVersionId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid ClassificationModelAxisId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public decimal EffectWeight { get; set; }

    public string MappingMode { get; set; } = null!;

    public decimal? CenterValue { get; set; }

    public decimal? Confidence { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ClassificationModelAxis ClassificationModelAxis { get; set; } = null!;

    public virtual ClassificationProjectionModelVersion ClassificationProjectionModelVersion { get; set; } = null!;

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;
}
