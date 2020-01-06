using System;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read
{
  public abstract class BvsDto
  {
    public string EventName { get; set; }

    public DateTime? EventDate { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public string EventType { get; set; }

    public string BaseValueSegmentTransactionTypeDescription { get; set; }

    public BaseValueSegmentDto Source { get; set; }
  }
}