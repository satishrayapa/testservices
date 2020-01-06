using System;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TAGov.Common.Security.Repository
{
	public static class AppMigrations
	{
		public static void Apply(string aumentumConnectionString, string identityConnectionString)
		{
			var aumentumSecurityContextOptions = new DbContextOptionsBuilder<AumentumSecurityContext>();
			aumentumSecurityContextOptions.UseSqlServer(aumentumConnectionString);
            using (var db = new AumentumSecurityContext(aumentumSecurityContextOptions))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                Environment.SetEnvironmentVariable(AumentumSecurityContext.AumentumSecurityConnectionStringEnvironmentVariable, aumentumConnectionString);
                db.Database.Migrate();
            }

			var proxyConfigurationDbContextOptions = new DbContextOptionsBuilder<ConfigurationDbContext>();
			proxyConfigurationDbContextOptions.UseSqlServer(identityConnectionString);
            using (var db = new ProxyConfigurationDbContext(proxyConfigurationDbContextOptions))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                Environment.SetEnvironmentVariable(ProxyConfigurationDbContext.CommonSecurityConnectionStringEnvironmentVariable, identityConnectionString);
                db.Database.Migrate();
            }

			var proxyPersistedGrantDbContext = new DbContextOptionsBuilder<PersistedGrantDbContext>();
			proxyPersistedGrantDbContext.UseSqlServer(identityConnectionString);
            using (var db = new ProxyPersistedGrantDbContext(proxyPersistedGrantDbContext))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                //same environment variable and connection string as above but setting again just in case...
                Environment.SetEnvironmentVariable(ProxyConfigurationDbContext.CommonSecurityConnectionStringEnvironmentVariable, identityConnectionString);
                db.Database.Migrate();
            }
		}

        public static IList<string> GetAumentumSecurityPendingMigrations(string connectionString)
        {

            var contextOptions = new DbContextOptionsBuilder<AumentumSecurityContext>();
            contextOptions.UseSqlServer(connectionString);
            using (var db = new AumentumSecurityContext(contextOptions))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                Environment.SetEnvironmentVariable(AumentumSecurityContext.AumentumSecurityConnectionStringEnvironmentVariable, connectionString);

                var pendingMigrations = db.Database.GetPendingMigrations();
                var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

                return migrations;
            }

        }

        public static IList<string> GetConfigurationDbPendingMigrations(string connectionString)
        {

            var contextOptions = new DbContextOptionsBuilder<ConfigurationDbContext>();
            contextOptions.UseSqlServer(connectionString);
            using (var db = new ProxyConfigurationDbContext(contextOptions))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                Environment.SetEnvironmentVariable(ProxyConfigurationDbContext.CommonSecurityConnectionStringEnvironmentVariable, connectionString);

                var pendingMigrations = db.Database.GetPendingMigrations();
                var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

                return migrations;
            }

        }

        public static IList<string> GetPersistedGrantDbPendingMigrations(string connectionString)
        {

            var contextOptions = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            contextOptions.UseSqlServer(connectionString);
            using (var db = new ProxyPersistedGrantDbContext(contextOptions))
            {
                //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
                Environment.SetEnvironmentVariable(ProxyConfigurationDbContext.CommonSecurityConnectionStringEnvironmentVariable, connectionString);

                var pendingMigrations = db.Database.GetPendingMigrations();
                var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

                return migrations;
            }

        }

    }
}
