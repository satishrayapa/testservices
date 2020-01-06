using System.Collections.Generic;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1
{
  public interface ICaliforniaConsumerPriceIndexRepository
  {
    IEnumerable<CaliforniaConsumerPriceIndex> List();

    CaliforniaConsumerPriceIndex GetByYear( int assessmentYear );
  }
}
