using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class ReactionPackVersion
{
    public Guid ReactionPackVersionId { get; set; }

    public Guid ReactionPackId { get; set; }

    public int VersionNo { get; set; }

    public string Status { get; set; } = null!;

    public int TargetPoolSize { get; set; }

    public int InitialVisibleCount { get; set; }

    public int? MaxActiveSelections { get; set; }

    public string DrynessInteractionMode { get; set; } = null!;

    public Guid? PromptTemplateVersionId { get; set; }

    public string EditorialStatus { get; set; } = null!;

    public string ModerationStatus { get; set; } = null!;

    public DateTime? ActivatedAt { get; set; }

    public DateTime? WithdrawnAt { get; set; }

    public Guid? CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AiGenerationTarget> AiGenerationTargets { get; set; } = new List<AiGenerationTarget>();

    public virtual ICollection<CandidatePresentation> CandidatePresentations { get; set; } = new List<CandidatePresentation>();

    public virtual Account? CreatedByAccount { get; set; }

    public virtual AiPromptTemplateVersion? PromptTemplateVersion { get; set; }

    public virtual ReactionPack ReactionPack { get; set; } = null!;

    public virtual ICollection<ReactionPackItem> ReactionPackItems { get; set; } = new List<ReactionPackItem>();

    public virtual ICollection<SelectionDecisionAlternative> SelectionDecisionAlternatives { get; set; } = new List<SelectionDecisionAlternative>();

    public virtual ICollection<SelectionDecision> SelectionDecisions { get; set; } = new List<SelectionDecision>();
}
