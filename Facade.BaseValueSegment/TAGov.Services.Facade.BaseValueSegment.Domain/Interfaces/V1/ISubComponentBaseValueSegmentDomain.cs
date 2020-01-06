using System.Threading.Tasks;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface ISubComponentBaseValueSegmentDomain
  {
    Task<SubComponentDto> Get( int assessmentEventId );
  }
}