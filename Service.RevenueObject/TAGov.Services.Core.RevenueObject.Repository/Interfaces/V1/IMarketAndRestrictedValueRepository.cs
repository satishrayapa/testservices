using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1
{
  public interface IMarketAndRestrictedValueRepository
  {
    IList<Models.V1.MarketAndRestrictedValue> Get( int revnueObjectId, DateTime effectiveDate );
  }
}
