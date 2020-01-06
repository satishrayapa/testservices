using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
  public partial class SeedGrmEventServiceSubComponentValues : Migration
  {
    protected override void Up( MigrationBuilder migrationBuilder )
    {
      using ( var db = new ProxyConfigurationDbContext() )
      {

        db.Database.ExecuteSqlCommand( IdentityHelper.InsertSql(
                                         AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Name,
                                         AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.SubComponentValues,
                                         ServiceTypes.Service ) );
      }
    }

    protected override void Down( MigrationBuilder migrationBuilder )
    {
      using ( var db = new ProxyConfigurationDbContext() )
      {
        db.Database.ExecuteSqlCommand( IdentityHelper.DeleteSql(
                                         AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Name,
                                         AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.SubComponentValues,
                                         ServiceTypes.Service ) );
      }
    }
  }
}