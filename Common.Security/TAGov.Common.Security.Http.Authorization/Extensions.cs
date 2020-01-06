using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TAGov.Common.Security.Http.Authorization
{
	public static class Extensions
	{
		public static void AddSharedAuthorizationHandler(this IServiceCollection services, IConfiguration configuration, string apiName)
		{
			services.AddSingleton<IAuthorizationHandler, HasResourceVerbClaim>();

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Constants.HasResourceVerb, policy => policy.Requirements.Add(new ResourceAccessRequirement(configuration["Security:ApplicationName"])));
			});

			services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme).AddIdentityServerAuthentication(o =>
			{
				o.Authority = configuration["Security:Authority"];
				o.RequireHttpsMetadata = Convert.ToBoolean(configuration["Security:RequireHttpsMetadata"]);
				o.ApiName = apiName;
			});
		}
	}
}
