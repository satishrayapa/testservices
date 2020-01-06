using System.Collections.Generic;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class GrmEventInformationDtoComparer : IEqualityComparer<GrmEventInformationDto>
  {
    public bool Equals( GrmEventInformationDto x, GrmEventInformationDto y )
    {
      return ( x.GrmEventId == y.GrmEventId );
    }

    public int GetHashCode( GrmEventInformationDto obj )
    {
      return obj.GrmEventId.GetHashCode();
    }
  }
}