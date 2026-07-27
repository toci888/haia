using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class Industry
{
    public Guid IndustryId { get; set; }

    public string IndustryKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AccountProfessionContext> AccountProfessionContexts { get; set; } = new List<AccountProfessionContext>();

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
