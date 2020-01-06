using System;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using TAGov.Common.Operations;

namespace TAGov.Services.Facade.BaseValueSegment.Operations
{
  public class Operations : IOperations
  {
    private const string NoMigrationMessage = "No migrations are defined for this application.";

    public IEnumerable<string> GetEfMigrations( IConfiguration configuration )
    {
      return new[] { NoMigrationMessage };
    }

    public void ApplyEfMigrations( IConfiguration configuration )
    {
      Console.WriteLine(NoMigrationMessage);
    }

    public int Apply(IConfiguration configuration)
    {
      return 0;
    }

    public void AddOption( CommandLineApplication commandLineApplication )
    {
      // Do nothing.
    }
  }
}