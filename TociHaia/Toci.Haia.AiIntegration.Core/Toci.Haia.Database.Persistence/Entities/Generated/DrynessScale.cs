using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class DrynessScale
{
    public Guid DrynessScaleId { get; set; }

    public string ScaleKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<DrynessScaleVersion> DrynessScaleVersions { get; set; } = new List<DrynessScaleVersion>();
}
