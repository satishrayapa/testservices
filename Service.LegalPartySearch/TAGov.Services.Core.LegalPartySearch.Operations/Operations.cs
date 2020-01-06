using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using TAGov.Common.Operations;
using TAGov.Services.Core.LegalPartySearch.Repository;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Operations
{
	public class Operations : IOperations
	{
		private const int OneHour = 3600;

		private CommandOption _rebuildFullSeed;
		private CommandOption _crawlProgress;

		public IEnumerable<string> GetEfMigrations(IConfiguration configuration)
		{
			return AppMigrations.GetPendingMigrations(configuration.GetConnectionString("Aumentum"));
		}

		public void ApplyEfMigrations(IConfiguration configuration)
		{
			int timeout;
			if (!int.TryParse(configuration["Db:TimeoutInSeconds"], out timeout))
			{
				timeout = OneHour;
			}

			AppMigrations.Apply(configuration.GetConnectionString("Aumentum"), timeout);
		}


		public void AddOption(CommandLineApplication commandLineApplication)
		{
			_rebuildFullSeed = commandLineApplication.Option("--legalpartysearch-rebuild-all",
			   "Reseed Legalpartysearch table",
			   CommandOptionType.NoValue);

			_crawlProgress = commandLineApplication.Option("--legalpartysearch-crawl-progress",
				"CrawlProgress Progress",
				CommandOptionType.NoValue);

		}

		public int Apply(IConfiguration configuration)
		{
			if (_rebuildFullSeed.HasValue())
			{
				int timeout;
				if (!int.TryParse(configuration["Db:TimeoutInSeconds"], out timeout))
				{
					timeout = OneHour;
				}

				AppOperations.RebuildAll(configuration.GetConnectionString("Aumentum"), timeout, Console.WriteLine);
			}

			if (_crawlProgress.HasValue())
			{
				int timeout;
				if (!int.TryParse(configuration["Db:TimeoutInSeconds"], out timeout))
				{
					timeout = OneHour;
				}

				var record = AppOperations.GetCrawlProgress(configuration.GetConnectionString("Aumentum"), timeout);
				Console.WriteLine("IndexRows " + record.IndexRows);
				Console.WriteLine("TotalRows " + record.TotalRows);
				Console.WriteLine($"% Completed {GetCrawlPercentage(record)}");
			}

			return 0;
		}

		private static string GetCrawlPercentage(CrawlProgress record)
		{
			return record.TotalRows > 0
				? Math.Round((record.IndexRows / (decimal)record.TotalRows) * 100M, 2)
					.ToString(CultureInfo.InvariantCulture) + "%"
				: "N/A";
		}
	}
}

