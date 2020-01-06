using System;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read
{
  public class BvsHistoryDetailDto
  {
    public DateTime EventDate { get; set; }

    public string EventType { get; set; }

    public string DocumentNumber { get; set; }

    public string BeneficialInterest { get; set; }

    public decimal PercentInterestGained { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public DateTime OriginalEventDate { get; set; }

    public string OriginalEventType { get; set; }

    public int BaseYear { get; set; }

    public string Component { get; set; }

    public string SubComponent { get; set; }

    public decimal MarketValue { get; set; }

    public decimal RestrictedValue { get; set; }

    public decimal BaseValue { get; set; }

    public string BvsTransactionType { get; set; }

    public string UserName { get; set; }

    public DateTime DateChanged { get; set; }
  }
}