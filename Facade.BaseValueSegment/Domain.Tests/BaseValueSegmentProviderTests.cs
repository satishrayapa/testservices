using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class BaseValueSegmentProviderTests
  {

    private readonly Mock<IBaseValueSegmentRepository> _baseValueSegmentRepository;
    private readonly Mock<IAssessmentEventRepository> _assessmentEventRepository;
    private readonly BaseValueSegmentProvider _baseValueSegmentProvider;

    public BaseValueSegmentProviderTests()
    {
      _baseValueSegmentRepository = new Mock<IBaseValueSegmentRepository>();
      _assessmentEventRepository = new Mock<IAssessmentEventRepository>();

      _baseValueSegmentProvider = new BaseValueSegmentProvider( _baseValueSegmentRepository.Object, _assessmentEventRepository.Object );
    }

    [Fact]
    public void NoAssessmentsAreReturned_GetNotFoundException()
    {
      const int assessmentEventId = 12345;

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( Enumerable.Empty<RevenueObjectBasedAssessmentEventDto>() );

      Should.ThrowAsync<NotFoundException>( () => _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ) );

    }

    [Fact]
    public void NoBaseValueSegmentsAreReturn_GetNullCurrentAndPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = DateTime.Today }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );
      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( Enumerable.Empty<BaseValueSegmentInfoDto> );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldBeNull();
      result.Item2.ShouldBeNull();

      _baseValueSegmentRepository.Verify( x => x.GetAsync( It.IsAny<int>() ), Times.Never );
    }

    [Fact]
    public void OneNextDayBaseValueSegmentIsReturned_GetNullCurrentAndPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( 1 ), Id = 334, RevenueObjectId = 3, SequenceNumber = 1 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldBeNull();
      result.Item2.ShouldBeNull();

      _baseValueSegmentRepository.Verify( x => x.GetAsync( It.IsAny<int>() ), Times.Never );
    }

    [Fact]
    public void OneSameDayButHigherSequenceBaseValueSegmentIsReturned_GetNullCurrentAndPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = 12346, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = 334, RevenueObjectId = revenueObjectId, SequenceNumber = 2 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldBeNull();
      result.Item2.ShouldBeNull();

      _baseValueSegmentRepository.Verify( x => x.GetAsync( It.IsAny<int>() ), Times.Never );
    }

    [Fact]
    public void OneCurrentDayBaseValueSegmentIsReturned_GetCurrentAndNullPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId = 334;
      const int sequenceNumber = 1;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) )
                                .ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId, RevenueObjectId = 3, SequenceNumber = sequenceNumber }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId, SequenceNumber = sequenceNumber } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldBeNull();

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId );
    }

    [Fact]
    public void OnePreviousDayBaseValueSegmentIsReturned_GetCurrentAndNullPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId = 334;
      const int sequenceNumber = 1;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) )
                                .ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId, RevenueObjectId = 3, SequenceNumber = sequenceNumber }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId, SequenceNumber = sequenceNumber } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldBeNull();

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId );
    }

    [Fact]
    public void OneSameDayAndEqualSequenceBaseValueSegmentIsReturned_GetCurrentAndNullPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId = 43535;
      const int sequenceNumber = 1;
      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = 12346, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId, RevenueObjectId = revenueObjectId, SequenceNumber = sequenceNumber }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId, SequenceNumber = sequenceNumber } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldBeNull();

      result.Item1.Id.ShouldBe( bvsId );
    }

    [Fact]
    public void OneSameDayAndLowerSequenceBaseValueSegmentIsReturned_GetCurrentAndNullPrevious()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId = 43535;
      const int sequenceNumber = 1;
      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 1, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId, RevenueObjectId = revenueObjectId, SequenceNumber = sequenceNumber }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId, SequenceNumber = sequenceNumber } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldBeNull();

      result.Item1.Id.ShouldBe( bvsId );
    }

    [Fact]
    public void TwoCurrentDaysAndOnePreviousDayBaseValueSegmentAreReturnedWhereAssessmentEventCorrespondToFirstOneForDay_GetCurrentFromCurrentDayAndPreviousFromPreviousDay()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId1 = 334;
      const int bvsId2 = 5335;
      const int bvsId3 = 1336;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 1, RevObjId = revenueObjectId, EventDate = eventDate.AddDays( -1 ) },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId + 1, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) )
                                .ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId1, RevenueObjectId = 3, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId2, RevenueObjectId = 3, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId3, RevenueObjectId = 3, SequenceNumber = 2 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId1 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId1, SequenceNumber = 1 } );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId2 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId2, SequenceNumber = 1 } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      _baseValueSegmentRepository.Verify( x => x.GetAsync( bvsId3 ), Times.Never );

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldNotBe( null );

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId2 );
      // ReSharper disable once PossibleInvalidOperationException
      result.Item2.Id.Value.ShouldBe( bvsId1 );
    }

    [Fact]
    public void TwoCurrentDaysAndOnePreviousDayBaseValueSegmentAreReturnedWhereAssessmentEventCorrespondToSecondForDay_GetCurrentFromSecondCurrentDayAndPreviousFromFirstCurrentDay()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;
      const int bvsId1 = 334;
      const int bvsId2 = 5335;
      const int bvsId3 = 1336;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 2, RevObjId = revenueObjectId, EventDate = eventDate.AddDays( -1 ) },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 1, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) )
                                .ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId1, RevenueObjectId = 3, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId2, RevenueObjectId = revenueObjectId, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId3, RevenueObjectId = revenueObjectId, SequenceNumber = 2 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId2 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId2, SequenceNumber = 1 } );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId3 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId3, SequenceNumber = 2 } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      _baseValueSegmentRepository.Verify( x => x.GetAsync( bvsId1 ), Times.Never );

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldNotBe( null );

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId3 );
      // ReSharper disable once PossibleInvalidOperationException
      result.Item2.Id.Value.ShouldBe( bvsId2 );
    }

    [Fact]
    public void TwoCurrentDaysAndTwoPreviousDayBaseValueSegmentAreReturnedWhereAssessmentEventCorrespondToSecondForDay_GetCurrentFromSecondCurrentDayAndPreviousFromFirstCurrentDay()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      var eventDate = DateTime.Today;

      const int bvsId0 = 8334;
      const int bvsId1 = 334;
      const int bvsId2 = 5335;
      const int bvsId3 = 1336;

      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 3, RevObjId = revenueObjectId, EventDate = eventDate.AddDays( -1 ) },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 2, RevObjId = revenueObjectId, EventDate = eventDate.AddDays( -1 ) },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId - 1, RevObjId = revenueObjectId, EventDate = eventDate },
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) )
                                .ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId0, RevenueObjectId = revenueObjectId, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId1, RevenueObjectId = revenueObjectId, SequenceNumber = 2 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId2, RevenueObjectId = revenueObjectId, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate, Id = bvsId3, RevenueObjectId = revenueObjectId, SequenceNumber = 2 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId2 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId2, SequenceNumber = 1 } );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId3 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId3, SequenceNumber = 2 } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      _baseValueSegmentRepository.Verify( x => x.GetAsync( bvsId0 ), Times.Never );
      _baseValueSegmentRepository.Verify( x => x.GetAsync( bvsId1 ), Times.Never );

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldNotBe( null );

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId3 );
      // ReSharper disable once PossibleInvalidOperationException
      result.Item2.Id.Value.ShouldBe( bvsId2 );
    }

    [Fact]
    public void TwoPreviousDayButHigherSequenceBaseValueSegmentIsReturned_GetCurrentFromSecondPreviousDayAndPreviousFromFirstPreviousDay()
    {
      const int assessmentEventId = 12345;
      const int revenueObjectId = 3;
      const int bvsId1 = 334;
      const int bvsId2 = 335;

      var eventDate = DateTime.Today;
      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto { Id = assessmentEventId, RevObjId = revenueObjectId, EventDate = eventDate }
                                                  };

      _assessmentEventRepository.Setup( x => x.ListAsync( assessmentEventId ) ).ReturnsAsync( revenueObjectBasedAssessmentEventDtos );

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId1, RevenueObjectId = revenueObjectId, SequenceNumber = 1 },
                                new BaseValueSegmentInfoDto { AsOf = eventDate.AddDays( -1 ), Id = bvsId2, RevenueObjectId = revenueObjectId, SequenceNumber = 2 }
                              };

      _baseValueSegmentRepository.Setup( x => x.GetListAsync( revenueObjectId ) ).ReturnsAsync( baseValueSegments );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId1 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId1, SequenceNumber = 1 } );
      _baseValueSegmentRepository.Setup( x => x.GetAsync( bvsId2 ) ).ReturnsAsync( new BaseValueSegmentDto { Id = bvsId2, SequenceNumber = 2 } );

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId ).Result;

      result.Item1.ShouldNotBe( null );
      result.Item2.ShouldNotBe( null );

      // ReSharper disable once PossibleInvalidOperationException
      result.Item1.Id.Value.ShouldBe( bvsId2 );
      // ReSharper disable once PossibleInvalidOperationException
      result.Item2.Id.Value.ShouldBe( bvsId1 );
    }

    [Fact]
    public void SameDayEventsSameDayBaseValueSegmentGenerated_GetCurrentAndPrevious()
    {
      const int assessmentEventId = 22267731;
      const int revenueObjectId = 534290066;
      const int bvsIdFirstEvent = 12737597;
      const int bvsIdSecondEvent = 12737598;
      const int bvsIdPreviousToTransferEvent = 12737596;
      var eventDate = new DateTime(2017, 7, 7);
      var revenueObjectBasedAssessmentEventDtos = new List<RevenueObjectBasedAssessmentEventDto>
                                                  {
                                                    new RevenueObjectBasedAssessmentEventDto
                                                    {
                                                      Id = 13037530,
                                                      RevObjId = revenueObjectId,
                                                      EventDate = new DateTime( 2017,1,1 )
                                                    },
                                                    new RevenueObjectBasedAssessmentEventDto
                                                    {
                                                      Id = 24532601,
                                                      RevObjId = revenueObjectId,
                                                      EventDate = eventDate
                                                    },
                                                    new RevenueObjectBasedAssessmentEventDto
                                                    {
                                                      Id = assessmentEventId,
                                                      RevObjId = revenueObjectId,
                                                      EventDate = eventDate
                                                    },
                                                    new RevenueObjectBasedAssessmentEventDto
                                                    {
                                                      Id = 13037531,
                                                      RevObjId = revenueObjectId,
                                                      EventDate = new DateTime( 2018,1,1 )
                                                    }
                                                  };

      _assessmentEventRepository
        .Setup(x => x.ListAsync(assessmentEventId))
        .ReturnsAsync(revenueObjectBasedAssessmentEventDtos);

      var baseValueSegments = new List<BaseValueSegmentInfoDto>
                              {
                                new BaseValueSegmentInfoDto
                                {
                                  AsOf = new DateTime( 2017, 1, 1 ),
                                  Id = bvsIdPreviousToTransferEvent,
                                  RevenueObjectId = revenueObjectId,
                                  SequenceNumber = 1
                                },
                                new BaseValueSegmentInfoDto
                                {
                                  AsOf = eventDate,
                                  Id = bvsIdFirstEvent,
                                  RevenueObjectId = revenueObjectId,
                                  SequenceNumber = 1
                                },
                                new BaseValueSegmentInfoDto
                                {
                                  AsOf = eventDate,
                                  Id = bvsIdSecondEvent,
                                  RevenueObjectId = revenueObjectId,
                                  SequenceNumber = 2
                                },
                                new BaseValueSegmentInfoDto
                                {
                                  AsOf = new DateTime( 2018, 1, 1 ),
                                  Id = 12737599,
                                  RevenueObjectId = revenueObjectId,
                                  SequenceNumber = 1
                                }
                              };

      _baseValueSegmentRepository
        .Setup(x => x.GetListAsync(revenueObjectId))
        .ReturnsAsync(baseValueSegments);

      _baseValueSegmentRepository
        .Setup(x => x.GetAsync(bvsIdPreviousToTransferEvent))
        .ReturnsAsync(new BaseValueSegmentDto { Id = bvsIdPreviousToTransferEvent });

      _baseValueSegmentRepository
        .Setup(x => x.GetAsync(bvsIdFirstEvent))
        .ReturnsAsync(new BaseValueSegmentDto { Id = bvsIdFirstEvent });

      _baseValueSegmentRepository
        .Setup(x => x.GetAsync(bvsIdSecondEvent))
        .ReturnsAsync(new BaseValueSegmentDto { Id = bvsIdSecondEvent });

      var result = _baseValueSegmentProvider.GetCurrentAndPrevious(assessmentEventId).Result;

      result.Item1.ShouldNotBe(null);
      result.Item1.Id.ShouldBe(bvsIdFirstEvent);

      result.Item2.ShouldNotBe(null);
      result.Item2.Id.ShouldBe(bvsIdPreviousToTransferEvent);

      _baseValueSegmentRepository.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Exactly(2));
    }
  }
}