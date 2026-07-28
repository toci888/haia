using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Aktywacja wartości słownikowej w wersji modelu wraz z wagą domyślną wartości.
/// </summary>
public partial class ClassificationModelValue
{
    public Guid ClassificationModelValueId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid ClassificationModelAxisId { get; set; }

    public Guid ClassificationAxisId { get; set; }

    public Guid ClassificationValueId { get; set; }

    /// <summary>
    /// Domyślna waga wartości (odrębna od wagi osi, relevance i confidence).
    /// </summary>
    public decimal DefaultWeight { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidateClassificationValue> CandidateClassificationValues { get; set; } = new List<CandidateClassificationValue>();

    public virtual ClassificationModelAxis ClassificationModelAxis { get; set; } = null!;

    public virtual ClassificationValue ClassificationValue { get; set; } = null!;

    public virtual ICollection<ClassificationValueProjectionRule> ClassificationValueProjectionRules { get; set; } = new List<ClassificationValueProjectionRule>();
}
