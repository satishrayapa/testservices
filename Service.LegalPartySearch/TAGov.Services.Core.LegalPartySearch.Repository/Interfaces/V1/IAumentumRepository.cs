using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1
{
	public interface IAumentumRepository
	{
		IEnumerable<int> GetLegalPartyIdByCommId(int commId);

		IEnumerable<int> GetLegalPartyIdByRevenueObjectId(int revenueObjectId);
		IEnumerable<int> GetLegalPartyIdBySitusAddressId(int situsAddressId);

		IEnumerable<int> GetLegalPartyIdByTaxAuthorityGroupId(int taxAuthorityGroupId);

		IEnumerable<int> GetLegalPartyIdByAppraisalSiteId(int appraisalSiteId);
	}
}
