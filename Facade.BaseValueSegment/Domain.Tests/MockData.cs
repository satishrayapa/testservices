using System;
using Moq;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace Domain.Tests
{
  public class MockData
  {
    public int RevenueObjectId { get; set; }
    public DateTime EventDate { get; set; }
    public string DisplayName { get; set; }
    public string DocumentNumber { get; set; }
    public string DocumentType { get; set; }
    public int BaseValueSegmentId { get; set; }
    public decimal BeneficialInterestPercentage { get; set; }
    public int OwnerId { get; set; }
    public int PercentageInterestGain { get; set; }

    public Mock<ILegalPartyDomain> LegalPartyDomain { get; set; }
    public Mock<IGrmEventDomain> GrmEventDomain { get; set; }
    public string GrmEventDescription { get; set; }
    public string GrmEventType { get; set; }
    public DateTime GrmEventDate { get; set; }
  }
}