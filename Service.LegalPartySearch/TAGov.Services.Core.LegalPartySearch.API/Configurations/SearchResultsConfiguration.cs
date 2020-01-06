using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;

namespace TAGov.Services.Core.LegalPartySearch.API.Configurations
{
	public class SearchResultsConfiguration : ISearchResultsConfiguration
	{
		private readonly IConfiguration _configuration;
		private const int DefaultMaxRows = 25000;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">configuration containing command timeout</param>
		public SearchResultsConfiguration(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <inheritdoc />
		public int MaxRows
		{
			get
			{
				if (int.TryParse(_configuration["SearchResults:MaxRows"], out int maxrows))
					return maxrows;
				return DefaultMaxRows;
			}
		}
	}
}
