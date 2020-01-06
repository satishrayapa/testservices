using System;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  public class GrmEventCreate
  {
    public int ParentId { get; set; }

    public GrmEventParentType ParentType { get; set; }

    public int RevenueObjectId { get; set; }

    public DateTime EffectiveDateTime { get; set; }

    public int EventType { get; set; }

    public int GrmEventId { get; set; }
  }
}