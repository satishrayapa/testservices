using System.Reflection;
using TAGov.Common.Security.Repository;

namespace TAGov.Common.Security.Domain.Implementation
{
	public static class MigrationsAssemblyReference
	{
		public static string Name => typeof(ProxyPersistedGrantDbContext).GetTypeInfo().Assembly.GetName().Name;
	}
}
