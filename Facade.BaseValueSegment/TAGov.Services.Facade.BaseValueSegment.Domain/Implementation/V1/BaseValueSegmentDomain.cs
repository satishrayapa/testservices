using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class BaseValueSegmentDomain : IBaseValueSegmentDomain
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IAssessmentEventRepository _assessmentEventRepository;

    public BaseValueSegmentDomain(
      IBaseValueSegmentRepository baseValueSegmentRepository,
      IAssessmentEventRepository assessmentEventRepository )
    {
      _baseValueSegmentRepository = baseValueSegmentRepository;
      _assessmentEventRepository = assessmentEventRepository;
    }

    public async Task<BaseValueSegmentDto> SaveAsync( int assessmentEventId, BaseValueSegmentDto baseValueSegmentDto )
    {
      if ( baseValueSegmentDto == null ) throw new BadRequestException( "BaseValueSegmentDto is null." );

      var assessmentEvents = await _assessmentEventRepository.ListAsync( baseValueSegmentDto.RevenueObjectId, baseValueSegmentDto.AsOf );
      // 1 based index, NOT 0-based index.
      var sequenceNumber = assessmentEvents.OrderBy( x => x.Id ).Select( ( item, index ) =>
                                                                           new { item.Id, Index = index } ).Single( x => x.Id == assessmentEventId ).Index + 1;

      if ( await IsExist( baseValueSegmentDto, sequenceNumber ) )
      {
        await _baseValueSegmentRepository.CreateTransactionAsync( baseValueSegmentDto.BaseValueSegmentTransactions.Single( x => x.Id.HasValue && x.Id < 0 ) );
        return baseValueSegmentDto;
      }

      baseValueSegmentDto.SequenceNumber = sequenceNumber;
      baseValueSegmentDto.Id = null; // Remember to null out the Id as we are about to create
      // a brand new Base Value Segment!

      // TODO: validate if caller sends multiple creates ie negative number
      baseValueSegmentDto.BaseValueSegmentTransactions =
        baseValueSegmentDto.BaseValueSegmentTransactions.Where( t => t.Id < 0 ).ToList();

      return await _baseValueSegmentRepository.CreateAsync( baseValueSegmentDto );
    }

    private async Task<bool> IsExist( BaseValueSegmentDto baseValueSegmentDto, int sequenceNumber )
    {
      try
      {
        var existing = await _baseValueSegmentRepository.GetAsync( baseValueSegmentDto.RevenueObjectId,
                                                                   baseValueSegmentDto.AsOf, sequenceNumber );

        return existing != null;
      }
      catch ( NotFoundException )
      {
        return false;
      }
      catch ( RecordNotFoundException )
      {
        return false;
      }
    }

    public async Task<BaseValueSegmentDto> GetAsync( int baseValueSegmentId )
    {
      return await _baseValueSegmentRepository.GetAsync( baseValueSegmentId );
    }
  }
}