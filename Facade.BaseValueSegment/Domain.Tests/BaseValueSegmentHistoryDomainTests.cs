using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;
using SitusAddressDto = TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.SitusAddressDto;

namespace Domain.Tests
{
  public class BaseValueSegmentHistoryDomainTests
  {
    private readonly Mock<IRevenueObjectRepository> _revenueObjectRepository;
    private readonly BaseValueSegmentHistoryDomain _baseValueSegmentHistoryDomain;
    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IGrmEventRepository> _grmEventRepository;
    private readonly Mock<ILegalPartyRepository> _legalPartyRepository;
    private readonly Mock<ILogger> _logger;

    public BaseValueSegmentHistoryDomainTests()
    {
      _revenueObjectRepository = new Mock<IRevenueObjectRepository>();
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _grmEventRepository = new Mock<IGrmEventRepository>();
      _legalPartyRepository = new Mock<ILegalPartyRepository>();
      _logger = new Mock<ILogger>();

      _baseValueSegmentHistoryDomain = new BaseValueSegmentHistoryDomain(
        _baseValueSegmentRepository.Object, _grmEventRepository.Object, _legalPartyRepository.Object,
        _revenueObjectRepository.Object, _logger.Object );
    }

    [Fact]
    public void GetRevObjWithSitusShouldReturnRevObjWithSitus()
    {
      var ain = "some ain";
      var pin = "some pin";
      var id = 123;
      var situs = new SitusAddressDto()
                  {
                    FreeFormAddress = "some free form address",
                    City = "some city",
                    StateCode = "some state code",
                    PostalCode = "some postal code"
                  };
      var status = "a";
      var expectedStatus = "Active";
      var tag = "some tag";

      _revenueObjectRepository
        .Setup( x =>
                  x.GetWithSitusByPin(
                    It.IsAny<string>() ) )
        .Returns( Task.FromResult( new RevenueObjectDto
                                   {
                                     Ain = ain,
                                     Pin = pin,
                                     Id = id,
                                     SitusAddress = new TAGov.Services.Core.RevenueObject.Domain.Models.V1.SitusAddressDto()
                                                    {
                                                      FreeFormAddress = situs.FreeFormAddress,
                                                      City = situs.City,
                                                      StateCode = situs.StateCode,
                                                      PostalCode = situs.PostalCode
                                                    },
                                     EffectiveStatus = status,
                                   } ) );
      _revenueObjectRepository
        .Setup( x => x.GetTag(
                  It.IsAny<int>() ) )
        .Returns( Task.FromResult( new TAGDto { Description = tag } ) );

      var result = _baseValueSegmentHistoryDomain.GetBaseValueSegmentPinHistoryAsync( "some pin" ).Result;
      result.Pin.ShouldBe( pin );
      result.Ain.ShouldBe( ain );
      result.SitusAddress.FreeFormAddress.ShouldBe( situs.FreeFormAddress );
      result.SitusAddress.City.ShouldBe( situs.City );
      result.SitusAddress.StateCode.ShouldBe( situs.StateCode );
      result.SitusAddress.PostalCode.ShouldBe( situs.PostalCode );
      result.Status.ShouldBe( expectedStatus );
      result.Tag.ShouldBe( tag );
      result.RevenueAccount.ShouldBe( id );

    }

