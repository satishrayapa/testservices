using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.AssessmentEvent.Domain.Interfaces;
using TAGov.Services.Core.AssessmentEvent.Domain.Mapping;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Implementation
{
	public class AssessmentEventDomain : IAssessmentEventDomain
	{
		private readonly IAssessmentEventRepository _assesmentEventRepository;
		public AssessmentEventDomain(IAssessmentEventRepository assesmentEventRepository)
		{
			_assesmentEventRepository = assesmentEventRepository;
		}
		public async Task<AssessmentEventDto> GetAsync(int id)
		{
			id.ThrowBadRequestExceptionIfInvalid( "assessmentEventId" );

			var assesmentEventRepoModel = await _assesmentEventRepository.GetAsync(id);

			if ( assesmentEventRepoModel == null )
				throw new RecordNotFoundException(
					id.ToString(), typeof( Repository.Models.V1.AssessmentEvent ), string.Format( "Id {0} is missing.", id ) );

			AssessmentEventDto assessmentEventDto = assesmentEventRepoModel.ToDomain();

			//Primary Base Year
			AssessmentEventTransactionDto assessmentEventTransaction
				= assessmentEventDto.AssessmentEventTransactions.OrderByDescending( aet => aet.Id ).FirstOrDefault();
			if ( assessmentEventTransaction != null )
			{
				AssessmentEventValue assessmentEventValue
					= await _assesmentEventRepository.GetAssessmentEventValueByAssessmentEventTransactionIdAsync( assessmentEventTransaction.Id );
				assessmentEventDto.PrimaryBaseYear =
					assessmentEventValue?.Attribute1;
				assessmentEventDto.PrimaryBaseYearMultipleOrSingleDescription =
					assessmentEventValue?.Attribute2Description;
			}
			else
			{
				assessmentEventDto.PrimaryBaseYear = null;
				assessmentEventDto.PrimaryBaseYearMultipleOrSingleDescription = null;
			}

			return assessmentEventDto;
		}

		public AssessmentRevisionDto GetAssessmentRevisionByAssessmentRevisionEventId(int assessmentRevisionEventId, DateTime effectiveDate)
		{
			assessmentRevisionEventId.ThrowBadRequestExceptionIfInvalid("assessmentRevisionEventId");

			var assessmentRevisionRepoModel =
				_assesmentEventRepository.GetAssessmentRevisionByAssessmentRevisionEventId(assessmentRevisionEventId, effectiveDate);

			assessmentRevisionRepoModel.ThrowRecordNotFoundExceptionIfNull(
				new IdInfo("assessmentRevisionEventId", assessmentRevisionEventId),
				new IdInfo("effectiveDate", effectiveDate));

			return assessmentRevisionRepoModel.ToDomain();
		}

		public IEnumerable<AssessmentEventDto> List(int revenueObjectId, DateTime eventDate)
		{
			return _assesmentEventRepository.List(revenueObjectId, eventDate).Select(x => x.ToDomain()).ToList();
		}

		public async Task<IEnumerable<RevenueObjectBasedAssessmentEventDto>> ListAsync(int assessmentEventId)
		{
			return (await _assesmentEventRepository.ListAsync(assessmentEventId)).Select(x => x.ToDomain());
		}

		public async Task<AssessmentEventIsEditableFlagDto> GetIsEditableFlag(int assessmentEventId)
		{
			return (await _assesmentEventRepository.GetIsEditableFlag(assessmentEventId)).ToDomain();
		}

		public async Task<AssessmentRevisionIdDto> GetCurrentRevisionId(int assessmentEventId)
		{
			var id = await _assesmentEventRepository.GetCurrentRevisionId(assessmentEventId);

			return new AssessmentRevisionIdDto { Value = id };
		}

		public async Task<AssessmentEventEffectiveDateDto> GetEffectiveDate(int assessmentEventId)
		{
			return (await _assesmentEventRepository.GetEffectiveDate(assessmentEventId)).ToDomain();
		}
	}
}
