using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionChoice
{
    public Guid ReactionChoiceId { get; set; }

    public Guid CandidatePresentationId { get; set; }

    public Guid ReactionPackItemId { get; set; }

    public Guid AccountId { get; set; }

    public Guid SourceReactionExposureId { get; set; }

    public int SelectionOrderNo { get; set; }

    public string SelectionContext { get; set; } = null!;

    public DateTime SelectedAt { get; set; }

    public DateTime? UnselectedAt { get; set; }

    public string? UnselectedReason { get; set; }

    public string IdempotencyKey { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual CandidatePresentation CandidatePresentation { get; set; } = null!;

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();

    public virtual ReactionExposure ReactionExposure { get; set; } = null!;

    public virtual ReactionPackItem ReactionPackItem { get; set; } = null!;

    public virtual ICollection<SecondPunchlineView> SecondPunchlineViews { get; set; } = new List<SecondPunchlineView>();
}
