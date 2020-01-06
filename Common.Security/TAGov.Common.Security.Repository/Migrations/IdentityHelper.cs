using System;
using System.Linq;
using System.Text;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.Security.Claims;

namespace TAGov.Common.Security.Repository.Migrations
{
	public enum ServiceTypes
	{
		Service,
		Common
	}
	public static class IdentityHelper
	{
		/// <summary>
		/// Ability to add new client prior to v2 of Identity EF. 
		/// </summary>
		/// <param name="db">ProxyConfigurationDbContext</param>
		/// <param name="clients">Clients</param>
		/// <remarks>There's a breaking change from v1 to v2 of Identity EF in terms of the clients table schema. Thus, prior to v2, this is how we should be adding new client(s). Do NOT continue to use this!</remarks>
		public static void AddClients(this ProxyConfigurationDbContext db, Client[] clients)
		{
			var sb = new StringBuilder();

			clients.ToList().ForEach(client =>
			{
				var uniqueclientId = "@clientId_" + Guid.NewGuid().ToString("N");
				var values = string.Format(ClientValues, client.ClientId);
				sb.AppendLine($"DECLARE {uniqueclientId} INT; INSERT INTO [dbo].[Clients]({ClientColumns}) VALUES ({values}); SET {uniqueclientId}=SCOPE_IDENTITY()");

				client.AllowedGrantTypes.ForEach(grantType =>
				{
					sb.AppendLine($"INSERT INTO [dbo].[ClientGrantTypes]([ClientId],[GrantType]) VALUES ({uniqueclientId},'{grantType.GrantType}')");
				});

				client.ClientSecrets.ForEach(secret =>
				{
					sb.AppendLine($"INSERT INTO [dbo].[ClientSecrets]([ClientId],[Type],[Value]) VALUES ({uniqueclientId},'SharedSecret','{secret.Value}')");
				});

				client.AllowedScopes.ForEach(allowedScope =>
				{
					sb.AppendLine($"INSERT INTO [dbo].[ClientScopes]([ClientId],[Scope]) VALUES ({uniqueclientId},'{allowedScope.Scope}')");
				});
			});

			db.Database.ExecuteSqlCommand(sb.ToString());
		}

		/// <summary>
		/// Ability to remove an existing client prior to v2 of Identity EF. 
		/// </summary>
		/// <param name="db"></param>
		/// <param name="clientId"></param>
		/// <remarks>There's a breaking change from v1 to v2 of Identity EF in terms of the clients table schema. Thus, prior to v2, this is how we should be adding new client(s). Do NOT continue to use this!</remarks>
		public static void RemoveClient(this ProxyConfigurationDbContext db, string clientId)
		{
			var sb = new StringBuilder();

			var uniqueclientId = "@clientId_" + Guid.NewGuid().ToString("N");
			sb.AppendLine($"DECLARE {uniqueclientId} INT; SET {uniqueclientId} = (SELECT Id from [dbo].[Clients] WHERE ClientId='{clientId}');");
			sb.AppendLine($"DELETE [dbo].[ClientGrantTypes] WHERE [ClientId]={uniqueclientId}");
			sb.AppendLine($"DELETE [dbo].[ClientSecrets] WHERE [ClientId]={uniqueclientId}");
			sb.AppendLine($"DELETE [dbo].[ClientScopes] WHERE [ClientId]={uniqueclientId}");
			sb.AppendLine($"DELETE [dbo].[Clients] WHERE [Id]={uniqueclientId}");

			db.Database.ExecuteSqlCommand(sb.ToString());
		}

		/// <summary>
		/// Adds scope to client.
		/// </summary>
		/// <param name="db">ProxyConfigurationDbContext</param>
		/// <param name="clientId">clientId</param>
		/// <param name="scope">scope</param>
		/// <remarks>There's a breaking change from v1 to v2 of Identity EF in terms of the clients table schema. Thus, prior to v2, this is how we should be adding new client(s). Do NOT continue to use this!</remarks>
		public static void AddClientScope(this ProxyConfigurationDbContext db, string clientId, string scope)
		{
			var uniqueclientId = "@clientId_" + Guid.NewGuid().ToString("N");
			var sql =
				$"DECLARE {uniqueclientId} INT; SET {uniqueclientId} = (SELECT Id from [dbo].[Clients] WHERE ClientId='{clientId}'); INSERT INTO [dbo].[ClientScopes]([ClientId],[Scope]) VALUES ({uniqueclientId},'{scope}')";
			db.Database.ExecuteSqlCommand(sql);
		}

