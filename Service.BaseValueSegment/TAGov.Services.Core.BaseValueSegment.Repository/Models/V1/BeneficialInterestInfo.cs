using System;
using System.ComponentModel.DataAnnotations;


namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  public class BeneficialInterestInfo
  {
    [Key]
    public int LegalPartyId { get; set; }

    public int GrmEventId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventType { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string DocNumber { get; set; }

    public DateTime? DocDate { get; set; }

    public string DocType { get; set; }

    public int? OwnerId { get; set; }

    public string OwnerName { get; set; }

    public int LegalPartyRoleId { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public decimal PercentageInterestGain { get; set; }
  }
}
