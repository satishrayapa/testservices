using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1
{
	public interface IRebuildSearchLegalPartyIndexRepository
	{
		Task RebuildSearchLegalPartyIndexByLegalPartyId(IEnumerable<int> legalPartyIdList);
		Task RebuildSearchLegalPartyIndexAll();
		void RebuildAll();
		CrawlProgress CrawlProgress();
		void EnableChangeTracking();
		void DisableChangeTracking();
	}
}
