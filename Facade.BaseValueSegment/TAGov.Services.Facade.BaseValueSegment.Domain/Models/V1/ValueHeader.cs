using System;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class HeaderValue
  {
    public int HeaderValueId { get; set; }

    public int GrmEventId { get; set; }

    public string DisplayName { get; set; }

    public int BaseYear { get; set; }

    public string EventType { get; set; }

    public DateTime EventDate { get; set; }

    public DateTime EffectiveDate { get; set; }
  }
}