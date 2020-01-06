using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
    public partial class SeedMyWorkListSearchRebuild : Migration
    {
	    private const string MyWorkListSearchRebuildName = "MyWorkListSearchRebuild";
		protected override void Up(MigrationBuilder migrationBuilder)
        {
			using (var db = new AumentumSecurityContext())
			{
				var permissionSeeder = new PermissionSeeder(db);

				var parent = db.Permissions.Single(p => p.AppFunctionType == "Application" &&
				                                        p.App == "api.MyWorkListSearchService");
				db.Permissions.AddRange(permissionSeeder.CreateFields(parent.Id, parent.Name, MyWorkListSearchRebuildName));

				db.SaveChanges();
			}
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permission = db.Permissions.Single(p => p.AppFunctionType == "field" &&
		                                                    p.Name == MyWorkListSearchRebuildName);
		        db.Permissions.Remove(permission);

		        db.SaveChanges();
	        }
		}
    }
}
