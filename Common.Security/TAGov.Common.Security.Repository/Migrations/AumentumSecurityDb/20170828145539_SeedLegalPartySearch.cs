using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
	public partial class SeedLegalPartySearch : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var permissionSeeder = new PermissionSeeder(db);

				int legalPartySearchServiceId = permissionSeeder.NextId();

				var legalPartySearchServiceApplication = permissionSeeder.CreateApplication(
					legalPartySearchServiceId, AumentumSecurityObjectModel.LegalPartySearchSecurityObjectModel.Name);

				db.Permissions.Add(legalPartySearchServiceApplication);

				db.Permissions.AddRange(permissionSeeder.CreateFields(legalPartySearchServiceId, legalPartySearchServiceApplication.Name, AumentumSecurityObjectModel.LegalPartySearchSecurityObjectModel.Resources.LegalPartySearch));

				db.SaveChanges();
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var permissions = db.Permissions.Where(x => x.AppFunctionType == "field" &&
															x.ParentName == "api.LegalPartySearch" &&
															x.TransactionId == 0 &&
															x.IsMenuItem == 0 &&
															x.ParentId > 0).ToList();
				db.Permissions.RemoveRange(permissions);

				var applications = db.Permissions.Where(x => x.AppFunctionType == "Application" &&
															 x.Name == "api.LegalPartySearch" &&
															 x.TransactionId == 0 &&
															 x.IsMenuItem == 0).ToList();

				db.RemoveRange(applications);
				db.SaveChanges();
			}
		}
	}
}
