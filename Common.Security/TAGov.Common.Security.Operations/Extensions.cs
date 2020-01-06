using Microsoft.Extensions.Configuration;

namespace TAGov.Common.Security.Operations
{
	public static class Extensions
	{
		public static string AumentumConnectionString(this IConfiguration configuration)
		{
			return configuration.GetConnectionString("Permissions");
		}

		public static string IdentityConnectionString(this IConfiguration configuration)
		{
			return configuration.GetConnectionString("DefaultConnection");
		}
	}
}
