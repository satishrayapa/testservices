using TAGov.Services.Core.AssessmentEvent.Domain.Interfaces;
using TAGov.Services.Core.AssessmentEvent.Domain.Mapping;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Implementation
{
   public class StatutoryReferenceDomain: IStatutoryReferenceDomain
	{
		private readonly IStatutoryReferenceRepository _statutoryReferenceRepository;

		public StatutoryReferenceDomain(IStatutoryReferenceRepository statutoryReferenceRepository)
		{
			_statutoryReferenceRepository = statutoryReferenceRepository;
		}

		public StatutoryReferenceDto GetStatutoryReferenceByAssessmentTransactionId(int assessmentTransactionId)
		{
			return _statutoryReferenceRepository.GetStatutoryReferenceByAssessmentTransactionId(assessmentTransactionId).ToDomain();
		}
	}
}
