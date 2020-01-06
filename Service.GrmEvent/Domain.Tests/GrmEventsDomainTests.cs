using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Implementation;
using TAGov.Services.Core.GrmEvent.Repository.Interfaces;
using TAGov.Services.Core.GrmEvent.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.GrmEvent.Domain.Tests
{
  public class GrmEventDomainTests
  {
    private readonly Mock<IGrmEventRepository> _mockGrmEventRepository;
    private readonly GrmEventDomain _grmEventDomain;

    public GrmEventDomainTests()
    {
      _mockGrmEventRepository = new Mock<IGrmEventRepository>();
      _grmEventDomain = new GrmEventDomain( _mockGrmEventRepository.Object );
    }

    /******************** get grm event ***********************/

    [Fact]
    public void GetGrmEvent_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      var grmEvent = new Repository.Models.V1.GrmEvent() { Id = 999 };
      var moqRepository = new Mock<IGrmEventRepository>();
      moqRepository.Setup( x => x.Get( 999 ) ).Returns( grmEvent );

      var grmEventDomain = new GrmEventDomain( moqRepository.Object );
      var returnGrmEvent = grmEventDomain.Get( 999 );
      returnGrmEvent.Id.ShouldBe( grmEvent.Id );
    }

    [Fact]
    public void GetGrmEvent_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
    {
      var moqRepository = new Mock<IGrmEventRepository>();

      var grmEventDomain = new GrmEventDomain( moqRepository.Object );
      Should.Throw<RecordNotFoundException>( () => grmEventDomain.Get( 22 ) );

      moqRepository.Verify( x => x.Get( 22 ) );
    }

    [Fact]
    public void GetGrmEvent_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
    {
      var grmEventDomain = new GrmEventDomain( null );
      Should.Throw<BadRequestException>( () => grmEventDomain.Get( -1 ) );
    }

    /******************************** get grm events ********************************/

    [Fact]
    public void GetGrmEvents_OneIdInListIs0_GetBadRequestException()
    {
      var grmEventDomain = new GrmEventDomain( null );
      Should.Throw<BadRequestException>( () => grmEventDomain.GetGrmEventInfo( new[] { 12, 0 } ) );
    }

    [Fact]
    public void GetGrmEvents_NoListReturned_GetRecordNotFoundException()
    {
      var moqRepository = new Mock<IGrmEventRepository>();
      moqRepository.Setup( x => x.GetGrmEventInfo( new[] { 12, 13 } ) ).Returns( () => new List<Repository.Models.V1.GrmEventInformation>() );

      var grmEventDomain = new GrmEventDomain( moqRepository.Object );

      Should.Throw<RecordNotFoundException>( () => grmEventDomain.GetGrmEventInfo( new[] { 12, 13 } ) );
    }

    [Fact]
    public void GetGrmEvents_EntitiesReturned_GetDtos()
    {
      var moqRepository = new Mock<IGrmEventRepository>();
      moqRepository.Setup( x => x.GetGrmEventInfo( new[] { 12, 13 } ) ).Returns( () => new List<Repository.Models.V1.GrmEventInformation>
                                                                                       {
                                                                                         new Repository.Models.V1.GrmEventInformation
                                                                                         {
                                                                                           GrmEventId = 12,
                                                                                           Description = "First Unit Test GRM Event",
                                                                                           EffectiveDate = new DateTime( 2015, 2, 1 ),
                                                                                           RevenueObjectId = 100,
                                                                                           EventType = "First Unit Test Event Type",
                                                                                           EventDate = new DateTime( 2015, 1, 1 )
                                                                                         },
                                                                                         new Repository.Models.V1.GrmEventInformation
                                                                                         {
                                                                                           GrmEventId = 13,
                                                                                           Description = "Second Unit Test GRM Event",
                                                                                           EffectiveDate = new DateTime( 2016, 2, 1 ),
                                                                                           RevenueObjectId = 200,
                                                                                           EventType = "Second Unit Test Event Type",
                                                                                           EventDate = new DateTime( 2016, 1, 1 )
                                                                                         }
                                                                                       } );

      var grmEventDomain = new GrmEventDomain( moqRepository.Object );

      var list = grmEventDomain.GetGrmEventInfo( new[] { 12, 13 } ).ToList();

      list.Count.ShouldBe( 2 );
      list[ 0 ].GrmEventId.ShouldBe( 12 );
      list[ 1 ].GrmEventId.ShouldBe( 13 );

      var grmEvent1 = list[ 0 ];
      grmEvent1.Description.ShouldBe( "First Unit Test GRM Event" );
      grmEvent1.EffectiveDate.ShouldBe( new DateTime( 2015, 2, 1 ) );
      grmEvent1.RevenueObjectId.ShouldBe( 100 );
      grmEvent1.EventType.ShouldBe( "First Unit Test Event Type" );
      grmEvent1.EventDate.ShouldBe( new DateTime( 2015, 1, 1 ) );
    }

    [Fact]
    public void CreateGrmEventWithValidCreateInput()
    {
      var grmEventListCreate = CreateMockDto();
      var moqRepository = new Mock<IGrmEventRepository>();
      moqRepository.Setup( x => x.CreateGrmEvents( It.IsAny<IEnumerable<GrmEventComponentCreate>>() ) ).Returns( Mapping.Mappers.ToEntity( grmEventListCreate.GrmEventList ) );
      var grmEventDomain = new GrmEventDomain( moqRepository.Object );

      var grmEventListResult = grmEventDomain.CreateGrmEvents( grmEventListCreate );
      grmEventListResult.ShouldNotBeNull();
      grmEventListResult.GrmEventList.Count.ShouldBe( 1 );
    }

    [Fact]
    public void CreateGrmEventWithInvalidCreateInput()
    {
      var grmEventListCreate = CreateMockDto();

      grmEventListCreate.GrmEventList[ 0 ].RevenueObjectId = -1;

      Should.Throw<BadRequestException>( () => _grmEventDomain.CreateGrmEvents( grmEventListCreate ) );
      ;
    }

    [Fact]
    public void GetSubComponentValuesWithValidPin()
    {
      var subComponentValues = new List<SubComponentValue>
                               {
                                 new SubComponentValue
                                 {
                                   SubComponentId = 100,
                                   MarketValue = 100000,
                                   DeltaValue = 105000,
                                   Description = "UnitTestSubComponentValue"
                                 }
                               };

      _mockGrmEventRepository.Setup( x => x.GetSubComponentValues( It.IsAny<string>(), It.IsAny<DateTime>() ) ).Returns( subComponentValues );

      var testSubComponentValues = _grmEventDomain.GetSubComponentValues( "UnitTest", DateTime.Now ).ToList();
      testSubComponentValues.Count.ShouldBe( 1 );
      testSubComponentValues[ 0 ].SubComponentId.ShouldBe( 100 );
      testSubComponentValues[ 0 ].MarketValue.ShouldBe( 100000 );
      testSubComponentValues[ 0 ].DeltaValue.ShouldBe( 105000 );
      testSubComponentValues[ 0 ].Description.ShouldBe( "UnitTestSubComponentValue" );
    }

    [Fact]
    public void GetSubComponentValuesWithInvalidPin()
    {
      var subComponentValues = new List<SubComponentValue>
                               {
                                 new SubComponentValue
                                 {
                                   SubComponentId = 100,
                                   MarketValue = 100000,
                                   DeltaValue = 105000,
                                   Description = "UnitTestSubComponentValue"
                                 }
                               };

      _mockGrmEventRepository.Setup( x => x.GetSubComponentValues( "UnitTest", It.IsAny<DateTime>() ) ).Returns( subComponentValues );

      var testSubComponentValues = _grmEventDomain.GetSubComponentValues( "UnitTestFail", DateTime.Now ).ToList();
      testSubComponentValues.Count.ShouldBe( 0 );
    }

    [Fact]
    public void GetGrmEventInfoByRevObjIdAndEffectiveDateReturnsGrmEventInfo()
    {
      var grmEventInformationList = new List<GrmEventInformation>
                                    {
                                      new GrmEventInformation
                                      {
                                        GrmEventId = 100,
                                        Description = "Unit Test GRM Event",
                                        EffectiveDate = new DateTime( 2015, 2, 1 ),
                                        RevenueObjectId = 100,
                                        EventType = "First Unit Test Event Type",
                                        EventDate = new DateTime( 2015, 1, 1 )
                                      }
                                    };

      _mockGrmEventRepository.Setup( x => x.GetGrmEventInfoByRevObjIdAndEffectiveDate( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( grmEventInformationList );

      var testGrmEventInfo = _grmEventDomain.GetGrmEventInfoByRevObjIdAndEffectiveDate( 100, DateTime.Now ).ToList();
      testGrmEventInfo.Count.ShouldBe( 1 );
      testGrmEventInfo[ 0 ].GrmEventId.ShouldBe( 100 );
      testGrmEventInfo[ 0 ].Description.ShouldBe( "Unit Test GRM Event" );
      testGrmEventInfo[ 0 ].EffectiveDate.ShouldBe( new DateTime( 2015, 2, 1 ) );
      testGrmEventInfo[ 0 ].RevenueObjectId.ShouldBe( 100 );
      testGrmEventInfo[ 0 ].EventType.ShouldBe( "First Unit Test Event Type" );
      testGrmEventInfo[ 0 ].EventDate.ShouldBe( new DateTime( 2015, 1, 1 ) );
    }

    [Fact]
    public void GetGrmEventInfoByRevObjIdAndEffectiveDateInvalidRevObjId()
    {
      var grmEventInformationList = new List<GrmEventInformation>
                                    {
                                      new GrmEventInformation
                                      {
                                        GrmEventId = 100,
                                        Description = "Unit Test GRM Event",
                                        EffectiveDate = new DateTime( 2015, 2, 1 ),
                                        RevenueObjectId = 100,
                                        EventType = "First Unit Test Event Type",
                                        EventDate = new DateTime( 2015, 1, 1 )
                                      }
                                    };

      _mockGrmEventRepository.Setup( x => x.GetGrmEventInfoByRevObjIdAndEffectiveDate( 100, It.IsAny<DateTime>() ) ).Returns( grmEventInformationList );

      var testGrmEventInfo = _grmEventDomain.GetGrmEventInfoByRevObjIdAndEffectiveDate( 200, DateTime.Now ).ToList();
      testGrmEventInfo.Count.ShouldBe( 0 );
    }

    private GrmEventListCreateDto CreateMockDto()
    {
      var grmEventListCreate = new GrmEventListCreateDto();
      var grmEventCreate = new GrmEventCreateDto();

      grmEventCreate.ParentId = -1;
      grmEventCreate.ParentType = Models.V1.GrmEventParentType.Owner;
      grmEventCreate.RevenueObjectId = 100;
      grmEventCreate.EffectiveDateTime = new DateTime( 2016, 1, 1 );
      grmEventCreate.EventType = 200;
      grmEventCreate.GrmEventId = 300;

      grmEventListCreate.GrmEventList.Add( grmEventCreate );

      return grmEventListCreate;
    }
  }
}
