using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IGrmEventRepository : IRepositoryBase
  {
    Task<IEnumerable<GrmEventInformationDto>> SearchAsync( GrmEventSearchDto grmEventSearchDto );
    Task<IEnumerable<GrmEventInformationDto>> GetAsync( int revenueObjectId, DateTime asOf );
    Task<GrmEventDto> CreateGrmEvent( GrmEventComponentDto grmEventComponent );
  }
}