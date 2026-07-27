using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionPackItem
{
    public Guid ReactionPackItemId { get; set; }

    public Guid ReactionPackVersionId { get; set; }

    public Guid ReactionVersionId { get; set; }

    public int PoolOrderNo { get; set; }

    public int? InitialSlotNo { get; set; }

    public int? ReplacementPriority { get; set; }

    public bool IsAvailableInMore { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ReactionChoice> ReactionChoices { get; set; } = new List<ReactionChoice>();

    public virtual ICollection<ReactionExposure> ReactionExposures { get; set; } = new List<ReactionExposure>();

    public virtual ReactionPackVersion ReactionPackVersion { get; set; } = null!;

    public virtual ReactionVersion ReactionVersion { get; set; } = null!;
}
