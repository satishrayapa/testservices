using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Interfaces
{
    public interface IStatutoryReferenceDomain
    {
		StatutoryReferenceDto GetStatutoryReferenceByAssessmentTransactionId(int assessmentTransactionId);
	}
}
