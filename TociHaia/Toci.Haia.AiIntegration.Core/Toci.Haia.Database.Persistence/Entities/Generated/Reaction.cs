using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class Reaction
{
    public Guid ReactionId { get; set; }

    public string ReactionKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ReactionVersion> ReactionVersions { get; set; } = new List<ReactionVersion>();
}
