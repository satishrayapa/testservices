using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
	public partial class SeedMyWorkListSearchRebuildAll : Migration
    {
	    private const string MyWorkListSearchRebuildAllName = "MyWorkListSearchRebuildAll";
		protected override void Up(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permissionSeeder = new PermissionSeeder(db);

		        Models.AppFunction parent = db.Permissions.Single(p => p.AppFunctionType == "Application" &&
		                                                               p.App == "api.MyWorkListSearchService");

		        db.Permissions.AddRange(permissionSeeder.CreateFields(parent.Id, parent.Name, MyWorkListSearchRebuildAllName));

		        db.SaveChanges();
	        }
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        Models.AppFunction permission = db.Permissions.Single(p => p.AppFunctionType == "field" &&
		                                                                   p.Name == MyWorkListSearchRebuildAllName);
		        db.Permissions.Remove(permission);

		        db.SaveChanges();
	        }
		}
    }
}
