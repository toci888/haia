using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Słownik osi klasyfikacji humoru dla materiału (meme/joke).
/// </summary>
public partial class ClassificationAxis
{
    public Guid ClassificationAxisId { get; set; }

    /// <summary>
    /// Stabilny klucz techniczny osi (EN), używany w integracjach i seedach.
    /// </summary>
    public string AxisKey { get; set; } = null!;

    /// <summary>
    /// Nazwa prezentacyjna osi (PL).
    /// </summary>
    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>
    /// Typ sygnału osi: categorical/ordinal/scalar.
    /// </summary>
    public string AxisType { get; set; } = null!;

    /// <summary>
    /// Rola osi w modelu: affinity/context/routing/gating/analytics.
    /// </summary>
    public string AxisRole { get; set; } = null!;

    /// <summary>
    /// Dopuszczalna liczba przypisań: single lub multi.
    /// </summary>
    public string Cardinality { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<ClassificationModelAxis> ClassificationModelAxes { get; set; } = new List<ClassificationModelAxis>();

    public virtual ICollection<ClassificationValue> ClassificationValues { get; set; } = new List<ClassificationValue>();
}
