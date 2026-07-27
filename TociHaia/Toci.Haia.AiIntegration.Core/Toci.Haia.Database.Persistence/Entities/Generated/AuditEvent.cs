using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Append-oriented audit table. UPDATE/DELETE blocked by trigger. INSERT-only role grants are deployment/infrastructure concern.
/// </summary>
public partial class AuditEvent
{
    public Guid AuditEventId { get; set; }

    public Guid? ActorAccountId { get; set; }

    public string? ActorRoleKey { get; set; }

    public string ActionKey { get; set; } = null!;

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Audit target pointer; intentionally polymorphic and resolved via target_type.
    /// </summary>
    public Guid? TargetId { get; set; }

    public string? PreviousState { get; set; }

    public string? NewState { get; set; }

    public string? Reason { get; set; }

    public string? CorrelationId { get; set; }

    public string? SourceApplication { get; set; }

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account? ActorAccount { get; set; }
}
