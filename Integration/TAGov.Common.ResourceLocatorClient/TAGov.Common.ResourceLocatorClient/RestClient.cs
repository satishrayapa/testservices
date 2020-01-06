using System.Collections.Generic;
using Newtonsoft.Json;

namespace TAGov.Common.ResourceLocatorClient
{
	public class RestClient : IRestClient
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpClientProxy _httpClientProxy;

		public RestClient(IConfiguration configuration, IHttpClientProxy httpClientProxy)
		{
			_configuration = configuration;
			_httpClientProxy = httpClientProxy;
		}

		public ResourceDto GetResource(string key)
		{
			var value = GetResourceString(key);
			return value != null ? JsonConvert.DeserializeObject<ResourceDto>(value) : null;
		}

		private string GetResourceString(string key)
		{
			var uri = _configuration.Get(Constants.CommonResourceLocatorUri);
			var partition = _configuration.Get(Constants.CommonResourceLocatorPartition);

			if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(partition))
			{
				return null;
			}

			var resUri = $"{GetVersion()}/Resources/{partition}/{key}";

			var result = _httpClientProxy.Get(uri, resUri);
			if (string.IsNullOrEmpty(result))
				return null;

			return result;
		}

		public IEnumerable<ResourceDto> GetResources(string partition)
		{
			if (string.IsNullOrEmpty(partition)) return null;

			var uri = _configuration.Get(Constants.CommonResourceLocatorUri);

			var resUri = $"{GetVersion()}/Resources?partition={partition}";

			var result = _httpClientProxy.Get(uri, resUri);

			if (string.IsNullOrEmpty(result))
				return null;

			return JsonConvert.DeserializeObject<List<ResourceDto>>(result);
		}

		protected virtual string GetVersion()
		{
			return "v1";
		}
	}
}
