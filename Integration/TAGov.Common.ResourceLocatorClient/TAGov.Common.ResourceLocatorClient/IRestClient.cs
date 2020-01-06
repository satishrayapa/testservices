using System.Collections.Generic;

namespace TAGov.Common.ResourceLocatorClient
{
	public interface IRestClient
	{
		ResourceDto GetResource(string key);
		IEnumerable<ResourceDto> GetResources(string key);
	}
}