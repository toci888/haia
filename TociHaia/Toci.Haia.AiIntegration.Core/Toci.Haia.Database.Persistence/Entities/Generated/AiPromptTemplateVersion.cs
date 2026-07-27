using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiPromptTemplateVersion
{
    public Guid AiPromptTemplateVersionId { get; set; }

    public Guid AiPromptTemplateId { get; set; }

    public string VersionLabel { get; set; } = null!;

    public string PromptBody { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AiOperationExecution> AiOperationExecutions { get; set; } = new List<AiOperationExecution>();

    public virtual AiPromptTemplate AiPromptTemplate { get; set; } = null!;

    public virtual Account? CreatedByAccount { get; set; }

    public virtual ICollection<ReactionPackVersion> ReactionPackVersions { get; set; } = new List<ReactionPackVersion>();
}
