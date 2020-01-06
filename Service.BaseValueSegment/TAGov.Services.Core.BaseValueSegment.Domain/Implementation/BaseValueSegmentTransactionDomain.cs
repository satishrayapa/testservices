using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class BaseValueSegmentTransactionDomain : BaseValueSegmentDomainBase, IBaseValueSegmentTransactionDomain
  {
    private readonly IBaseValueSegmentTransactionRepository _baseValueSegmentTransactionRepository;
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IGrmEventDomain _grmEventDomain;

    public BaseValueSegmentTransactionDomain(
      IBaseValueSegmentTransactionRepository baseValueSegmentTransactionRepository,
      IBaseValueSegmentRepository baseValueSegmentRepository,
      IGrmEventDomain grmEventDomain )
    {
      _baseValueSegmentTransactionRepository = baseValueSegmentTransactionRepository;
      _baseValueSegmentRepository = baseValueSegmentRepository;
      _grmEventDomain = grmEventDomain;
    }

    public async Task<BaseValueSegmentTransactionDto> CreateAsync( BaseValueSegmentTransactionDto baseValueSegmentTransactionDto )
    {
      if ( baseValueSegmentTransactionDto.BaseValueSegmentOwners == null ||
           baseValueSegmentTransactionDto.BaseValueSegmentOwners.Count == 0 )
      {
        throw new BadRequestException( "No Base Value Segment Owners defined for the Base Value Segment Transaction" );
      }

      if ( baseValueSegmentTransactionDto.BaseValueSegmentValueHeaders == null ||
           baseValueSegmentTransactionDto.BaseValueSegmentValueHeaders.Count == 0 )
      {
        throw new BadRequestException( "No Base Value Segment Value Headers defined for the Base Value Segment Transaction" );
      }

      var eventsCreated = await _grmEventDomain.CreateForTransaction( baseValueSegmentTransactionDto );

      var baseValueSegmentTransaction = baseValueSegmentTransactionDto.ToEntity();

      PrepareTransactionForCreate( baseValueSegmentTransaction, true );

      var baseValueSegmentOwnerValues = new List<BaseValueSegmentOwnerValue>();

      DiscoverOwnerValuesForSavingInTransaction( () => _baseValueSegmentRepository.GetUserTransactionType(),
                                                 () => _baseValueSegmentRepository.GetUserDeletedTransactionType(),
                                                 baseValueSegmentTransaction,
                                                 baseValueSegmentOwnerValues );

      try
      {
        await _baseValueSegmentTransactionRepository.CreateAsync( baseValueSegmentTransaction, baseValueSegmentOwnerValues );
      }
      catch
      {
        _grmEventDomain.Delete( eventsCreated );
        throw;
      }

      return baseValueSegmentTransaction.ToDomain();

    }

    public async Task<BaseValueSegmentTransactionDto> GetAsync( int id )
    {
      var result = await _baseValueSegmentTransactionRepository.GetAsync( id );
      return result.ToDomain();
    }
  }
}
