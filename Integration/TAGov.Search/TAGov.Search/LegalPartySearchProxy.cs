using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TAGov.Common.ResourceLocatorClient;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Search
{
	public class LegalPartySearchProxy : ISearchProxy
	{
		private readonly IHttpClientProxy _httpClientProxy;
		private readonly IFeatureToggle _featureToggle;
		private readonly IUrlServices _urlServices;


		private Features _feature;

		public LegalPartySearchProxy(IHttpClientProxy httpClientProxy, IFeatureToggle featureToggle, IUrlServices urlServices)
		{
			_httpClientProxy = httpClientProxy;
			_featureToggle = featureToggle;
			_urlServices = urlServices;
		}

		public bool CanHandle(Features feature)
		{
			_feature = feature;
			return _featureToggle.IsEnabled(feature);
		}

		private void ExcludeNoneFeatureSpecificSearchParameters(SearchLegalPartyQueryDto searchLegalPartyQueryDto)
		{
			switch (_feature)
			{
				case Features.LegalPartySearch:
					searchLegalPartyQueryDto.ExcludeTag = true;
					searchLegalPartyQueryDto.ExcludeGeoCode = true;
					break;
			}
		}

		public IEnumerable<SearchLegalPartyDto> Search(SearchLegalPartyQueryDto legalPartySearchQuery)
		{
			var uri = _urlServices.GetServiceUri(Constants.ServiceLegalPartySearch);

			ExcludeNoneFeatureSpecificSearchParameters(legalPartySearchQuery);

			string result;

			try
			{
				result = _httpClientProxy.Post(uri.ToString(), "v1.1/SearchLegalParties", legalPartySearchQuery);
			}
			catch (HttpRequestException e)
			{
				if (e.Message.Contains("403 (Forbidden)"))
				{
					throw new InvalidProgramException("You do not have permission to operate this page. Please contact your system administrator for more details.");
				}
				throw;
			}
			catch (TimeoutException e)
			{
				throw new TimeoutException("The search information you have provided is too vague.  Please refine your search and try again.", e);
			}

			if (!string.IsNullOrEmpty(result))
			{
				return JsonConvert.DeserializeObject<List<SearchLegalPartyDto>>(result);
			}

			return null;
		}
	}
}
