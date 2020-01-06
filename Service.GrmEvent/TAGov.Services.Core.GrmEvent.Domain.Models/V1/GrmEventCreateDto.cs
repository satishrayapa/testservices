using System;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventCreateDto
  {
    public int ParentId { get; set; }

    public GrmEventParentType ParentType { get; set; }

    public int RevenueObjectId { get; set; }

    public DateTime EffectiveDateTime { get; set; }

    public int EventType { get; set; }

    public int GrmEventId { get; set; }
  }
}