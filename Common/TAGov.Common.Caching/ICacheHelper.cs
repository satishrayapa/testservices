using System;

namespace TAGov.Common.Caching
{
	public interface ICacheHelper
	{
		bool TryGetValue<T>(string key, out T value);

		void Set<T>(string key, TimeSpan timeSpan, T value);
	}
}
