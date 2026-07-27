using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class StudioComment
{
    public Guid StudioCommentId { get; set; }

    public string TargetType { get; set; } = null!;

    /// <summary>
    /// Typed target pointer; intentionally loose UUID to support multiple reviewed aggregates.
    /// </summary>
    public Guid TargetId { get; set; }

    public Guid? AuthorAccountId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsInternal { get; set; }

    public virtual Account? AuthorAccount { get; set; }
}
