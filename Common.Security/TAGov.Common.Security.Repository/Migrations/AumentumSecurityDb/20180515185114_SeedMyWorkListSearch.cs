using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
    public partial class SeedMyWorkListSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permissionSeeder = new PermissionSeeder(db);

		        int legalPartySearchServiceId = permissionSeeder.NextId();

		        var legalPartySearchServiceApplication = permissionSeeder.CreateApplication(
			        legalPartySearchServiceId, AumentumSecurityObjectModel.MyWorkListSearchSecurityObjectModel.Name);

		        db.Permissions.Add(legalPartySearchServiceApplication);

		        db.Permissions.AddRange(permissionSeeder.CreateFields(legalPartySearchServiceId, legalPartySearchServiceApplication.Name, AumentumSecurityObjectModel.MyWorkListSearchSecurityObjectModel.Resources.MyWorkListSearch));

		        db.SaveChanges();
	        }
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        using (var db = new AumentumSecurityContext())
	        {
		        var permissions = db.Permissions.Where(x => x.AppFunctionType == "field" &&
		                                                    x.ParentName == "api.MyWorkListSearch" &&
		                                                    x.TransactionId == 0 &&
		                                                    x.IsMenuItem == 0 &&
		                                                    x.ParentId > 0).ToList();
		        db.Permissions.RemoveRange(permissions);

		        var applications = db.Permissions.Where(x => x.AppFunctionType == "Application" &&
		                                                     x.Name == "api.MyWorkListSearch" &&
		                                                     x.TransactionId == 0 &&
		                                                     x.IsMenuItem == 0).ToList();

		        db.RemoveRange(applications);
		        db.SaveChanges();
	        }
		}
    }
}
