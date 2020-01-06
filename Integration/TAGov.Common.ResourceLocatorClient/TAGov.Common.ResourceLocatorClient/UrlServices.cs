using System;
using System.Linq;

namespace TAGov.Common.ResourceLocatorClient
{
	public class UrlServices : IUrlServices
	{
		private readonly IRestClient _client;
		private readonly IConfiguration _configuration;

		public UrlServices(IRestClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
		}
		public Uri GetServiceUri(string key)
		{
			var partition = _configuration.Get(Constants.CommonResourceLocatorPartition);

			if (string.IsNullOrEmpty(partition)) return null;

			var dtos = _client.GetResources( $"urlservices:{partition}");

			if (dtos != null) return new Uri(dtos.Single(x => x.Key == key).Value);

			return null;
		}
	}
}
