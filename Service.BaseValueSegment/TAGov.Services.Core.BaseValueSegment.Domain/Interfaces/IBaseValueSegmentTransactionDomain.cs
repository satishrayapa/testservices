using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IBaseValueSegmentTransactionDomain
  {
    Task<BaseValueSegmentTransactionDto> CreateAsync( BaseValueSegmentTransactionDto baseValueSegmentTransactionDto );
    Task<BaseValueSegmentTransactionDto> GetAsync( int id );
  }
}
