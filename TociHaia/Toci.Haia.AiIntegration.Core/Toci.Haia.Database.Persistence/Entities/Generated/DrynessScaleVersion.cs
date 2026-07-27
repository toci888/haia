using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class DrynessScaleVersion
{
    public Guid DrynessScaleVersionId { get; set; }

    public Guid DrynessScaleId { get; set; }

    public int VersionNo { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual DrynessScale DrynessScale { get; set; } = null!;

    public virtual ICollection<DrynessScaleLevel> DrynessScaleLevels { get; set; } = new List<DrynessScaleLevel>();
}
