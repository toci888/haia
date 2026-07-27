using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CohortDefinition
{
    public Guid CohortDefinitionId { get; set; }

    public string CohortKey { get; set; } = null!;

    public string CohortType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CohortDefinitionVersion> CohortDefinitionVersions { get; set; } = new List<CohortDefinitionVersion>();
}
