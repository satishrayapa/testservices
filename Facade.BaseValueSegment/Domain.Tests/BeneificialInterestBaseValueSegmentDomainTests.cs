using System;
using Moq;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class BeneificialInterestBaseValueSegmentDomainTests
  {
    private readonly IBeneificialInterestBaseValueSegmentDomain _beneificialInterestBaseValueSegmentDomain;
    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IBaseValueSegmentProvider> _baseValueSegmentProvider;
    private readonly Mock<IGrmEventDomain> _grmEventDomain;
    private readonly Mock<ILegalPartyDomain> _legalPartyDomain;
    private readonly Mock<IAssessmentEventRepository> _assessmentEventRepository;

    public BeneificialInterestBaseValueSegmentDomainTests()
    {
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _baseValueSegmentProvider = new Mock<IBaseValueSegmentProvider>();
      _grmEventDomain = new Mock<IGrmEventDomain>();
      _legalPartyDomain = new Mock<ILegalPartyDomain>();
      _assessmentEventRepository = new Mock<IAssessmentEventRepository>();

      _beneificialInterestBaseValueSegmentDomain = new BeneificialInterestBaseValueSegmentDomain(
        _baseValueSegmentRepository.Object,
        _baseValueSegmentProvider.Object, _grmEventDomain.Object,
        _legalPartyDomain.Object, _assessmentEventRepository.Object );
    }

    [Fact]
    public async void ReturnBaseValueSegmentThatIsFoundWithOwner()
    {
      const int assessmentEventIdWithoutACorrespondingBaseValueSegment = 12348;
      const int revenueObjectId = 12;
      DateTime eventDate = new DateTime( 1999, 1, 1 );

      var mock = new MockData
                 {
                   BaseValueSegmentId = 31333,
                   DisplayName = "foo bar",
                   EventDate = eventDate,
                   RevenueObjectId = revenueObjectId,
                   DocumentNumber = "abcd",
                   DocumentType = "efg",
                   OwnerId = 233,
                   PercentageInterestGain = 23,
                   GrmEventType = "grm type 1",
                   GrmEventDate = new DateTime( 1998, 12, 1 ),
                   GrmEventDescription = "grm foo bar",
                   LegalPartyDomain = _legalPartyDomain,
                   GrmEventDomain = _grmEventDomain,

                 };

      _baseValueSegmentProvider.MockData( mock );

      _assessmentEventRepository.Setup( x => x.Get( assessmentEventIdWithoutACorrespondingBaseValueSegment ) )
                                .ReturnsAsync( new AssessmentEventDto { EventDate = eventDate } );

      var result = await _beneificialInterestBaseValueSegmentDomain.Get( assessmentEventIdWithoutACorrespondingBaseValueSegment );

      result.CurrentBaseValueSegment.AssertWith( mock );
    }
  }
}