using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class SeedBaseValueSegmentFlags : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Flags,
					ServiceTypes.Service));
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.Database.ExecuteSqlCommand(IdentityHelper.DeleteSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Flags,
					ServiceTypes.Service));
			}
		}
	}
}
