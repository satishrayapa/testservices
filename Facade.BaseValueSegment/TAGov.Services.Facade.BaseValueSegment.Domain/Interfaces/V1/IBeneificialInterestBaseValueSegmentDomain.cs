using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IBeneificialInterestBaseValueSegmentDomain
  {
    Task<BeneificialInterestDto> Get( int assessmentEventId );
    Task<IEnumerable<BeneficialInterestEventDto>> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate );
  }
}