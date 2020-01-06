using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IGrmEventRepository
  {
    Task<GrmEventListCreateDto> CreateAsync( GrmEventListCreateDto grmEventCreateInformation );

    void Delete( IEnumerable<int> ids );

  }
}
