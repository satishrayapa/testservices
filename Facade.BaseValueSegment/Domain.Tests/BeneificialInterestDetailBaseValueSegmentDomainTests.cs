using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class BeneificialInterestDetailBaseValueSegmentDomainTests
  {
    private readonly IBeneificialInterestDetailBaseValueSegmentDomain _beneificialInterestDetailBaseValueSegmentDomain;
    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IBaseValueSegmentProvider> _baseValueSegmentProvider;
    private readonly Mock<IGrmEventDomain> _grmEventDomain;
    private readonly Mock<ILegalPartyDomain> _legalPartyDomain;
    private readonly Mock<IAssessmentEventRepository> _assessmentEventRepository;

    public BeneificialInterestDetailBaseValueSegmentDomainTests()
    {
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _baseValueSegmentProvider = new Mock<IBaseValueSegmentProvider>();
      _grmEventDomain = new Mock<IGrmEventDomain>();
      _legalPartyDomain = new Mock<ILegalPartyDomain>();
      _assessmentEventRepository = new Mock<IAssessmentEventRepository>();

      _beneificialInterestDetailBaseValueSegmentDomain = new BeneificialInterestDetailBaseValueSegmentDomain(
        _baseValueSegmentRepository.Object,
        _baseValueSegmentProvider.Object, _grmEventDomain.Object, _legalPartyDomain.Object, _assessmentEventRepository.Object );
    }

    [Fact]
    public void GetBeneficialInterestDetailWithValidAssessmentEventId()
    {
      int assessmentEventId = 100;
      int revenueObjectId = 200;
      int bvsId = 300;
      int bvsTransId = 310;
      int bvsOwnerId = 320;
      int bvsValueHeaderId = 330;
      int bvsOwnerValueId = 340;
      int bvsValueId = 350;
      int transId = 400;
      int legalPartyRoleId = 400;
      int grmEventId = 500;
      int subComponentId = 600;
      int sequenceNumber = 1;
      int dynCalcStepTrackingId = -100;
      DateTime eventDate = new DateTime( 2016, 1, 1 );
      DateTime effectiveDate = new DateTime( 2016, 2, 1 );

      var baseValueSegmentCurrent = new BaseValueSegmentDto
                                    {
                                      AsOf = eventDate,
                                      AssessmentEventTransactionId = 10,
                                      BaseValueSegmentAssessmentRevisions = new List<AssessmentRevisionBaseValueSegmentDto>(),
                                      BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                                     {
                                                                       new BaseValueSegmentTransactionDto
                                                                       {
                                                                         Id = bvsTransId,
                                                                         BaseValueSegmentId = bvsId,
                                                                         TransactionId = transId,
                                                                         EffectiveStatus = "A",
                                                                         BaseValueSegmentTransactionTypeId = 2,
                                                                         DynCalcStepTrackingId = dynCalcStepTrackingId,
                                                                         BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
                                                                                                  {
                                                                                                    new BaseValueSegmentOwnerDto
                                                                                                    {
                                                                                                      Id = bvsOwnerId,
                                                                                                      BaseValueSegmentTransactionId = bvsTransId,
                                                                                                      BeneficialInterestPercent = 50,
                                                                                                      DynCalcStepTrackingId = dynCalcStepTrackingId,
                                                                                                      GRMEventId = grmEventId,
                                                                                                      LegalPartyRoleId = legalPartyRoleId,
                                                                                                      BaseValueSegmentOwnerValueValues = new List<BaseValueSegmentOwnerValueDto>
                                                                                                                                         {
                                                                                                                                           new BaseValueSegmentOwnerValueDto
                                                                                                                                           {
                                                                                                                                             Id = bvsOwnerValueId,
                                                                                                                                             BaseValueSegmentOwnerId = bvsOwnerId,
                                                                                                                                             BaseValueSegmentValueHeaderId = bvsValueHeaderId,
                                                                                                                                             BaseValue = 10000,
                                                                                                                                             DynCalcStepTrackingId = dynCalcStepTrackingId
                                                                                                                                           }
                                                                                                                                         }
                                                                                                    },
                                                                                                  },
                                                                         BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeaderDto>
                                                                                                        {
                                                                                                          new BaseValueSegmentValueHeaderDto
                                                                                                          {
                                                                                                            Id = bvsValueHeaderId,
                                                                                                            BaseValueSegmentTransactionId = bvsTransId,
                                                                                                            GRMEventId = grmEventId,
                                                                                                            BaseYear = 2016,
                                                                                                            BaseValueSegmentValues = new List<BaseValueSegmentValueDto>
                                                                                                                                     {
                                                                                                                                       new BaseValueSegmentValueDto
                                                                                                                                       {
                                                                                                                                         Id = bvsValueId,
                                                                                                                                         BaseValueSegmentValueHeaderId = bvsValueHeaderId,
                                                                                                                                         DynCalcStepTrackingId = dynCalcStepTrackingId,
                                                                                                                                         FullValueAmount = 100000,
                                                                                                                                         PercentComplete = 50,
                                                                                                                                         SubComponent = subComponentId,
                                                                                                                                         ValueAmount = 80000
                                                                                                                                       }
                                                                                                                                     }

                                                                                                          }
                                                                                                        }
                                                                       }
                                                                     },
                                      DynCalcInstanceId = -1,
                                      Id = bvsId,
                                      RevenueObjectId = revenueObjectId,
                                      SequenceNumber = sequenceNumber,
                                      TransactionId = -1000
                                    };

      var baseValueSegmentInfoList = new List<BaseValueSegmentInfoDto>
                                     {
                                       new BaseValueSegmentInfoDto
                                       {
                                         AsOf = eventDate,
                                         Id = bvsId,
                                         RevenueObjectId = revenueObjectId,
                                         SequenceNumber = sequenceNumber
                                       }
                                     };

      var assessmentEvent = new AssessmentEventDto
                            {
                              Id = assessmentEventId,
                              EventDate = eventDate
                            };

      var legalPartyRoleDocumentList = new List<LegalPartyDocumentDto>
                                       {
                                         new LegalPartyDocumentDto
                                         {
                                           LegalPartyRoleId = legalPartyRoleId,
                                           DocDate = effectiveDate,
                                           DocNumber = "UnitTestDocument",
                                           DocType = "Deed",
                                           GrmEventId = grmEventId,
                                           LegalPartyDisplayName = "Unit Test Legal Party",
                                           PctGain = 50,
                                           RightTransferId = 0
                                         }
                                       };

      var ownerGrmEvents = new List<GrmEventInformationDto>
                           {
                             new GrmEventInformationDto
                             {
                               GrmEventId = grmEventId,
                               Description = "UnitTestGrmEvent",
                               EventDate = eventDate,
                               EffectiveDate = effectiveDate,
                               RevenueObjectId = revenueObjectId,
                               EventType = "Transfer"
                             }
                           };

      var marketRestrictedValues = new List<MarketAndRestrictedValueDto>
                                   {
                                     new MarketAndRestrictedValueDto
                                     {
                                       SubComponent = subComponentId,
                                       MarketValue = 100000,
                                       RestrictedValue = 50000
                                     }
                                   };

      var subComponentDetailsList = new List<SubComponentDetailDto>
                                    {
                                      new SubComponentDetailDto
                                      {
                                        SubComponentId = subComponentId,
                                        Component = "Unit Test Component",
                                        SubComponent = "Unit Test Subcomponent"
                                      }
                                    };

      var factorBaseYearValueDetail = new FactorBaseYearValueDetailDto
                                      {
                                        AssessmentYear = 2016,
                                        Fbyv = 102000
                                      };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegmentInfoList );

      Tuple<BaseValueSegmentDto, BaseValueSegmentDto> resultSet = new Tuple<BaseValueSegmentDto, BaseValueSegmentDto>( baseValueSegmentCurrent, null );

      _baseValueSegmentProvider.Setup( x => x.GetCurrentAndPrevious( assessmentEventId ) ).ReturnsAsync( resultSet );

      _assessmentEventRepository.Setup( x => x.Get( assessmentEventId ) ).ReturnsAsync( assessmentEvent );

      _legalPartyDomain.Setup( x => x.GetLegalPartyRoleDocuments( It.IsAny<BaseValueSegmentDto>() ) )
                       .ReturnsAsync( legalPartyRoleDocumentList.AsEnumerable );

      _grmEventDomain.Setup( x => x.GetOwnerGrmEvents( It.IsAny<BaseValueSegmentDto>() ) )
                     .ReturnsAsync( ownerGrmEvents.AsEnumerable );

      _baseValueSegmentRepository.Setup( x => x.GetSubComponentDetails( It.IsAny<int>(), It.IsAny<DateTime>() ) )
                                 .ReturnsAsync( subComponentDetailsList.AsEnumerable );

      _baseValueSegmentRepository.Setup( x => x.GetFactorBaseYearValueDetail( It.IsAny<DateTime>(),
                                                                              It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>() ) ).ReturnsAsync( factorBaseYearValueDetail );

      var beneficalInterestDetail = _beneificialInterestDetailBaseValueSegmentDomain.Get( assessmentEventId ).Result;

      beneficalInterestDetail.ShouldNotBeNull();
      beneficalInterestDetail.CurrentBaseValueSegment.ShouldNotBeNull();
      var bvsDetail = beneficalInterestDetail.CurrentBaseValueSegment;
      bvsDetail.ShouldNotBeNull();
      bvsDetail.BaseValueSegmentTransactionTypeDescription.ShouldBeNull();
      bvsDetail.EventDate.ShouldBe( eventDate );
      bvsDetail.EffectiveDate.ShouldBe( effectiveDate );
      bvsDetail.EventName.ShouldBe( "UnitTestGrmEvent" );
      bvsDetail.EventType.ShouldBe( "Transfer" );
      var details = bvsDetail.Details;
      details.Length.ShouldBeGreaterThan( 0 );
      var detail = details[ 0 ];
      detail.BaseValueSegmentEventDate.ShouldBe( effectiveDate );
      detail.BaseValueSegmentEventName.ShouldBe( "UnitTestGrmEvent" );
      detail.BaseValueSegmentEventType.ShouldBe( "Transfer" );
      detail.BaseYear.ShouldBe( 2016 );
      detail.BeneficialInterest.ShouldBe( "Unit Test Legal Party" );
      detail.BiPercentage.ShouldBe( 50 );
      detail.ComponentName.ShouldBe( "Unit Test Component" );
      detail.DocNumber.ShouldBe( "UnitTestDocument" );
      detail.DocType.ShouldBeNull();
      detail.OwnershipEventDate.ShouldBe( effectiveDate );
      detail.OwnershipEventName.ShouldBe( "UnitTestGrmEvent" );
      detail.OwnershipEventType.ShouldBe( "Transfer" );
      detail.PercentageInterestGain.ShouldBe( 50 );
      detail.SubComponentName.ShouldBe( "Unit Test Subcomponent" );
    }
  }
}