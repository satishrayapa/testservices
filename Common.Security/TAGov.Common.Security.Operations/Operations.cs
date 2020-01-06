using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using TAGov.Common.Operations;
using TAGov.Common.Security.Repository;

namespace TAGov.Common.Security.Operations
{
	public class Operations : IOperations
	{
		public IEnumerable<string> GetEfMigrations( IConfiguration configuration )
		{
			var list = AppMigrations.GetConfigurationDbPendingMigrations(configuration.IdentityConnectionString()).ToList();
			list.AddRange(AppMigrations.GetPersistedGrantDbPendingMigrations(configuration.IdentityConnectionString()));
			list.AddRange(AppMigrations.GetAumentumSecurityPendingMigrations(configuration.AumentumConnectionString()));
			return list;
		}

		public void ApplyEfMigrations( IConfiguration configuration )
		{
			AppMigrations.Apply(
				configuration.AumentumConnectionString(),
				configuration.IdentityConnectionString());
		}

    public int Apply( IConfiguration configuration )
    {
        return 0;
    }

    public void AddOption( CommandLineApplication commandLineApplication )
    {
        // Do nothing.
    }
  }
}
