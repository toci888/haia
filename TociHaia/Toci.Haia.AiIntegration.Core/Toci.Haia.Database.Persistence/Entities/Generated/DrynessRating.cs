using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class DrynessRating
{
    public Guid DrynessRatingId { get; set; }

    public Guid CandidatePresentationId { get; set; }

    public Guid AccountId { get; set; }

    public Guid DrynessScaleLevelId { get; set; }

    public DateTime SelectedAt { get; set; }

    public DateTime? UnselectedAt { get; set; }

    public string IdempotencyKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CandidatePresentation CandidatePresentation { get; set; } = null!;

    public virtual DrynessScaleLevel DrynessScaleLevel { get; set; } = null!;

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();
}
