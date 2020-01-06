using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TAGov.Services.Core.BaseValueSegment.Repository
{
  public static class AppMigrations
  {
    public static void Apply( string baseValueSegmentConnectionString, int timeoutInSeconds )
    {
      var aumentumSecurityContextOptions = new DbContextOptionsBuilder<BaseValueSegmentContext>();
      aumentumSecurityContextOptions.UseSqlServer( baseValueSegmentConnectionString );
      using ( var db = new BaseValueSegmentContext( aumentumSecurityContextOptions ) )
      {
        if ( timeoutInSeconds > 0 )
          db.Database.SetCommandTimeout( timeoutInSeconds );

        //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
        Environment.SetEnvironmentVariable( BaseValueSegmentContext.BaseValueSegmentConnectionStringEnvironmentVariable, baseValueSegmentConnectionString );

        db.Database.Migrate();
      }
    }

    public static IList<string> GetPendingMigrations( string baseValueSegmentConnectionString )
    {
      var aumentumSecurityContextOptions = new DbContextOptionsBuilder<BaseValueSegmentContext>();
      aumentumSecurityContextOptions.UseSqlServer( baseValueSegmentConnectionString );
      using ( var db = new BaseValueSegmentContext( aumentumSecurityContextOptions ) )
      {
        //workaround to make sure migrations use the connection string specified in appsettings.json, if specified
        Environment.SetEnvironmentVariable( BaseValueSegmentContext.BaseValueSegmentConnectionStringEnvironmentVariable, baseValueSegmentConnectionString );

        var pendingMigrations = db.Database.GetPendingMigrations();
        var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();

        return migrations;
      }
    }
  }
}
