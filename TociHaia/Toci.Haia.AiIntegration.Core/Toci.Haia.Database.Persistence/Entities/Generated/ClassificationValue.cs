using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Słownik wartości dla osi klasyfikacji humoru.
/// </summary>
public partial class ClassificationValue
{
    public Guid ClassificationValueId { get; set; }

    public Guid ClassificationAxisId { get; set; }

    /// <summary>
    /// Stabilny klucz techniczny wartości (EN).
    /// </summary>
    public string ValueKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>
    /// Krótka wskazówka semantyczna dla pipeline AI.
    /// </summary>
    public string? AiDescription { get; set; }

    /// <summary>
    /// Krótka wskazówka redakcyjna do review klasyfikacji.
    /// </summary>
    public string? EditorialGuidance { get; set; }

    /// <summary>
    /// Opcjonalna hierarchia wartości (parent-child).
    /// </summary>
    public Guid? ParentClassificationValueId { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ClassificationAxis ClassificationAxis { get; set; } = null!;

    public virtual ICollection<ClassificationModelValue> ClassificationModelValues { get; set; } = new List<ClassificationModelValue>();

    public virtual ClassificationValue? ClassificationValueNavigation { get; set; }

    public virtual ICollection<ClassificationValue> InverseClassificationValueNavigation { get; set; } = new List<ClassificationValue>();
}
