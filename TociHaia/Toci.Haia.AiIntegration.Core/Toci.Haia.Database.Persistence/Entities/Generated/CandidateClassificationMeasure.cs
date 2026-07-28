using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class CandidateClassificationMeasure
{
    public Guid CandidateClassificationMeasureId { get; set; }

    public Guid CandidateClassificationId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid ClassificationModelAxisId { get; set; }

    public decimal NormalizedValue { get; set; }

    public decimal? Confidence { get; set; }

    public string MeasurementSource { get; set; } = null!;

    public string? EditorialNote { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CandidateClassification CandidateClassification { get; set; } = null!;

    public virtual ClassificationModelAxis ClassificationModelAxis { get; set; } = null!;
}
