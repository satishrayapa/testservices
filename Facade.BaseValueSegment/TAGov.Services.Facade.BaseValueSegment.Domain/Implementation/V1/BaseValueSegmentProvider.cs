using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class BaseValueSegmentProvider : IBaseValueSegmentProvider
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IAssessmentEventRepository _assessmentEventRepository;

    public BaseValueSegmentProvider(
      IBaseValueSegmentRepository baseValueSegmentRepository,
      IAssessmentEventRepository assessmentEventRepository )
    {
      _baseValueSegmentRepository = baseValueSegmentRepository;
      _assessmentEventRepository = assessmentEventRepository;
    }

    public async Task<Tuple<BaseValueSegmentDto, BaseValueSegmentDto>> GetCurrentAndPrevious( int assessmentEventId )
    {
      var assessmentEventsWithSameRevenueObjectId = ( await _assessmentEventRepository.ListAsync( assessmentEventId ) )
        .OrderBy( x => x.EventDate.Date )
        .ThenBy( x => x.Id ).ToList();

      var selectedAssessmentEvent = assessmentEventsWithSameRevenueObjectId.SingleOrDefault( x => x.Id == assessmentEventId );

      if ( selectedAssessmentEvent == null ) throw new NotFoundException( $"Assessment event Id: {assessmentEventId} is invalid." );

      var sameDayAssessments = assessmentEventsWithSameRevenueObjectId
        .Where( x => x.EventDate.Date == selectedAssessmentEvent.EventDate.Date )
        .ToList();

      var sequence = sameDayAssessments.Count == 1 ? 1 : sameDayAssessments.IndexOf( selectedAssessmentEvent ) + 1;

      List<BaseValueSegmentInfoDto> baseValueSegmentDtos = ( await _baseValueSegmentRepository.GetListAsync( selectedAssessmentEvent.RevObjId ) )
                                                           .Where( x => x.AsOf.Date < selectedAssessmentEvent.EventDate.Date ||
                                                                        ( x.AsOf.Date == selectedAssessmentEvent.EventDate.Date && x.SequenceNumber <= sequence ) )
                                                           .OrderByDescending( x => x.AsOf.Date ).ThenByDescending( x => x.SequenceNumber ).ToList();

      BaseValueSegmentDto current =
        // ReSharper disable once PossibleInvalidOperationException
        baseValueSegmentDtos.Count == 0 ? null : await _baseValueSegmentRepository.GetAsync( baseValueSegmentDtos.First().Id.Value );

      BaseValueSegmentDto previous =
        // ReSharper disable once PossibleInvalidOperationException
        baseValueSegmentDtos.Count > 1 ? await _baseValueSegmentRepository.GetAsync( baseValueSegmentDtos[ 1 ].Id.Value ) : null;

      return new Tuple<BaseValueSegmentDto, BaseValueSegmentDto>( current, previous );
    }
  }
}