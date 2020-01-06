using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IBaseValueSegmentFlagDomain
  {
    Task<IEnumerable<BaseValueSegmentFlagDto>> ListAsync( int revenueObjectId );
  }
}
