using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateClassificationSensitivity
{
    public Guid CandidateClassificationSensitivityId { get; set; }

    public Guid CandidateClassificationId { get; set; }

    public Guid SensitivityCategoryId { get; set; }

    public short SeverityLevel { get; set; }

    public decimal? Confidence { get; set; }

    public bool ModerationRelevance { get; set; }

    public string AssignmentSource { get; set; } = null!;

    public string? EditorialNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CandidateClassification CandidateClassification { get; set; } = null!;

    public virtual SensitivityCategory SensitivityCategory { get; set; } = null!;
}
