using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentConclusionDto
  {
    public int GrmEventId { get; set; }

    public DateTime ConclusionDate { get; set; }

    public string Description { get; set; }
  }
}
