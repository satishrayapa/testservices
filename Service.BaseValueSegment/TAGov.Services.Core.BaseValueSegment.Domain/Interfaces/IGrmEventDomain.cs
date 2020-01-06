using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IGrmEventDomain
  {
    Task<int[]> Create( BaseValueSegmentDto baseValueSegmentDto );

    Task<int[]> CreateForTransaction( BaseValueSegmentTransactionDto transaction );

    void Delete( int[] ids );
  }
}
