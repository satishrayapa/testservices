using TAGov.Common;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public static class Extensions
  {
    public static void ThrowBadRequestExceptionIfRevenueObjectIdInvalid( this int revenueObjectId )
    {
      revenueObjectId.ThrowBadRequestExceptionIfInvalid( "RevenueObjectId" );
    }
  }
}
