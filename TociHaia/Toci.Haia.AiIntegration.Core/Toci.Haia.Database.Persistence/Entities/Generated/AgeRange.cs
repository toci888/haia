using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AgeRange
{
    public Guid AgeRangeId { get; set; }

    public string RangeKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public short? MinAge { get; set; }

    public short? MaxAge { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AccountAgeContext> AccountAgeContexts { get; set; } = new List<AccountAgeContext>();
}
