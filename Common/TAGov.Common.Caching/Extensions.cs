using Microsoft.Extensions.DependencyInjection;

namespace TAGov.Common.Caching
{
	public static class Extensions
	{
		public static IServiceCollection AddSharedCaching(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ICacheHelper, CacheHelper>();
			return serviceCollection;
		}
	}
}
