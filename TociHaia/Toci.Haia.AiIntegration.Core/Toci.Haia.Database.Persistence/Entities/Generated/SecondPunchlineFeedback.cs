using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class SecondPunchlineFeedback
{
    public Guid SecondPunchlineFeedbackId { get; set; }

    public Guid SecondPunchlineViewId { get; set; }

    public string FeedbackType { get; set; } = null!;

    public short? FeedbackValue { get; set; }

    public string? FeedbackText { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<HumorEvidence> HumorEvidences { get; set; } = new List<HumorEvidence>();

    public virtual SecondPunchlineView SecondPunchlineView { get; set; } = null!;
}
