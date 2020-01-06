using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentEventDto
  {
    public int BvsId { get; set; }

    public DateTime BvsAsOf { get; set; }

    public int RevenueObjectId { get; set; }

    public int SequenceNumber { get; set; }

    public int? GRMEventId { get; set; }
  }
}
