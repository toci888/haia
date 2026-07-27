using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class SecondPunchlineView
{
    public Guid SecondPunchlineViewId { get; set; }

    public Guid ReactionChoiceId { get; set; }

    public DateTime ViewedAt { get; set; }

    public int? DwellMs { get; set; }

    public string SourceAction { get; set; } = null!;

    public virtual ReactionChoice ReactionChoice { get; set; } = null!;

    public virtual ICollection<SecondPunchlineFeedback> SecondPunchlineFeedbacks { get; set; } = new List<SecondPunchlineFeedback>();
}
