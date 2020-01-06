using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TAGov.Common.Http
{
    public static class Extensions
    {
		public static void AddHttpJwtAccessor(this IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<ISecurityTokenServiceProxy, SecurityTokenServiceProxy>();
		}
	}
}
