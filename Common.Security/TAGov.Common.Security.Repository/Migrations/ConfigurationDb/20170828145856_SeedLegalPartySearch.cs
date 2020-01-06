using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class SeedLegalPartySearch : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.ApiResources.AddRange(new ApiResource[]
				{
					new ApiResource
					{
						Name = ApiServices.Service.LegalPartySearch,
						Description ="API to search for legal parties.",
						Scopes = new List<ApiScope> {new ApiScope {Name = ApiServices.Service.LegalPartySearch } }
					},
				});

				db.SaveChanges();

				db.AddClientScope("aumentum.web", ApiServices.Service.LegalPartySearch);

				db.AddClientScope("service.tagov.search", ApiServices.Service.LegalPartySearch);

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.LegalPartySearchSecurityObjectModel.Name,
					AumentumSecurityObjectModel.LegalPartySearchSecurityObjectModel.Resources.LegalPartySearch,
					ServiceTypes.Service));

				db.SaveChanges();
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[ClientScopes] WHERE [Scope] ='api.service.legalpartysearch';");

				db.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[ApiScopeClaims] WHERE [Type] LIKE '%api.legalpartysearch';");

				var apiRes = db.ApiResources.Single(x => x.Name == ApiServices.Service.LegalPartySearch);

				db.ApiResources.Remove(apiRes);

				db.SaveChanges();
			}
		}
	}
}
