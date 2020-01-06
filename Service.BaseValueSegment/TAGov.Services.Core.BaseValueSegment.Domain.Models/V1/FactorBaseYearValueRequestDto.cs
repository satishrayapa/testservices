using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class FactorBaseYearValueRequestDto
  {
    public DateTime EventDate { get; set; }

    public int AssessmentYear { get; set; }

    public decimal BaseValue { get; set; }

    public int AssessmentEventType { get; set; }

    public decimal Fbyv { get; set; }

    public int FbyvYear { get; set; }
  }
}
