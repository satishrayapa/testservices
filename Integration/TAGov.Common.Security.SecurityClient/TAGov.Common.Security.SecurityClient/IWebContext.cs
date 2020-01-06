namespace TAGov.Common.Security.SecurityClient
{
	public interface IWebContext
	{
		bool IsExist();
		object GetSessionProfileLoginId();

		void AddToCache<T>(string key, T value, int timeoutInSeconds);

		T GetFromCache<T>(string key);
	}
}
