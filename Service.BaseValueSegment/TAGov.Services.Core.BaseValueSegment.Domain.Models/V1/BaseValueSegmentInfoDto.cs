using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentInfoDto
  {
    public int? Id { get; set; }

    public DateTime AsOf { get; set; }

    public int RevenueObjectId { get; set; }

    public int SequenceNumber { get; set; }
  }
}
