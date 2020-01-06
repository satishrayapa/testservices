using System.Collections.Generic;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using System;
using System.Threading.Tasks;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Interfaces
{
  public interface IBaseValueSegmentDomain
  {
    Task<BaseValueSegmentDto> CreateAsync( BaseValueSegmentDto baseValueSegment );
    BaseValueSegmentDto Get( int id );
    BaseValueSegmentDto GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId, DateTime assessmentEventDate );
    IEnumerable<BaseValueSegmentEventDto> GetBvsEventsByRevenueObjectId( int revenueObjectId );
    Task<IEnumerable<SubComponentDetailDto>> GetSubComponentDetailsByRevenueObjectId( int revenueObjectId, DateTime asOfDate );
    IEnumerable<BeneficialInterestEventDto> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate );
    IEnumerable<BaseValueSegmentConclusionDto> GetBaseValueSegmentConclusions( int revenueObjectId, DateTime effectiveDate );
    BaseValueSegmentInfoDto Get( int revenueObjectId, DateTime asOf, int sequenceNumber );
    IEnumerable<BaseValueSegmentInfoDto> List( int revenueObjectId );
    IEnumerable<BaseValueSegmentHistoryDto> GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate, DateTime toDate );
  }
}
