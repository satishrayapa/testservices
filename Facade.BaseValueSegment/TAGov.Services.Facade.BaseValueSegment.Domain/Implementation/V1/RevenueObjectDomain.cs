using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class RevenueObjectDomain : IRevenueObjectDomain
  {
    private readonly IRevenueObjectRepository _revenueObjectRepository;

    public RevenueObjectDomain( IRevenueObjectRepository revenueObjectRepository )
    {
      _revenueObjectRepository = revenueObjectRepository;
    }

    public async Task<IEnumerable<MarketAndRestrictedValueDto>> GetMarketAndRestrictedValues( DateTime assessmentEventDate, int revenueObjectId )
    {
      return ( await _revenueObjectRepository.Get( revenueObjectId, assessmentEventDate ) ).MarketAndRestrictedValues.ToArray();
    }
  }
}