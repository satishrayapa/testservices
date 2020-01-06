using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class SeedSearchService : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.AddClients(new Client[]
				{
					new Client
					{
						ClientId = "service.tagov.search",
						AllowedGrantTypes = new List<ClientGrantType>()
						{
							new ClientGrantType{ GrantType = GrantType.ClientCredentials}
						}, ClientSecrets = new List<ClientSecret>()
						{
							new ClientSecret{ Value = "password".Sha256() }
						}, AllowedScopes = new List<ClientScope>()
						{
							new ClientScope { Scope = ApiServices.Common.ResourceLocator },
							new ClientScope { Scope = ApiServices.Facade.AssessmentHeader },
							new ClientScope { Scope = ApiServices.Facade.BaseValueSegment },
							new ClientScope { Scope = ApiServices.Service.GrmEvent },
							new ClientScope { Scope = ApiServices.Service.BaseValueSegment },
							new ClientScope { Scope = ApiServices.Service.AssessmentEvent },
							new ClientScope { Scope = ApiServices.Service.LegalParty },
							new ClientScope { Scope = ApiServices.Service.RevenueObject }
						}
					}
				});
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.RemoveClient("service.tagov.search");
			}
		}
	}
}