		/// <summary>
		/// Renames an existing client Id.
		/// </summary>
		/// <param name="db">ProxyConfigurationDbContext</param>
		/// <param name="oldName">oldName</param>
		/// <param name="newName">newName</param>
		/// <remarks>There's a breaking change from v1 to v2 of Identity EF in terms of the clients table schema. Thus, prior to v2, this is how we should be adding new client(s). Do NOT continue to use this!</remarks>
		public static void RenameClientId(this ProxyConfigurationDbContext db, string oldName, string newName)
		{
			var sql = $"UPDATE [dbo].[Clients] SET ClientId='{newName}' WHERE ClientId='{oldName}'";
			db.Database.ExecuteSqlCommand(sql);
		}

		/// <summary>
		/// Updates an existing client's grant-type.
		/// </summary>
		/// <param name="db">ProxyConfigurationDbContext.</param>
		/// <param name="clientId">clientId.</param>
		/// <param name="grantType">grantType.</param>
		/// <remarks>There's a breaking change from v1 to v2 of Identity EF in terms of the clients table schema. Thus, prior to v2, this is how we should be adding new client(s). Do NOT continue to use this!</remarks>
		public static void UpdateClientGrantType(this ProxyConfigurationDbContext db, string clientId, string grantType)
		{
			var uniqueclientId = "@clientId_" + Guid.NewGuid().ToString("N");
			var sql =
				$"DECLARE {uniqueclientId} INT; SET {uniqueclientId} = (SELECT Id from [dbo].[Clients] WHERE ClientId='{clientId}'); UPDATE [dbo].[ClientGrantTypes] SET [GrantType]='{grantType}' WHERE ClientId={uniqueclientId}";
			db.Database.ExecuteSqlCommand(sql);
		}

		private const string ClientColumns = "[ClientId],[AbsoluteRefreshTokenLifetime],[AccessTokenLifetime],[AccessTokenType],[AllowAccessTokensViaBrowser],[AllowOfflineAccess],[AllowPlainTextPkce],[AllowRememberConsent],[AlwaysIncludeUserClaimsInIdToken],[AlwaysSendClientClaims],[AuthorizationCodeLifetime],[EnableLocalLogin],[Enabled],[IdentityTokenLifetime],[IncludeJwtId],[LogoutSessionRequired],[PrefixClientClaims],[RefreshTokenExpiration],[ProtocolType],[RefreshTokenUsage],[RequireClientSecret],[RequireConsent],[RequirePkce],[SlidingRefreshTokenLifetime],[UpdateAccessTokenClaimsOnRefresh]";
		private const string ClientValues = "'{0}'		,2592000					   ,3600				 ,0				   ,0							 ,0					  ,0				   ,1					  ,0								 ,0						  ,300						  ,1				 ,1		   ,300					   ,0			  ,0					  ,0				   ,1						,'oidc'		   ,1				   ,1					 ,1				  ,0			,1296000					  ,0";

		public static string InsertSql(string applicationName, string resource, ServiceTypes serviceType)
		{
			var apiScopeName = GetApiScopeName(applicationName, serviceType);

			var claim = ClaimsHelper.ToClaim(
				PermissionSeeder.ToApplicationName(applicationName),
				PermissionSeeder.ToResourceName(resource), false, false, false, false);

			return $"INSERT INTO [dbo].[ApiScopeClaims]([ApiScopeId],[Type]) VALUES((SELECT Id FROM [dbo].[ApiScopes] WHERE Name='{apiScopeName}'),'{claim.Type}')";
		}

		public static string DeleteSql(string applicationName, string resource, ServiceTypes serviceType)
		{
			var apiScopeName = GetApiScopeName(applicationName, serviceType);

			var claim = ClaimsHelper.ToClaim(
				PermissionSeeder.ToApplicationName(applicationName),
				PermissionSeeder.ToResourceName(resource), false, false, false, false);

			return $"DELETE FROM [dbo].[ApiScopeClaims] WHERE [ApiScopeId] = (SELECT Id FROM [dbo].[ApiScopes] WHERE Name='{apiScopeName}') AND [Type] = '{claim.Type}'";

		}

		private static string GetApiScopeName(string applicationName, ServiceTypes serviceType)
		{
			string apiScopeName;
			switch (serviceType)
			{
				case ServiceTypes.Service:
					apiScopeName = "api.service.";
					break;

				case ServiceTypes.Common:
					apiScopeName = "api.common.";
					break;

				default:
					throw new NotImplementedException(serviceType + " not implemented.");
			}
			apiScopeName = (apiScopeName + applicationName.Replace(" ", "")).ToLowerInvariant();

			return apiScopeName;
		}
	}
}
