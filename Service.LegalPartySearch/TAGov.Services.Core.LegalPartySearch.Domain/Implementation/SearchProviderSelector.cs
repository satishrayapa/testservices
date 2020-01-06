using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Implementation
{
	public class SearchProviderSelector : ISearchProviderSelector
	{
		private readonly IDefaultSearchProviderConfiguration _defaultSearchProviderConfiguration;
		private readonly List<ISearchLegalPartyRepository> _searchLegalPartyRepositories;

		public SearchProviderSelector(
			IEnumerable<ISearchLegalPartyRepository> searchLegalPartyRepositories,
			IDefaultSearchProviderConfiguration defaultSearchProviderConfiguration)
		{
			_defaultSearchProviderConfiguration = defaultSearchProviderConfiguration;
			_searchLegalPartyRepositories = searchLegalPartyRepositories.ToList();
		}

		public SearchProvider Get(string rawSearchText)
		{
			if (rawSearchText.StartsWith("--provider"))
			{
				string[] parsed = rawSearchText.Split(' ');

				if (parsed.Length > 2)
				{
					var secondArgAsProviderName = parsed[1];

					var provider = _searchLegalPartyRepositories.SingleOrDefault(x => x.ProviderName == secondArgAsProviderName);
					var parsedSearchText = GetSearchText(parsed, rawSearchText);

					return provider != null ? new SearchProvider
					{
						Provider = provider,
						ParsedSearchText = parsedSearchText
					} : GetProviderFromDefaultConfiguration(parsedSearchText);
				}

				throw new BadRequestException("--provider must be accompied by a valid search provider, then by the search text.");
			}
			return GetProviderFromDefaultConfiguration(rawSearchText);
		}

		private string GetSearchText(string[] parsed, string rawSearchText)
		{
			return rawSearchText.Substring((parsed[0] + " " + parsed[1] + " ").Length);
		}

		private SearchProvider GetProviderFromDefaultConfiguration(string rawSearchText)
		{
			var defaultName = _defaultSearchProviderConfiguration.DefaultName;

			if (string.IsNullOrEmpty(defaultName))
				throw new InternalServerErrorException("Configuration value for Default Search Provider is not set.");

			return new SearchProvider
			{
				Provider = _searchLegalPartyRepositories.Single(x => x.ProviderName == defaultName),
				ParsedSearchText = rawSearchText
			};
		}
	}
}
