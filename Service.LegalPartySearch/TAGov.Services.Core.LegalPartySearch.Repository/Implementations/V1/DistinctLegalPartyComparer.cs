using System.Collections.Generic;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class DistinctLegalPartyComparer : IEqualityComparer<SearchLegalParty>
	{

		public bool Equals(SearchLegalParty x, SearchLegalParty y)
		{
			return x.LegalPartyId == y.LegalPartyId &&
			       x.AddressId == y.AddressId && x.RevenueObjectId == y.RevenueObjectId;
				  
		}

		public int GetHashCode(SearchLegalParty legalParty)
		{
			return legalParty.LegalPartyId.GetHashCode() ^
				   legalParty.AddressId.GetHashCode() ^ legalParty.RevenueObjectId.GetHashCode();
		}
	}
}
