using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IBaseValueSegmentHistoryDomain
  {
    Task<IEnumerable<BvsHistoryDetailDto>> GetBaseValueSegmentHistoryAsync( string pin, DateTime fromDate, DateTime toDate );
    Task<BvsPinHistoryDto> GetBaseValueSegmentPinHistoryAsync( string pin );
  }
}