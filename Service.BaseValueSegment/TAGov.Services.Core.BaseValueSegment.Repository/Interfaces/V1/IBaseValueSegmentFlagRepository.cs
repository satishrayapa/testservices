using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1
{
  public interface IBaseValueSegmentFlagRepository
  {
    Task<IEnumerable<BaseValueSegmentFlag>> ListAsync( int revenueObjectId );
  }
}
