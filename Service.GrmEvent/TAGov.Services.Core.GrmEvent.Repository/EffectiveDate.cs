using System;

namespace TAGov.Services.Core.GrmEvent.Repository
{
  public static class EffectiveDate
  {
    public const string MaxDate = "12/31/9999";
    public const string MaxDateDefaultValue = "12/31/9999 23:59:59.997";

    public static DateTime CalculateNewEffectiveDate( DateTime effectiveDate )
    {
      return effectiveDate >= Convert.ToDateTime( MaxDate ) ? Convert.ToDateTime( MaxDateDefaultValue ) : effectiveDate.AddDays( 1 );
    }

    public static DateTime CalculateEffectiveDate( this DateTime effectiveDate )
    {
      return CalculateNewEffectiveDate( effectiveDate );
    }
  }
}