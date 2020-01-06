using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class SeedInitialApiScopeClaims : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Name,
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalParty,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Name,
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalPartyRole,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Name,
					AumentumSecurityObjectModel.LegalPartySecurityObjectModel.Resources.LegalPartyDocument,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEventRevision,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEventAttributeValue,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.AssessmentEvent,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.AssessmentEventSecurityObjectModel.Resources.StatutoryReference,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Name,
					AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Resources.RevenueObject,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Name,
					AumentumSecurityObjectModel.RevenueObjectSecurityObjectModel.Resources.TaxAuthorityGroup,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.GRMEvent,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Name,
					AumentumSecurityObjectModel.GRMEventSecurityObjectModel.Resources.GRMEventInformation,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.ResourceLocatorSecurityObjectModel.Name,
					AumentumSecurityObjectModel.ResourceLocatorSecurityObjectModel.Resources.Resource,
					ServiceTypes.Common));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegment,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentEvent,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.CaliforniaConsumerPriceIndex,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.SubComponent,
					ServiceTypes.Service));
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[ApiScopeClaims]");

				db.SaveChanges();
			}
		}
	}
}
