using System;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class OwnerDto
  {
    public int OwnerId { get; set; }

    public string BeneficialInterest { get; set; }

    public string EventName { get; set; }

    public DateTime? EventDate { get; set; }

    public string EventType { get; set; }

    public string DocNumber { get; set; }

    public string DocType { get; set; }

    public decimal? PercentageInterestGain { get; set; }

    public decimal BiPercentage { get; set; }

    public int LegalPartyRoleId { get; set; }

    public OwnerValueDto[] OwnerValues { get; internal set; }

    public bool? IsOverride { get; set; }
  }
}