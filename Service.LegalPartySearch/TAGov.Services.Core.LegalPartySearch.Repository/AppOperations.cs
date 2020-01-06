using Microsoft.EntityFrameworkCore;
using System;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository
{
	public static class AppOperations
	{
		public static CrawlProgress GetCrawlProgress(string connectionString, int timeoutInSeconds)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Connection string cannot be null or empty string.");

			var contextOptions = new DbContextOptionsBuilder<SearchLegalPartyContext>();
			contextOptions.UseSqlServer(connectionString);
			using (var db = new SearchLegalPartyContext(contextOptions.Options))
			{
				//workaround to make sure migrations use the connection string specified in appsettings.json, if specified
				Environment.SetEnvironmentVariable(SearchLegalPartyContext.SearchLegalPartyContextConnectionStringEnvironmentVariable, connectionString);

				var rebuildSearchLegalPartyIndexRepository = new RebuildSearchLegalPartyIndexRepository(db, null, new AppCommandTimeoutConfiguration
				{
					CommandTimeout = timeoutInSeconds
				});

				return rebuildSearchLegalPartyIndexRepository.CrawlProgress();
			}
		}

		public static void RebuildAll(string connectionString, int timeoutInSeconds, Action<string> message)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Connection string cannot be null or empty string.");

			var contextOptions = new DbContextOptionsBuilder<SearchLegalPartyContext>();
			contextOptions.UseSqlServer(connectionString);
			using (var db = new SearchLegalPartyContext(contextOptions.Options))
			{
				//workaround to make sure migrations use the connection string specified in appsettings.json, if specified
				Environment.SetEnvironmentVariable(SearchLegalPartyContext.SearchLegalPartyContextConnectionStringEnvironmentVariable, connectionString);

				var rebuildSearchLegalPartyIndexRepository = new RebuildSearchLegalPartyIndexRepository(db, null, new AppCommandTimeoutConfiguration
				{
					CommandTimeout = timeoutInSeconds
				});

				message("Disabling change tracking.");
				rebuildSearchLegalPartyIndexRepository.DisableChangeTracking();
				message("Change tracking disabled successfully. Starting rebuild all.");
				rebuildSearchLegalPartyIndexRepository.RebuildAll();
				message("Completed rebuild all. Enabling change tracking.");
				rebuildSearchLegalPartyIndexRepository.EnableChangeTracking();
				message("Change tracking enabled successfully.");
			}
		}
	}
}