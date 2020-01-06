using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
  public partial class SeedLegalPartySearchRebuildAll : Migration
  {
    private const string LegalPartySearchRebuildAllName = "LegalPartySearchRebuildAll";

    protected override void Up( MigrationBuilder migrationBuilder )
    {
      using ( var db = new AumentumSecurityContext() )
      {
        var permissionSeeder = new PermissionSeeder( db );

        Models.AppFunction parent = db.Permissions.Single( p => p.AppFunctionType == "Application" &&
                                                                p.App == "api.LegalPartySearchService" );

        db.Permissions.AddRange( permissionSeeder.CreateFields( parent.Id, parent.Name, LegalPartySearchRebuildAllName ) );

        db.SaveChanges();
      }
    }

    protected override void Down( MigrationBuilder migrationBuilder )
    {
      using ( var db = new AumentumSecurityContext() )
      {
        Models.AppFunction permission = db.Permissions.Single( p => p.AppFunctionType == "field" &&
                                                                    p.Name == LegalPartySearchRebuildAllName );
        db.Permissions.Remove( permission );

        db.SaveChanges();
      }
    }
  }
}