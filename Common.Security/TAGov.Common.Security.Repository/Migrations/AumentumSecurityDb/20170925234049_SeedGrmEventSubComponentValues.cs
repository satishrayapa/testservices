using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
  public partial class SeedGrmEventSubComponentValues : Migration
  {
    private const string ApiGrmEventServiceName = "api.GrmEventService";

    protected override void Up( MigrationBuilder migrationBuilder )
    {
      using ( var db = new AumentumSecurityContext() )
      {
        var permissionSeeder = new PermissionSeeder( db );

        var grmEventServiceApplication = db.Permissions.Single( p => p.Name == ApiGrmEventServiceName &&
                                                                     p.AppFunctionType == "Application" &&
                                                                     p.App == ApiGrmEventServiceName );

        db.Permissions.AddRange( permissionSeeder.CreateFields( grmEventServiceApplication.Id, grmEventServiceApplication.Name,
                                                                AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.SubComponentValues ) );
        db.SaveChanges();
      }

    }

    protected override void Down( MigrationBuilder migrationBuilder )
    {
      using ( var db = new AumentumSecurityContext() )
      {
        var permission = db.Permissions.Single( x => x.AppFunctionType == "field" &&
                                                     x.ParentName == ApiGrmEventServiceName &&
                                                     x.Name == PermissionSeeder.ToResourceName( AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.SubComponentValues ) );
        db.Permissions.Remove( permission );

        db.SaveChanges();
      }
    }
  }
}