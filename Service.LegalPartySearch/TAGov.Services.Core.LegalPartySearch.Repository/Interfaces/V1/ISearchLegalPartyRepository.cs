using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1
{
    public interface ISearchLegalPartyRepository 
    {
		string ProviderName { get; }
	    Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions);
	    Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate);
	    Task<IEnumerable<SearchLegalParty>> SearchPinAsync(string searchText, SearchLegalPartyQuery toExclusions, bool? isActive, DateTime? effectiveDate);
    }
}
