using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities.Generated;

/// <summary>
/// Słownik kategorii wrażliwości/safety dla materiału.
/// </summary>
public partial class SensitivityCategory
{
    public Guid SensitivityCategoryId { get; set; }

    /// <summary>
    /// Stabilny klucz kategorii safety (EN).
    /// </summary>
    public string CategoryKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CandidateClassificationSensitivity> CandidateClassificationSensitivities { get; set; } = new List<CandidateClassificationSensitivity>();
}
