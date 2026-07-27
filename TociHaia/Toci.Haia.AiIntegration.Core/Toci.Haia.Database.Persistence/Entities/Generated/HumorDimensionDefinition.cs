using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class HumorDimensionDefinition
{
    public Guid HumorDimensionDefinitionId { get; set; }

    public string DimensionKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? DimensionGroup { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<HumorDimensionModelMember> HumorDimensionModelMembers { get; set; } = new List<HumorDimensionModelMember>();
}
