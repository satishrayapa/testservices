using System;


namespace TAGov.Services.Core.BaseValueSegment.Domain.Models.V1
{
  public class BaseValueSegmentHistoryDto
  {
    public int BvsId { get; set; }

    public DateTime AsOf { get; set; }

    public int BaseYear { get; set; }

    public decimal BaseValue { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public string BvsTransactionType { get; set; }

    public int LegalPartyRoleId { get; set; }

    public int OwnerGrmEventId { get; set; }

    public int SubComponentId { get; set; }

    public long TransactionId { get; set; }

    public int ValueHeaderGrmEventId { get; set; }
  }
}
