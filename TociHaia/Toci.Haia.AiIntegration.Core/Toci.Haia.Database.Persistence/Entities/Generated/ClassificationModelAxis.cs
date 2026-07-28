using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Członkostwo osi w konkretnej wersji modelu wraz z wagą osi.
/// </summary>
public partial class ClassificationModelAxis
{
    public Guid ClassificationModelAxisId { get; set; }

    public Guid ClassificationModelVersionId { get; set; }

    public Guid ClassificationAxisId { get; set; }

    /// <summary>
    /// Waga osi na poziomie wersji modelu (globalna ważność sygnału).
    /// </summary>
    public decimal AxisWeight { get; set; }

    public bool IsRequired { get; set; }

    public int MinimumAssignments { get; set; }

    public int? MaximumAssignments { get; set; }

    /// <summary>
    /// Metoda normalizacji agregacji przypisań osi.
    /// </summary>
    public string NormalizationMethod { get; set; } = null!;

    public string MemberStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidateClassificationMeasure> CandidateClassificationMeasures { get; set; } = new List<CandidateClassificationMeasure>();

    public virtual ICollection<CandidateClassificationValue> CandidateClassificationValues { get; set; } = new List<CandidateClassificationValue>();

    public virtual ClassificationAxis ClassificationAxis { get; set; } = null!;

    public virtual ICollection<ClassificationMeasureProjectionRule> ClassificationMeasureProjectionRules { get; set; } = new List<ClassificationMeasureProjectionRule>();

    public virtual ICollection<ClassificationModelValue> ClassificationModelValues { get; set; } = new List<ClassificationModelValue>();

    public virtual ClassificationModelVersion ClassificationModelVersion { get; set; } = null!;
}
