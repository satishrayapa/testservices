using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Models.V1
{
	public class RebuildSearchLegalPartyDto
	{
		public List<int> LegalPartyIdList { get; set; }

		public List<int> CommIdList { get; set; }

		public List<int> RevenueObjectIdList { get; set; }

		public List<int> SitusAddressIdList { get; set; }

		public List<int> TaxAuthorityGroupIdList { get; set; }

		public List<int> AppraisalSiteIdList { get; set; }
	}
}
