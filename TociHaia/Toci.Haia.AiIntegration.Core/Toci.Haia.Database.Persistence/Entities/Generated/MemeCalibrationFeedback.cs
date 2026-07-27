using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class MemeCalibrationFeedback
{
    public Guid MemeCalibrationFeedbackId { get; set; }

    public Guid MemeCalibrationVariantId { get; set; }

    public Guid AccountId { get; set; }

    public short? RatingValue { get; set; }

    public bool? SelectedFinal { get; set; }

    public decimal? ImpactWeight { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual MemeCalibrationVariant MemeCalibrationVariant { get; set; } = null!;
}
