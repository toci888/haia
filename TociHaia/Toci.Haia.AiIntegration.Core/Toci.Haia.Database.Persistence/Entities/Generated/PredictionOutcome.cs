using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class PredictionOutcome
{
    public Guid PredictionOutcomeId { get; set; }

    public Guid SelectionDecisionId { get; set; }

    public decimal? PredictedEnjoyment { get; set; }

    public decimal? PredictedInformationGain { get; set; }

    public decimal? ActualEnjoyment { get; set; }

    public decimal? ActualInformationGain { get; set; }

    public bool? CompletedFlow { get; set; }

    public bool? MismatchFlag { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual SelectionDecision SelectionDecision { get; set; } = null!;
}
