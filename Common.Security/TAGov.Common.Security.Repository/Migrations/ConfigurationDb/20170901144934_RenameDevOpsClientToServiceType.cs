using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class RenameDevOpsClientToServiceType : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.RenameClientId("devops", "service.tagov.devops");
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.RenameClientId("service.tagov.devops", "devops");
			}
		}
	}
}
