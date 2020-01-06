using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class GrmEventDomainTests
  {
    private readonly Mock<IGrmEventRepository> _grmEventRepository;
    private readonly GrmEventDomain _grmEventDomain;

    public GrmEventDomainTests()
    {
      _grmEventRepository = new Mock<IGrmEventRepository>();
      _grmEventDomain = new GrmEventDomain( _grmEventRepository.Object );
    }

    private BaseValueSegmentDto MockData()
    {
      var owner1 = new BaseValueSegmentOwnerDto { GRMEventId = 101 };
      var owner2 = new BaseValueSegmentOwnerDto();
      var owner3 = new BaseValueSegmentOwnerDto { GRMEventId = 202 }; // Testing the distint part of the query
      var owner4 = new BaseValueSegmentOwnerDto { GRMEventId = 202 };

      var transaction = new BaseValueSegmentTransactionDto();

      transaction.BaseValueSegmentOwners.Add( owner1 );
      transaction.BaseValueSegmentOwners.Add( owner2 );
      transaction.BaseValueSegmentOwners.Add( owner3 );
      transaction.BaseValueSegmentOwners.Add( owner4 );

      var header1 = new BaseValueSegmentValueHeaderDto { GRMEventId = 400 };
      var header2 = new BaseValueSegmentValueHeaderDto();
      var header3 = new BaseValueSegmentValueHeaderDto { GRMEventId = 510 }; // Testing the distint part of the query
      var header4 = new BaseValueSegmentValueHeaderDto { GRMEventId = 510 };
      var header5 = new BaseValueSegmentValueHeaderDto { GRMEventId = 202 };

      transaction.BaseValueSegmentValueHeaders.Add( header1 );
      transaction.BaseValueSegmentValueHeaders.Add( header2 );
      transaction.BaseValueSegmentValueHeaders.Add( header3 );
      transaction.BaseValueSegmentValueHeaders.Add( header4 );
      transaction.BaseValueSegmentValueHeaders.Add( header5 );

      var baseValueSegmentDto = new BaseValueSegmentDto
                                {
                                  RevenueObjectId = 4565,
                                  AsOf = new DateTime( 2011, 8, 1 )
                                };

      baseValueSegmentDto.BaseValueSegmentTransactions.Add( transaction );
      return baseValueSegmentDto;
    }

    [Fact]
    public void GetOwnerGrmEventsWhereSearchGrmEventIdListIsUnique()
    {
      _grmEventRepository.Setup( x => x.GetAsync( 4565, new DateTime( 2011, 8, 1 ) ) )
                         .ReturnsAsync( () => new List<GrmEventInformationDto>() );

      _grmEventRepository.Setup( x => x.SearchAsync( It.IsAny<GrmEventSearchDto>() ) )
                         .ReturnsAsync( () => new List<GrmEventInformationDto>() );

      var result = _grmEventDomain.GetOwnerGrmEvents( MockData() ).Result;

      result.ShouldBeEmpty();

      _grmEventRepository.Verify( x => x.SearchAsync( It.Is<GrmEventSearchDto>( y =>
                                                                                  y.GrmEventIdList.Count == 4 &&
                                                                                  y.GrmEventIdList.Contains( 101 ) &&
                                                                                  y.GrmEventIdList.Contains( 202 ) &&
                                                                                  y.GrmEventIdList.Contains( 510 ) &&
                                                                                  y.GrmEventIdList.Contains( 400 ) ) ), Times.Once );
    }

    [Fact]
    public void GetOwnerGrmEventsWhereUnionOfSearchAsyncAndGetAsyncProducesUniqueSet()
    {
      var list1 = new List<GrmEventInformationDto>
                  {
                    new GrmEventInformationDto { GrmEventId = 41 },
                    new GrmEventInformationDto { GrmEventId = 42 },
                    new GrmEventInformationDto { GrmEventId = 43 },
                    new GrmEventInformationDto(),
                    new GrmEventInformationDto { GrmEventId = 44 }
                  };

      _grmEventRepository.Setup( x => x.GetAsync( It.IsAny<int>(), It.IsAny<DateTime>() ) )
                         .ReturnsAsync( () => list1 );

      var list2 = new List<GrmEventInformationDto>
                  {
                    new GrmEventInformationDto { GrmEventId = 41 },
                    new GrmEventInformationDto { GrmEventId = 52 },
                    new GrmEventInformationDto(),
                    new GrmEventInformationDto { GrmEventId = 53 }
                  };

      _grmEventRepository.Setup( x => x.SearchAsync( It.IsAny<GrmEventSearchDto>() ) )
                         .ReturnsAsync( () => list2 );

      var result = _grmEventDomain.GetOwnerGrmEvents( MockData() ).Result.ToList();

      result.ShouldNotBeEmpty();
      result.ShouldContain( x => x.GrmEventId == 41 );
      result.ShouldContain( x => x.GrmEventId == 42 );
      result.ShouldContain( x => x.GrmEventId == 43 );
      result.ShouldContain( x => x.GrmEventId == 44 );
      result.ShouldContain( x => x.GrmEventId == 52 );
      result.ShouldContain( x => x.GrmEventId == 53 );
    }

    [Fact]
    public void GetValueHeaderGrmEventsWhereSearchGrmEventIdListIsUnique()
    {
      _grmEventRepository.Setup( x => x.SearchAsync( It.IsAny<GrmEventSearchDto>() ) )
                         .ReturnsAsync( () => new List<GrmEventInformationDto>() );

      var result = _grmEventDomain.GetValueHeaderGrmEvents( MockData() ).Result;

      result.ShouldBeEmpty();

      _grmEventRepository.Verify( x => x.SearchAsync( It.Is<GrmEventSearchDto>( y =>
                                                                                  y.GrmEventIdList.Count == 3 &&
                                                                                  y.GrmEventIdList.Contains( 202 ) &&
                                                                                  y.GrmEventIdList.Contains( 510 ) &&
                                                                                  y.GrmEventIdList.Contains( 400 ) ) ), Times.Once );
    }
  }
}