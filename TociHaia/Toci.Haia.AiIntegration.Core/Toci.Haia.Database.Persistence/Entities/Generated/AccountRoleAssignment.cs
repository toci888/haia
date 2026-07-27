using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AccountRoleAssignment
{
    public Guid AccountRoleAssignmentId { get; set; }

    public Guid AccountId { get; set; }

    public Guid AccountRoleId { get; set; }

    public Guid? AssignedByAccountId { get; set; }

    public DateTime AssignedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual AccountRole AccountRole { get; set; } = null!;

    public virtual Account? AssignedByAccount { get; set; }
}
