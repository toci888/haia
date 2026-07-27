using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionVersion
{
    public Guid ReactionVersionId { get; set; }

    public Guid ReactionId { get; set; }

    public int VersionNo { get; set; }

    public string ReactionLabel { get; set; } = null!;

    public string? Emoji { get; set; }

    public string StyleKey { get; set; } = null!;

    public short Intensity { get; set; }

    public bool IsReactionRescue { get; set; }

    public string? SecondPunchlineText { get; set; }

    public string? EditorialNote { get; set; }

    public string? SafetyFlags { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Reaction Reaction { get; set; } = null!;

    public virtual ICollection<ReactionPackItem> ReactionPackItems { get; set; } = new List<ReactionPackItem>();

    public virtual ICollection<ReactionPerformanceSnapshot> ReactionPerformanceSnapshots { get; set; } = new List<ReactionPerformanceSnapshot>();

    public virtual ICollection<ReactionMechanism> ReactionMechanisms { get; set; } = new List<ReactionMechanism>();
}
