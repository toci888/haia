using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class PolicyDefinition
{
    public Guid PolicyDefinitionId { get; set; }

    public string PolicyKey { get; set; } = null!;

    public string PolicyArea { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual PolicyVersion? PolicyVersion { get; set; }
}
