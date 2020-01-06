using System;
using System.Collections.Generic;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.GrmEvent.Domain.Interfaces
{
  public interface IGrmEventDomain
  {
    GrmEventDto Get( int id );

    IEnumerable<GrmEventInformationDto> GetGrmEventInfo( int[] grmEventIdList );

    IEnumerable<GrmEventInformationDto> GetGrmEventInfoByRevObjIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate );

    IEnumerable<SubComponentValueDto> GetSubComponentValues( string pin, DateTime effectiveDate );

    GrmEventListCreateDto CreateGrmEvents( GrmEventListCreateDto grmDto );

    void Delete( int grmEventId );
  }
}
