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
  public class SubComponentBaseValueSegmentDomainTests
  {
    private readonly ISubComponentBaseValueSegmentDomain _subComponentBaseValueSegmentDomain;
    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IRevenueObjectDomain> _revenueObjectDomain;
    private readonly Mock<IBaseValueSegmentProvider> _baseValueSegmentProvider;
    private readonly Mock<IGrmEventDomain> _grmEventDomain;
    private readonly Mock<IAssessmentEventRepository> _assessmentEventRepository;
    private readonly Mock<ILegalPartyDomain> _legalPartyDomainRepository;

    public SubComponentBaseValueSegmentDomainTests()
    {
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _revenueObjectDomain = new Mock<IRevenueObjectDomain>();
      _baseValueSegmentProvider = new Mock<IBaseValueSegmentProvider>();
      _grmEventDomain = new Mock<IGrmEventDomain>();
      _legalPartyDomainRepository = new Mock<ILegalPartyDomain>();
      _assessmentEventRepository = new Mock<IAssessmentEventRepository>();

      _subComponentBaseValueSegmentDomain = new SubComponentBaseValueSegmentDomain(
        _baseValueSegmentRepository.Object,
        _baseValueSegmentProvider.Object, _grmEventDomain.Object,
        _legalPartyDomainRepository.Object,
        _assessmentEventRepository.Object );
    }

    [Fact]
    public void GetSubComponentsWithValidAssessmentEventId()
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
                                                                         BaseValueSegmentTransactionType = new BaseValueSegmentTransactionTypeDto
                                                                                                           {
                                                                                                             Description = "UnitTestBVSTransactionType",
                                                                                                             Name = "UnitTestBVSTransactionType"
                                                                                                           },
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

      var conclusionEventsList = new List<BaseValueSegmentConclusionDto>
                                 {
                                   new BaseValueSegmentConclusionDto
                                   {
                                     ConclusionDate = eventDate,
                                     Description = "UnitTestConclusionEvent",
                                     GrmEventId = grmEventId
                                   }
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

      _legalPartyDomainRepository.Setup( x => x.GetLegalPartyRoleDocuments( It.IsAny<BaseValueSegmentDto>() ) )
                                 .ReturnsAsync( legalPartyRoleDocumentList );

      var valueHeaderGrmEvents = new List<GrmEventInformationDto>
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

      _grmEventDomain.Setup( x => x.GetValueHeaderGrmEvents( It.IsAny<BaseValueSegmentDto>() ) )
                     .ReturnsAsync( valueHeaderGrmEvents.AsEnumerable );

      _baseValueSegmentRepository.Setup( x => x.GetConclusionsData( It.IsAny<int>(), It.IsAny<DateTime>() ) )
                                 .ReturnsAsync( conclusionEventsList.AsEnumerable );

      _revenueObjectDomain.Setup( x => x.GetMarketAndRestrictedValues( It.IsAny<DateTime>(), It.IsAny<int>() ) )
                          .ReturnsAsync( marketRestrictedValues.AsEnumerable );

      _baseValueSegmentRepository.Setup( x => x.GetSubComponentDetails( It.IsAny<int>(), It.IsAny<DateTime>() ) )
                                 .ReturnsAsync( subComponentDetailsList.AsEnumerable );

      _baseValueSegmentRepository.Setup( x => x.GetFactorBaseYearValueDetail( It.IsAny<DateTime>(),
                                                                              It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>() ) ).ReturnsAsync( factorBaseYearValueDetail );

      var subComponent = _subComponentBaseValueSegmentDomain.Get( assessmentEventId ).Result;

      subComponent.ShouldNotBeNull();
      subComponent.CurrentBaseValueSegment.ShouldNotBeNull();
      subComponent.PreviousBaseValueSegment.ShouldBeNull();
      var bvsComponent = subComponent.CurrentBaseValueSegment;
      bvsComponent.ShouldNotBeNull();
      bvsComponent.BaseValueSegmentTransactionTypeDescription.ShouldBe( "UnitTestBVSTransactionType" );
      bvsComponent.EventDate.ShouldBe( eventDate );
      bvsComponent.EffectiveDate.ShouldBe( effectiveDate );
      bvsComponent.EventName.ShouldBe( "UnitTestGrmEvent" );
      bvsComponent.EventType.ShouldBe( "UnitTestConclusionEvent" );
      bvsComponent.Source.ShouldBe( baseValueSegmentCurrent );
      bvsComponent.Components.Length.ShouldBeGreaterThan( 0 );
      bvsComponent.ValueHeaders.Length.ShouldBeGreaterThan( 0 );
      var component = bvsComponent.Components[ 0 ];
      component.BaseValue.ShouldBe( 80000 );
      component.BaseYear.ShouldBe( 2016 );
      component.EventId.ShouldBe( grmEventId );
      component.EventName.ShouldBe( "UnitTestGrmEvent" );
      component.EventType.ShouldBe( "UnitTestConclusionEvent" );
      component.Fbyv.ShouldBe( 102000 );
      component.FbyvAsOfYear.ShouldBe( 2016 );
      component.ValueHeaderId.ShouldBe( bvsValueHeaderId );
      var componentDetail = component.ComponentDetails[ 0 ];
      componentDetail.BaseValue.ShouldBe( 80000 );
      componentDetail.Component.ShouldBe( "Unit Test Component" );
      componentDetail.ComponentId.ShouldBe( 0 );
      componentDetail.Fbyv.ShouldBe( 102000 );
      componentDetail.SubComponent.ShouldBe( "Unit Test Subcomponent" );
      componentDetail.SubComponentId.ShouldBe( subComponentId );
      componentDetail.ValueId.ShouldBe( bvsValueId );
      var valueHeader = bvsComponent.ValueHeaders[ 0 ];
      valueHeader.BaseYear.ShouldBe( 2016 );
      valueHeader.DisplayName.ShouldBe( "UnitTestConclusionEvent 2016" );
      valueHeader.EffectiveDate.ShouldBe( effectiveDate );
      valueHeader.EventDate.ShouldBe( eventDate );
      valueHeader.EventType.ShouldBe( "UnitTestConclusionEvent" );
      valueHeader.GrmEventId.ShouldBe( grmEventId );
      valueHeader.HeaderValueId.ShouldBe( bvsValueHeaderId );

    }
  }
}