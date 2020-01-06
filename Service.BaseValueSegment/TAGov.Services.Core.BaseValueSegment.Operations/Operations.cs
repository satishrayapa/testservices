using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using TAGov.Common.Operations;
using TAGov.Services.Core.BaseValueSegment.Repository;

namespace TAGov.Services.Core.BaseValueSegment.Operations
{
  public class Operations : IOperations
  {
    private const int OneHour = 3600;

    public IEnumerable<string> GetEfMigrations( IConfiguration configuration )
    {
      return AppMigrations.GetPendingMigrations(configuration.GetConnectionString("Aumentum"));

    }

    public void ApplyEfMigrations( IConfiguration configuration )
    {
      int timeout;
      if (!int.TryParse(configuration["Db:TimeoutInSeconds"], out timeout))
      {
        timeout = OneHour;
      }

      AppMigrations.Apply(configuration.GetConnectionString("Aumentum"), timeout);
    }

    public int Apply( IConfiguration configuration )
    {
      return 0;
    }

    public void AddOption(CommandLineApplication commandLineApplication)
    {
      // Do nothing.
    }
  }
}
