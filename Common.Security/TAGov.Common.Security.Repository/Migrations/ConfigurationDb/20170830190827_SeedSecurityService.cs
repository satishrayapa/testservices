using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using IdentityServer4.Models;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class SeedSecurityService : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.ApiResources.AddRange(new ApiResource[]
				{
					new ApiResource
					{
						Name = ApiServices.Common.Security,
						Description ="API to manage security resources.",
						Scopes = new List<ApiScope> {new ApiScope {Name = ApiServices.Common.Security } }
					},
				});

				db.SaveChanges();

				db.AddClients(new Client[]
				{
					new Client
					{
						ClientId = "devops",
						AllowedGrantTypes = new List<ClientGrantType>()
						{
							new ClientGrantType{ GrantType = GrantType.ResourceOwnerPassword}
						}, ClientSecrets = new List<ClientSecret>()
						{
							new ClientSecret{ Value = "password".Sha256() }
						}, AllowedScopes = new List<ClientScope>()
						{
							new ClientScope { Scope = ApiServices.Common.Security },
						}
					}
				});
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.RemoveClient("devops");

				var apiResource = db.ApiResources.SingleOrDefault(x => x.Name == ApiServices.Common.Security);

				if (apiResource != null)
					db.ApiResources.Remove(apiResource);

				db.SaveChanges();
			}
		}
	}
}
