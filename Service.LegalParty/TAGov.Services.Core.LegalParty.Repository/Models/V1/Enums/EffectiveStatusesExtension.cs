using System;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Constants;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums
{

  public static class EffectiveStatusesExt
  {
    public static string GetEffectiveStatusStringFromEnum( this EffectiveStatuses effectiveStatus )
    {
      switch ( effectiveStatus )
      {
        case EffectiveStatuses.Active:
          return EffectiveStatus.Active;
        case EffectiveStatuses.InActive:
          return EffectiveStatus.Inactive;
        default:
          throw new ArgumentException( "Enum is not mapped: " + effectiveStatus );
      }
    }

    public static EffectiveStatuses GetEffectiveStatusEnumFromString( this string value )
    {
      switch ( value )
      {
        case EffectiveStatus.Active:
          return EffectiveStatuses.Active;
        case EffectiveStatus.Inactive:
          return EffectiveStatuses.InActive;
        default:
          throw new ArgumentException( "Unknown EffectiveStatus value: " + value );
      }
    }
  }
}