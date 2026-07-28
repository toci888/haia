using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities.Generated;

public partial class ClassificationValueProjectionRule
{
    public Guid ClassificationValueProjectionRuleId { get; set; }

    public Guid ClassificationProjectionModelVersionId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid HumorDimensionModelVersionId { get; set; }

    public Guid ClassificationModelValueId { get; set; }

    public Guid HumorDimensionModelMemberId { get; set; }

    public decimal EffectWeight { get; set; }

    public decimal? Confidence { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ClassificationModelValue ClassificationModelValue { get; set; } = null!;

    public virtual ClassificationProjectionModelVersion ClassificationProjectionModelVersion { get; set; } = null!;

    public virtual HumorDimensionModelMember HumorDimensionModelMember { get; set; } = null!;
}
