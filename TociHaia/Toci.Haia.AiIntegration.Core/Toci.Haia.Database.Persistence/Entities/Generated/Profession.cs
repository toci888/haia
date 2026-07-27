using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class Profession
{
    public Guid ProfessionId { get; set; }

    public Guid? IndustryId { get; set; }

    public string ProfessionKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AccountProfessionContext> AccountProfessionContexts { get; set; } = new List<AccountProfessionContext>();

    public virtual Industry? Industry { get; set; }
}
