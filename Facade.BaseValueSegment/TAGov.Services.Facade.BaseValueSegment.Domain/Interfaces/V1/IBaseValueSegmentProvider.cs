using System;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IBaseValueSegmentProvider
  {
    Task<Tuple<BaseValueSegmentDto, BaseValueSegmentDto>> GetCurrentAndPrevious( int assessmentEventId );
  }
}