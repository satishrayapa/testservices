using System;


namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{

  public class GrmEventCreateInformation
  {
    public int RevenueObjectId { get; set; }

    public DateTime EffectiveDateTime { get; set; }

    public int EventType { get; set; }

  }
}
