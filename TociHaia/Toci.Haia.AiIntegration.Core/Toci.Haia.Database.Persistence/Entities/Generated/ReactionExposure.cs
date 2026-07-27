using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionExposure
{
    public Guid ReactionExposureId { get; set; }

    public Guid CandidatePresentationId { get; set; }

    public Guid ReactionPackItemId { get; set; }

    public Guid AccountId { get; set; }

    public int SlotNo { get; set; }

    public string ExposureSource { get; set; } = null!;

    public int ExposureSequenceNo { get; set; }

    public DateTime VisibleFrom { get; set; }

    public DateTime? VisibleTo { get; set; }

    public string IdempotencyKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CandidatePresentation CandidatePresentation { get; set; } = null!;

    public virtual ICollection<ReactionChoice> ReactionChoices { get; set; } = new List<ReactionChoice>();

    public virtual ReactionPackItem ReactionPackItem { get; set; } = null!;
}
