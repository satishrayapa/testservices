
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IBaseValueSegmentRepository
  {
    Task<BaseValueSegmentDto> GetAsync( int id );

    Task<IEnumerable<BaseValueSegmentEventDto>> GetEventsAsync( int revenueObjectId );

    Task<FactorBaseYearValueDetailDto> GetFactorBaseYearValueDetail( DateTime asOf, int baseYear, decimal amount, int assessmentEventType );

    Task<IEnumerable<SubComponentDetailDto>> GetSubComponentDetails( int revenueObjectId, DateTime asOf );

    Task<IEnumerable<BeneficialInterestEventDto>> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOf );
    Task<BaseValueSegmentInfoDto> GetAsync( int revenueObjectId, DateTime asOf, int sequenceNumber );
    Task<BaseValueSegmentDto> CreateAsync( BaseValueSegmentDto baseValueSegmentDto );
    Task CreateTransactionAsync( BaseValueSegmentTransactionDto baseValueSegmentTransactionDto );
    Task<IEnumerable<BaseValueSegmentInfoDto>> GetListAsync( int revenueObjectId, DateTime asOf );

    Task<IEnumerable<BaseValueSegmentInfoDto>> GetListAsync( int revenueObjectId );

    Task<IEnumerable<BaseValueSegmentConclusionDto>> GetConclusionsData( int revenueObjectId, DateTime effectiveDate );

    Task<IEnumerable<BaseValueSegmentHistoryDto>> GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate, DateTime toDate );
  }
}