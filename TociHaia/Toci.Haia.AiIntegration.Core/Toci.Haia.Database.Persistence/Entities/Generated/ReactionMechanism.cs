using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionMechanism
{
    public Guid ReactionMechanismId { get; set; }

    public string MechanismKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<ReactionVersion> ReactionVersions { get; set; } = new List<ReactionVersion>();
}
