using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IAssessmentEventRepository
  {
    Task<IEnumerable<RevenueObjectBasedAssessmentEventDto>> ListAsync( int assessmentEventId );

    Task<AssessmentEventDto> Get( int assessmentEventId );

    Task<IEnumerable<AssessmentEventDto>> ListAsync( int revenueObjectId, DateTime eventDate );
  }
}