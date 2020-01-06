using System;
using Microsoft.Extensions.Caching.Memory;

namespace TAGov.Common.Caching
{
	public class CacheHelper : ICacheHelper
	{
		private readonly IMemoryCache _memoryCache;

		public CacheHelper(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			return _memoryCache.TryGetValue(key, out value);
		}

		public void Set<T>(string key, TimeSpan timeSpan, T value)
		{
			// Set cache options.
			var cacheEntryOptions = new MemoryCacheEntryOptions()
				// Keep in cache for this time, reset time if accessed.
				.SetSlidingExpiration(timeSpan);

			// Save data in cache.
			_memoryCache.Set(key, value, cacheEntryOptions);
		}
	}
}
