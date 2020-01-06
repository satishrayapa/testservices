using System;
using System.Collections.Generic;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IAumentumDomain
  {
    IEnumerable<CaliforniaConsumerPriceIndexDto> GetAllCaConsumerPriceIndexes();

    FactorBaseYearValueDetailDto GetFactoredBasedYearValue( DateTime assessmentDate, int baseYear, decimal baseValue, int assessmentEventType );

    FactorBaseYearValueRequestDto[] GetFactoredBasedYearValue( FactorBaseYearValueRequestDto[] factorBaseYearValueRequestDtos );

  }
}
