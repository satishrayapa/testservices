using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TAGov.Common.ResourceLocator.Repository
{
	public class AppMigrations
	{
		public static void Apply(string aumentumConnectionString)
		{
			var aumentumSecurityContextOptions = new DbContextOptionsBuilder<ResourceContext>();
			aumentumSecurityContextOptions.UseSqlServer(aumentumConnectionString);
			using (var db = new ResourceContext(aumentumSecurityContextOptions))
			{
				//workaround to make sure migrations use the connection string specified in appsettings.json, if specified
				Environment.SetEnvironmentVariable( ResourceContext.CommonResourceLocatorConnectionStringEnvironmentVariable, aumentumConnectionString );
				db.Database.Migrate();
			}
		}

		public static IList<string> GetPendingMigrations(string aumentumConnectionString)
		{

			var aumentumSecurityContextOptions = new DbContextOptionsBuilder<ResourceContext>();
			aumentumSecurityContextOptions.UseSqlServer(aumentumConnectionString);
			using (var db = new ResourceContext(aumentumSecurityContextOptions))
			{
				//workaround to make sure migrations use the connection string specified in appsettings.json, if specified
				Environment.SetEnvironmentVariable(ResourceContext.CommonResourceLocatorConnectionStringEnvironmentVariable, aumentumConnectionString);

				var pendingMigrations = db.Database.GetPendingMigrations();
				var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

				return migrations;
			}
		}
	}
}
