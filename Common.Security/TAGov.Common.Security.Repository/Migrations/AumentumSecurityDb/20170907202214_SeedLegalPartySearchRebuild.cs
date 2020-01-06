using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
    public partial class SeedLegalPartySearchRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permissionSeeder = new PermissionSeeder(db);
		        
		        var parent = db.Permissions.Single(p => p.AppFunctionType == "Application" &&
		                                                p.App == "api.LegalPartySearchService");

		        db.Permissions.AddRange(permissionSeeder.CreateFields(parent.Id, parent.Name, "LegalPartySearchRebuild"));

		        db.SaveChanges();
	        }
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permission = db.Permissions.Single(p => p.AppFunctionType == "field" &&
		                                                p.Name == "LegalPartySearchRebuild");
				db.Permissions.Remove(permission);

		        db.SaveChanges();
	        }
		}
    }
}
