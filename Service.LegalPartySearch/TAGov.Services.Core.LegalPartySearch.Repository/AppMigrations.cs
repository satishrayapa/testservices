using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TAGov.Services.Core.LegalPartySearch.Repository
{
	public static class AppMigrations
	{
		public static void Apply(string connectionString, int timeoutInSeconds)
		{
			var contextOptions = new DbContextOptionsBuilder<SearchLegalPartyContext>();
			contextOptions.UseSqlServer(connectionString);
			using (var db = new SearchLegalPartyContext(contextOptions.Options))
			{
				if (timeoutInSeconds > 0)
					db.Database.SetCommandTimeout(timeoutInSeconds);

				db.Database.Migrate();
			}
		}

		public static IList<string> GetPendingMigrations(string connectionString)
		{

			var contextOptions = new DbContextOptionsBuilder<SearchLegalPartyContext>();
			contextOptions.UseSqlServer(connectionString);
			using (var db = new SearchLegalPartyContext(contextOptions.Options))
			{
				//workaround to make sure migrations use the connection string specified in appsettings.json, if specified
				Environment.SetEnvironmentVariable(SearchLegalPartyContext.SearchLegalPartyContextConnectionStringEnvironmentVariable, connectionString);

				var pendingMigrations = db.Database.GetPendingMigrations();
				var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

				return migrations;
			}
			
		}
	}
}
