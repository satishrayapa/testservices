using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
	public partial class CreateBaseline : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ProxyConfigurationDbContext())
			{
				db.ApiResources.AddRange(new ApiResource[]
				{
					new ApiResource{Name = ApiServices.Common.ResourceLocator, Description = "Resource Locator is a common or shared service used to provide resource information such as service endpoints to consumer.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Common.ResourceLocator } } },
					new ApiResource{Name = ApiServices.Facade.AssessmentHeader, Description = "Assessment Header is a facade service used to provide assessment header information to base value segement header panel.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Facade.AssessmentHeader } }  },
					new ApiResource{Name = ApiServices.Facade.BaseValueSegment, Description = "Base value segment is a facade service used to provide assessment header information to base value segement grids.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Facade.BaseValueSegment } }  },
					new ApiResource{Name = ApiServices.Service.GrmEvent, Description = "GRM event service.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Service.GrmEvent } }  },
					new ApiResource{Name = ApiServices.Service.BaseValueSegment, Description = "Base value segment service.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Service.BaseValueSegment } }  },
					new ApiResource{Name = ApiServices.Service.AssessmentEvent, Description = "Assessment event service.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Service.AssessmentEvent } }  },
					new ApiResource{Name = ApiServices.Service.LegalParty, Description = "Legal party service.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Service.LegalParty } }  },
					new ApiResource{Name = ApiServices.Service.RevenueObject, Description = "Revenue object service.",
						Scopes = new List<ApiScope>{ new ApiScope{ Name = ApiServices.Service.RevenueObject } }  }
				});

				db.SaveChanges();

				db.AddClients(new Client[]
				{
					new Client
					{
						ClientId = "aumentum.web",
						AllowedGrantTypes = new List<ClientGrantType>()
						{
							new ClientGrantType{ GrantType = GrantType.ResourceOwnerPassword}
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
				db.RemoveClient("aumentum.web");

				db.RemoveRange(db.ApiResources);

				db.SaveChanges();
			}
		}
	}
}
