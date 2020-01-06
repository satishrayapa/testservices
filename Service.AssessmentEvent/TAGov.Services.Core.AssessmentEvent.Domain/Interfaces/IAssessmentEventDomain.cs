using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Interfaces
{
	public interface IAssessmentEventDomain
	{
		Task<AssessmentEventDto> GetAsync(int id);

		AssessmentRevisionDto GetAssessmentRevisionByAssessmentRevisionEventId(int assessmentRevisionEventId, DateTime effectiveDate);

		IEnumerable<AssessmentEventDto> List(int revenueObjectId, DateTime eventDate);

		Task<IEnumerable<RevenueObjectBasedAssessmentEventDto>> ListAsync(int assessmentEventId);
		Task<AssessmentEventIsEditableFlagDto> GetIsEditableFlag(int assessmentEventId);

		Task<AssessmentRevisionIdDto> GetCurrentRevisionId(int assessmentEventId);

		Task<AssessmentEventEffectiveDateDto> GetEffectiveDate(int assessmentEventId);
	}
}
