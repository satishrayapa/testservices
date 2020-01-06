using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1
{
  public interface IBaseValueSegmentRepository
  {
    Task<Models.V1.BaseValueSegment> CreateAsync( Models.V1.BaseValueSegment baseValueSegment, IEnumerable<BaseValueSegmentOwnerValue> baseValueSegmentOwnerValues );

    Models.V1.BaseValueSegment Get( int id );
    Models.V1.BaseValueSegment GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId, DateTime assessmentEventDate );

    IEnumerable<BaseValueSegmentEvent> GetBvsEventsByRevenueObjectId( int revenueObjectId );
    Task<IEnumerable<SubComponentDetail>> GetSubComponentDetailsByRevenueObjectId( int revenueObjectId, DateTime asOfDate );
    IEnumerable<BeneficialInterestEvent> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate );
    BaseValueSegmentTransactionType GetUserTransactionType();
    BaseValueSegmentTransactionType GetUserDeletedTransactionType();
    BaseValueSegmentStatusType GetNewStatusType();
    IEnumerable<BaseValueSegmentConclusion> GetBaseValueSegmentConclusions( int revenueObjectId, DateTime effectiveDate );
    Models.V1.BaseValueSegment Get( int revenueObjectId, DateTime asOf, int sequenceNumber );
    IEnumerable<Models.V1.BaseValueSegment> List( int revenueObjectId );
    IEnumerable<Models.V1.BaseValueSegmentHistory> GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate, DateTime toDate );
  }
}
