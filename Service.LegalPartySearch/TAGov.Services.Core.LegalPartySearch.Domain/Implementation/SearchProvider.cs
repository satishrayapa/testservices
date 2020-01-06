using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Implementation
{
	public class SearchProvider
	{
		public ISearchLegalPartyRepository Provider { get; set; }
		public string ParsedSearchText { get; set; }
	}
}
