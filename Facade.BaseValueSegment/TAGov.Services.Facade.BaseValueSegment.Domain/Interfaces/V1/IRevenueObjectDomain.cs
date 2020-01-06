using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IRevenueObjectDomain
  {
    Task<IEnumerable<MarketAndRestrictedValueDto>> GetMarketAndRestrictedValues( DateTime assessmentEventDate, int revenueObjectId );
  }
}