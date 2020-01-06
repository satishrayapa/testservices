using System.Linq;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class FixDevOpsClientGrantType : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.UpdateClientGrantType("service.tagov.devops", GrantType.ClientCredentials);
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.UpdateClientGrantType("service.tagov.devops", GrantType.ResourceOwnerPassword);
			}
		}
	}
}