    [Fact]
    public void GetBvsHistoryWithValidRevObjIdAndDates()
    {
      var baseValueSegmentHistoryList = new List<BaseValueSegmentHistoryDto>
                                        {
                                          new BaseValueSegmentHistoryDto
                                          {
                                            BvsId = 100,
                                            AsOf = new DateTime( 2016, 1, 1 ),
                                            BaseYear = 2016,
                                            BaseValue = 100000,
                                            BeneficialInterestPercentage = 50,
                                            BvsTransactionType = "User",
                                            LegalPartyRoleId = 200,
                                            OwnerGrmEventId = 300,
                                            SubComponentId = 400,
                                            TransactionId = 500,
                                            ValueHeaderGrmEventId = 600
                                          }
                                        };

      var revenueObject = new RevenueObjectDto
                          {
                            Id = 700,
                            Pin = "UnitTestPin"
                          };

      var grmEventInformationList = new List<GrmEventInformationDto>
                                    {
                                      new GrmEventInformationDto
                                      {
                                        Description = "UnitTestGrmEvent1",
                                        EffectiveDate = new DateTime( 2016, 1, 1 ),
                                        EventDate = new DateTime( 2016, 2, 1 ),
                                        EventType = "Unit Test Event Type1",
                                        GrmEventId = 300,
                                        RevenueObjectId = 700
                                      },
                                      new GrmEventInformationDto
                                      {
                                        Description = "UnitTestGrmEvent2",
                                        EffectiveDate = new DateTime( 2016, 1, 1 ),
                                        EventDate = new DateTime( 2016, 2, 1 ),
                                        EventType = "Unit Test Event Type2",
                                        GrmEventId = 600,
                                        RevenueObjectId = 700
                                      }
                                    };

      var baseValueSegmentEventList = new List<BaseValueSegmentEventDto>
                                      {
                                        new BaseValueSegmentEventDto
                                        {
                                          BvsAsOf = new DateTime( 2016, 1, 1 ),
                                          BvsId = 800,
                                          GRMEventId = 600,
                                          RevenueObjectId = 700,
                                          SequenceNumber = 1
                                        }
                                      };

      var subComponentDetailDtoList = new List<SubComponentDetailDto>
                                      {
                                        new SubComponentDetailDto
                                        {
                                          Component = "UnitTestComponent",
                                          ComponentTypeId = 900,
                                          SubComponent = "UnitTestSubComponent",
                                          SubComponentId = 400
                                        }
                                      };

      var marketAndRestrictedValueList = new List<MarketAndRestrictedValueDto>
                                         {
                                           new MarketAndRestrictedValueDto
                                           {
                                             MarketValue = 100000,
                                             RestrictedValue = 50000,
                                             SubComponent = 400
                                           }
                                         };

      var legalPartyDocumentList = new List<LegalPartyDocumentDto>
                                   {
                                     new LegalPartyDocumentDto
                                     {
                                       DocDate = new DateTime( 2016, 1, 1 ),
                                       DocNumber = "UnitTestDocumentNumber",
                                       DocType = "Deed",
                                       GrmEventId = 600,
                                       LegalPartyDisplayName = "UnitTestLegalParty",
                                       LegalPartyRoleId = 200,
                                       PctGain = 25,
                                       RightTransferId = 0
                                     }
                                   };
      revenueObject.MarketAndRestrictedValues = new List<MarketAndRestrictedValueDto>();
      revenueObject.MarketAndRestrictedValues = marketAndRestrictedValueList;

      _baseValueSegmentRepository.Setup( x => x.GetBaseValueSegmentHistory( It.IsAny<int>(), It.IsAny<DateTime>(),
                                                                            It.IsAny<DateTime>() ) ).Returns( Task.FromResult( baseValueSegmentHistoryList.AsEnumerable() ) );

      _baseValueSegmentRepository.Setup( x => x.GetEventsAsync( It.IsAny<int>() ) ).Returns( Task.FromResult( baseValueSegmentEventList.AsEnumerable() ) );

      _baseValueSegmentRepository.Setup( x => x.GetSubComponentDetails( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( Task.FromResult( subComponentDetailDtoList.AsEnumerable() ) );

      _revenueObjectRepository.Setup( x => x.GetByPin( It.IsAny<string>() ) ).Returns( Task.FromResult( revenueObject ) );

      _revenueObjectRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) )
                              .Returns( Task.FromResult( revenueObject ) );

      _grmEventRepository.Setup( x => x.SearchAsync( It.IsAny<GrmEventSearchDto>() ) ).Returns( Task.FromResult( grmEventInformationList.AsEnumerable() ) );

      _legalPartyRepository.Setup( x => x.SearchAsync( It.IsAny<LegalPartySearchDto>() ) )
                           .ReturnsAsync( legalPartyDocumentList.AsEnumerable );

      var bvsHistoryResults = _baseValueSegmentHistoryDomain.GetBaseValueSegmentHistoryAsync( "UnitTestPin",
                                                                                              new DateTime( 2012, 1, 1 ), new DateTime( 2017, 1, 1 ) ).Result.ToList();

      bvsHistoryResults.Count.ShouldBe( 1 );
      var bvsHistoryDetail = bvsHistoryResults[ 0 ];
      bvsHistoryDetail.BaseValue.ShouldBe( 100000 );
      bvsHistoryDetail.BaseYear.ShouldBe( 2016 );
      bvsHistoryDetail.BeneficialInterest.ShouldBe( "UnitTestLegalParty" );
      bvsHistoryDetail.BeneficialInterestPercentage.ShouldBe( 50 );
      bvsHistoryDetail.BvsTransactionType.ShouldBe( "User" );
      bvsHistoryDetail.Component.ShouldBe( "UnitTestComponent" );
      bvsHistoryDetail.DocumentNumber.ShouldBe( "UnitTestDocumentNumber" );
      bvsHistoryDetail.EventDate.ShouldBe( new DateTime( 2016, 1, 1 ) );
      bvsHistoryDetail.EventType.ShouldBe( "UnitTestGrmEvent2" );
      bvsHistoryDetail.MarketValue.ShouldBe( 100000 );
      bvsHistoryDetail.OriginalEventDate.ShouldBe( new DateTime( 2016, 1, 1 ) );
      bvsHistoryDetail.OriginalEventType.ShouldBe( "UnitTestGrmEvent2" );
      bvsHistoryDetail.PercentInterestGained.ShouldBe( 25 );
      bvsHistoryDetail.RestrictedValue.ShouldBe( 50000 );
      bvsHistoryDetail.SubComponent.ShouldBe( "UnitTestSubComponent" );
      bvsHistoryDetail.UserName.ShouldBeNull();
    }
  }
}