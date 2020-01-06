using System;

namespace TAGov.Common.ResourceLocatorClient
{
	public interface IUrlServices
	{
		Uri GetServiceUri(string key);
	}
}
