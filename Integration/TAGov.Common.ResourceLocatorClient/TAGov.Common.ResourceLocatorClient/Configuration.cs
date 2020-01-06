using System.Configuration;

namespace TAGov.Common.ResourceLocatorClient
{
	public class Configuration : IConfiguration
	{
		public string Get(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
