using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IGrmEventDomain
  {
    Task<IEnumerable<GrmEventInformationDto>> GetOwnerGrmEvents( BaseValueSegmentDto baseValueSegmentDto );
    Task<IEnumerable<GrmEventInformationDto>> GetValueHeaderGrmEvents( BaseValueSegmentDto baseValueSegmentDto );
    Task<GrmEventDto> CreateBvsValueHeaderOverideGrmEvent( int revenueObjectId, DateTime effectiveDate );
  }
}