using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BeneficialInterestEventDto
  {
    public int GrmEventId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventType { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string DocNumber { get; set; }

    public string DocType { get; set; }

    public OwnerDetailDto[] Owners { get; set; }
  }
}
