using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AccountRole
{
    public Guid AccountRoleId { get; set; }

    public string RoleKey { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public string RoleScope { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<AccountRoleAssignment> AccountRoleAssignments { get; set; } = new List<AccountRoleAssignment>();
}
