using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
	public partial class SeedAumentumSecurity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var permissionSeeder = new PermissionSeeder(db);

				int legalPartyServiceId = permissionSeeder.NextId();
				int revenueObjectServiceId = permissionSeeder.NextId();
				int assessmentEventServiceId = permissionSeeder.NextId();
				int grmEventServiceId = permissionSeeder.NextId();
				int baseValueSegementServiceId = permissionSeeder.NextId();
				int resourceLocatorServiceId = permissionSeeder.NextId();

				var legalPartyServiceApplication = permissionSeeder.CreateApplication(legalPartyServiceId, AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Name);
				var revenueObjectServiceApplication = permissionSeeder.CreateApplication(revenueObjectServiceId, AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Name);
				var assessmentEventServiceApplication = permissionSeeder.CreateApplication(assessmentEventServiceId, AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Name);
				var grmEventServiceApplication = permissionSeeder.CreateApplication(grmEventServiceId, AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Name);
				var baseValueSegmentServiceApplication = permissionSeeder.CreateApplication(baseValueSegementServiceId, AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name);
				var resourceLocatorServiceApplication = permissionSeeder.CreateApplication(resourceLocatorServiceId, AumentumSecurityObjectModel.ResourceLocatorSecurityObjectModel.Name);

				// For applications
				db.Permissions.AddRange(
					legalPartyServiceApplication,
					revenueObjectServiceApplication,
					assessmentEventServiceApplication,
					grmEventServiceApplication,
					baseValueSegmentServiceApplication,
					resourceLocatorServiceApplication);

				db.Permissions.AddRange(permissionSeeder.CreateFields(legalPartyServiceId, legalPartyServiceApplication.Name, AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalParty));
				db.Permissions.AddRange(permissionSeeder.CreateFields(legalPartyServiceId, legalPartyServiceApplication.Name, AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalPartyRole));
				db.Permissions.AddRange(permissionSeeder.CreateFields(legalPartyServiceId, legalPartyServiceApplication.Name, AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalPartyDocument));
				db.Permissions.AddRange(permissionSeeder.CreateFields(revenueObjectServiceId, revenueObjectServiceApplication.Name, AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Resources.RevenueObject));
				db.Permissions.AddRange(permissionSeeder.CreateFields(revenueObjectServiceId, revenueObjectServiceApplication.Name, AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Resources.TaxAuthorityGroup));
				db.Permissions.AddRange(permissionSeeder.CreateFields(assessmentEventServiceId, assessmentEventServiceApplication.Name, AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEvent));
				db.Permissions.AddRange(permissionSeeder.CreateFields(assessmentEventServiceId, assessmentEventServiceApplication.Name, AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEventAttributeValue));
				db.Permissions.AddRange(permissionSeeder.CreateFields(assessmentEventServiceId, assessmentEventServiceApplication.Name, AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEventRevision));
				db.Permissions.AddRange(permissionSeeder.CreateFields(assessmentEventServiceId, assessmentEventServiceApplication.Name, AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.StatutoryReference));
				db.Permissions.AddRange(permissionSeeder.CreateFields(grmEventServiceId, grmEventServiceApplication.Name, AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.GRMEvent));
				db.Permissions.AddRange(permissionSeeder.CreateFields(grmEventServiceId, grmEventServiceApplication.Name, AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.GRMEventInformation));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegementServiceId, baseValueSegmentServiceApplication.Name, AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegment));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegementServiceId, baseValueSegmentServiceApplication.Name, AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentEvent));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegementServiceId, baseValueSegmentServiceApplication.Name, AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.CaliforniaConsumerPriceIndex));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegementServiceId, baseValueSegmentServiceApplication.Name, AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.SubComponent));
				db.Permissions.AddRange(permissionSeeder.CreateFields(resourceLocatorServiceId, resourceLocatorServiceApplication.Name, AumentumSecurityObjectModel.ResourceLocatorSecurityObjectModel.Resources.Resource));

				db.SaveChanges();
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var permissions = db.Permissions.Where(x => x.AppFunctionType == "field" &&
															x.ParentName.StartsWith("api.") &&
															x.TransactionId == 0 &&
															x.IsMenuItem == 0 &&
															x.ParentId > 0).ToList();
				db.Permissions.RemoveRange(permissions);

				var applications = db.Permissions.Where(x => x.AppFunctionType == "Application" &&
															 x.Name.StartsWith("api.") &&
															 x.TransactionId == 0 &&
															 x.IsMenuItem == 0).ToList();

				db.RemoveRange(applications);
				db.SaveChanges();
			}
		}
	}
}
