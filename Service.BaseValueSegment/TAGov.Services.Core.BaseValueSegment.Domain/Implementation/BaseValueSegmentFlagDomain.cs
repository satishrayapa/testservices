using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class BaseValueSegmentFlagDomain : IBaseValueSegmentFlagDomain
  {
    private readonly IBaseValueSegmentFlagRepository _baseValueSegmentFlagRepository;

    public BaseValueSegmentFlagDomain( IBaseValueSegmentFlagRepository baseValueSegmentFlagRepository )
    {
      _baseValueSegmentFlagRepository = baseValueSegmentFlagRepository;
    }

    public async Task<IEnumerable<BaseValueSegmentFlagDto>> ListAsync( int revenueObjectId )
    {
      var list = ( await _baseValueSegmentFlagRepository.ListAsync( revenueObjectId ) ).ToList();

      if ( !list.Any() ) throw new RecordNotFoundException( revenueObjectId.ToString(), typeof( BaseValueSegmentFlagDto ), "No Flags are associated with this particular Revenue Object Id." );

      return list.Select( x => x.ToDomain() ).ToList();
    }
  }
}
