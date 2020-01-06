using TAGov.Services.Core.LegalPartySearch.Domain.Implementation;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Interfaces
{
	public interface ISearchProviderSelector
	{
		SearchProvider Get(string rawSearchText);
	}
}
