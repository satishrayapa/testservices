using System.Linq;
using TAGov.Common.Security.Repository.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.migrations.AumentumSecurityDb
{
	public partial class SeedLegalPartySearchOperationsStopwords : Migration
	{
		private const string OperationsSystemStopwords = "OperationsSystemStopwords";

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var permissionSeeder = new PermissionSeeder(db);

				Models.AppFunction parent = db.Permissions.Single(p => p.AppFunctionType == "Application" &&
																	   p.App == "api.LegalPartySearchService");

				db.Permissions.AddRange(permissionSeeder.CreateFields(parent.Id, parent.Name, OperationsSystemStopwords));

				db.SaveChanges();
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				Models.AppFunction permission = db.Permissions.Single(p => p.AppFunctionType == "field" &&
																		   p.Name == OperationsSystemStopwords);
				db.Permissions.Remove(permission);

				db.SaveChanges();
			}
		}
	}
}
