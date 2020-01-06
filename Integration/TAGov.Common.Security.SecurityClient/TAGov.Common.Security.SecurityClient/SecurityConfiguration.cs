using System;
using System.Configuration;

namespace TAGov.Common.Security.SecurityClient
{
	public class SecurityConfiguration : ISecurityConfiguration
	{
	    public string AzureAdResourceId => ConfigurationManager.AppSettings["TAGov.AzureAd.ResourceId"];
	    public string AzureAdClientId => ConfigurationManager.AppSettings["TAGov.AzureAd.ClientId"];
	    public string AzureAdClientPassword => ConfigurationManager.AppSettings["TAGov.AzureAd.ClientPassword"];

        public string Authority => ConfigurationManager.AppSettings["TAGov.Common.Security.Authority"];
		public string ClientId => ConfigurationManager.AppSettings["TAGov.Common.Security.ClientId"];
		public string ClientPassword => ConfigurationManager.AppSettings["TAGov.Common.Security.ClientPassword"];
		public string ClientScope => ConfigurationManager.AppSettings["TAGov.Common.Security.ClientScope"];
		public bool DisableRequireHttps
		{
			get
			{
				var disableRequireHttps = ConfigurationManager.AppSettings["TAGov.Common.Security.DisableRequireHttps"];

				if (string.IsNullOrEmpty(disableRequireHttps)) return false;

				return Convert.ToBoolean(disableRequireHttps);
			}
		}

		public string LogLocation => ConfigurationManager.AppSettings["TAGov.Common.Security.LogLocation"];
		public string ServiceClientId => ConfigurationManager.AppSettings["TAGov.Common.Security.ServiceClientId"];
		public string ServiceClientPassword => ConfigurationManager.AppSettings["TAGov.Common.Security.ServiceClientPassword"];
		public string ServiceClientScope => ConfigurationManager.AppSettings["TAGov.Common.Security.ServiceClientScope"];
	}
}
