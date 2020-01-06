using System;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventInformationDto
  {
    public int GrmEventId { get; set; }

    public string Description { get; set; }

    public DateTime EffectiveDate { get; set; }

    public int RevenueObjectId { get; set; }

    public string EventType { get; set; }

    public DateTime EventDate { get; set; }
  }
}