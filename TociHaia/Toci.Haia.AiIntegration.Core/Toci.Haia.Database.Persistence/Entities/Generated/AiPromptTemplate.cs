using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AiPromptTemplate
{
    public Guid AiPromptTemplateId { get; set; }

    public string TemplateKey { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiPromptTemplateVersion? AiPromptTemplateVersion { get; set; }
}
