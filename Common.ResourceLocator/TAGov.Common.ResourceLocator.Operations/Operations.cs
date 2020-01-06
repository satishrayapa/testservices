using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TAGov.Common.Operations;
using TAGov.Common.ResourceLocator.Repository;

namespace TAGov.Common.ResourceLocator.Operations
{
  public class Operations : IOperations
  {
	private const string NoMigrationMessage = "No migrations are defined for this application.";

	public IEnumerable<string> GetEfMigrations( IConfiguration configuration )
	{
		return AppMigrations.GetPendingMigrations(configuration.GetConnectionString("Resource"));
	}

	public void ApplyEfMigrations( IConfiguration configuration )
	{
		AppMigrations.Apply(configuration.GetConnectionString("Resource"));
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
