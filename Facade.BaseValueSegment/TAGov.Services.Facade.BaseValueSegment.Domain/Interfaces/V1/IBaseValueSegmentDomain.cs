using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IBaseValueSegmentDomain
  {
    Task<BaseValueSegmentDto> SaveAsync( int assessmentEventId, BaseValueSegmentDto baseValueSegmentDto );
    Task<BaseValueSegmentDto> GetAsync( int baseValueSegmentId );
  }
}