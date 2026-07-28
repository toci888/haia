using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities.Generated;

public partial class CandidateClassificationValue
{
    public Guid CandidateClassificationValueId { get; set; }

    public Guid CandidateClassificationId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid ClassificationModelAxisId { get; set; }

    public Guid ClassificationModelValueId { get; set; }

    public decimal RelevanceScore { get; set; }

    public decimal? Confidence { get; set; }

    public bool IsPrimary { get; set; }

    public int? RankNo { get; set; }

    public string AssignmentSource { get; set; } = null!;

    public string? EditorialNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CandidateClassification CandidateClassification { get; set; } = null!;

    public virtual ClassificationModelAxis ClassificationModelAxis { get; set; } = null!;

    public virtual ClassificationModelValue ClassificationModelValue { get; set; } = null!;
}
