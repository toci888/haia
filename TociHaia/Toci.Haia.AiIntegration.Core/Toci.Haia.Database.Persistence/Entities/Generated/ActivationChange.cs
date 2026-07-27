using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ActivationChange
{
    public Guid ActivationChangeId { get; set; }

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Typed target pointer; intentionally loose UUID to support activation history across aggregates.
    /// </summary>
    public Guid TargetId { get; set; }

    public string ActionType { get; set; } = null!;

    public Guid? ChangedByAccountId { get; set; }

    public string? PreviousState { get; set; }

    public string? NewState { get; set; }

    public string? Reason { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual Account? ChangedByAccount { get; set; }
}
