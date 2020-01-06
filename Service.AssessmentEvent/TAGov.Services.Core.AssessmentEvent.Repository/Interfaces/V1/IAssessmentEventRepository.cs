using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1
{
  public interface IAssessmentEventRepository
  {
    Task<Models.V1.AssessmentEvent> GetAsync( int id );

    /// <summary>
    /// Get the assessment event revision.
    /// </summary>
    /// <param name="assessmentRevisionEventId">comes from the AssessmentEventTransaction</param>
    /// <param name="effectiveDate">comes from the AssessmentEvent</param>
    /// <returns>assessment revision</returns>
    AssessmentRevision GetAssessmentRevisionByAssessmentRevisionEventId( int assessmentRevisionEventId, DateTime effectiveDate );

    Task<AssessmentEventValue> GetAssessmentEventValueByAssessmentEventTransactionIdAsync( int assessmentEventTransactionId );

    IEnumerable<Models.V1.AssessmentEvent> List( int revenueObjectId, DateTime eventDate );

    /// <summary>
    /// Gets a list of Assessment Events with the same RevenueObjectId based on a root assessment event.
    /// </summary>
    /// <param name="assessmentEventId"></param>
    /// <returns></returns>
    Task<IEnumerable<RevenueObjectBasedAssessmentEvent>> ListAsync( int assessmentEventId );

    /// <summary>
    /// Gets whether we are able to edit the Base Value Segment.
    /// </summary>
    /// <param name="assessmentEventId">Assessment Id</param>
    /// <returns></returns>
    Task<AssessmentEventIsEditableFlag> GetIsEditableFlag( int assessmentEventId );

    Task<int> GetCurrentRevisionId( int assessmentEventId );

    Task<AssessmentEventEffectiveDate> GetEffectiveDate( int assessmentEventId );
  }
}
