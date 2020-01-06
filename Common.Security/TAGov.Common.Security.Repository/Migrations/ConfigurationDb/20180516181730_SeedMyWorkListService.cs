using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
    public partial class SeedMyWorkListService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        using (var db = new ProxyConfigurationDbContext())
	        {
		        db.ApiResources.AddRange(new ApiResource[]
		        {
			        new ApiResource
			        {
				        Name = ApiServices.Service.MyWorkListSearch,
				        Description ="API to search for legal parties.",
				        Scopes = new List<ApiScope> {new ApiScope {Name = ApiServices.Service.MyWorkListSearch } }
			        },
		        });

		        db.SaveChanges();

		        db.AddClientScope("aumentum.web", ApiServices.Service.MyWorkListSearch);

		        db.AddClientScope("service.tagov.search", ApiServices.Service.MyWorkListSearch);

		        db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
			        AumentumSecurityObjectModel.MyWorkListSearchSecurityObjectModel.Name,
			        AumentumSecurityObjectModel.MyWorkListSearchSecurityObjectModel.Resources.MyWorkListSearch,
			        ServiceTypes.Service));

		        db.SaveChanges();
	        }
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        using (var db = new ProxyConfigurationDbContext())
	        {
		        db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[ClientScopes] WHERE [Scope] ='api.service.myworklistsearch';");

		        db.Database.ExecuteSqlCommand($"DELETE FROM [dbo].[ApiScopeClaims] WHERE [Type] LIKE '%api.myworklistsearch';");

		        var apiRes = db.ApiResources.Single(x => x.Name == ApiServices.Service.MyWorkListSearch);

		        db.ApiResources.Remove(apiRes);

		        db.SaveChanges();
	        }
		}
    }
}
