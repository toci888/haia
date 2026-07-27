using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class DrynessScaleLevel
{
    public Guid DrynessScaleLevelId { get; set; }

    public Guid DrynessScaleVersionId { get; set; }

    public short LevelNo { get; set; }

    public string Label { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<DrynessRating> DrynessRatings { get; set; } = new List<DrynessRating>();

    public virtual DrynessScaleVersion DrynessScaleVersion { get; set; } = null!;
}
