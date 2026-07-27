using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class MemeCalibrationVariant
{
    public Guid MemeCalibrationVariantId { get; set; }

    public Guid MemeCalibrationSessionId { get; set; }

    public int VariantNo { get; set; }

    public string? VariantText { get; set; }

    public string? VariantStyle { get; set; }

    public Guid? AiOperationExecutionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AiOperationExecution? AiOperationExecution { get; set; }

    public virtual ICollection<MemeCalibrationFeedback> MemeCalibrationFeedbacks { get; set; } = new List<MemeCalibrationFeedback>();

    public virtual MemeCalibrationSession MemeCalibrationSession { get; set; } = null!;
}
