using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace TAGov.Common
{
	public class UrlService : IUrlService
	{
		private readonly Lazy<string> _grmEventServiceApiUrl;
		private readonly IConfiguration _configurationRoot;

		public UrlService(IConfiguration configurationRoot)
		{
			_configurationRoot = configurationRoot;

			_grmEventServiceApiUrl = new Lazy<string>(() => GetConfigurationSetting("ServiceApiUrls:grmEventServiceApiUrl"), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public string GrmEventServiceApiUrl => _grmEventServiceApiUrl.Value;

		private string GetConfigurationSetting(string settingName)
		{
			var setting = _configurationRoot.GetSection(settingName).Value;
			if (string.IsNullOrWhiteSpace(setting))
			{
				throw new ArgumentException(string.Format("Could not find configuration setting '{0}'.", settingName));
			}

			return setting;
		}
	}
}
