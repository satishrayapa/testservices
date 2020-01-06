using System;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class OwnerDetailDto
  {
    public int? OwnerId { get; set; }

    public string OwnerName { get; set; }

    public int LegalPartyRoleId { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public int GrmEventId { get; set; }

    public decimal PercentageInterestGain { get; set; }
  }
}
