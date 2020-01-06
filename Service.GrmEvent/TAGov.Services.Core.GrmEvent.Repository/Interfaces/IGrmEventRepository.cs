using System;
using System.Collections.Generic;
using TAGov.Services.Core.GrmEvent.Repository.Models.V1;

namespace TAGov.Services.Core.GrmEvent.Repository.Interfaces
{
  public interface IGrmEventRepository
  {
    Models.V1.GrmEvent Get( int id );

    IEnumerable<Models.V1.GrmEventInformation> GetGrmEventInfo( int[] grmEventIdList );

    IEnumerable<GrmEventInformation> GetGrmEventInfoByRevObjIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate );

    IEnumerable<SubComponentValue> GetSubComponentValues( string pin, DateTime effectiveDate );

    IEnumerable<GrmEventCreate> CreateGrmEvents( IEnumerable<GrmEventComponentCreate> grmEventComponentCreate );

    Models.V1.GrmEvent Get( int revenueObjectId, DateTime effectiveDate, int eventType );

    void Delete( int grmEventId );
  }
}