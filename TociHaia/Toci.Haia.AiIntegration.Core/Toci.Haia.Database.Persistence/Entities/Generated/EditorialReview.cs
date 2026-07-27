using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class EditorialReview
{
    public Guid EditorialReviewId { get; set; }

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Typed target pointer; enforced by target_type and application workflow, no single relational FK available.
    /// </summary>
    public Guid TargetId { get; set; }

    public string ReviewStatus { get; set; } = null!;

    public Guid? ReviewerAccountId { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Account? ReviewerAccount { get; set; }
}
