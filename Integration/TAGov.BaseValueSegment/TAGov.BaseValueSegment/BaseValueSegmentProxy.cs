using Newtonsoft.Json;
using TAGov.Common.ResourceLocatorClient;

namespace TAGov.BaseValueSegment
{
	public class BaseValueSegmentProxy : IBaseValueSegmentProxy
	{
		private readonly IHttpClientProxy _httpClientProxy;
		private readonly IUrlServices _urlServices;

		public BaseValueSegmentProxy( IHttpClientProxy httpClientProxy, IUrlServices urlServices )
		{
			_httpClientProxy = httpClientProxy;
			_urlServices = urlServices;
		}
		public BaseValueSegmentDto Save( int baseValueSegmentId, int assessmentEventId, BaseValueSegmentDto baseValueSegmentDto )
		{
			var uri = _urlServices.GetServiceUri( Constants.FacadeBaseValueSegment );

			var saveResult = _httpClientProxy.Post( uri.ToString(),
			                                        "v1.1/BaseValueSegments/" + baseValueSegmentId + "/AssessmentEventId/" + assessmentEventId,
			                                        baseValueSegmentDto );
			if (!string.IsNullOrEmpty(saveResult))
			{
				return JsonConvert.DeserializeObject<BaseValueSegmentDto>(saveResult);
			}

			return null;
		}
	}
}
