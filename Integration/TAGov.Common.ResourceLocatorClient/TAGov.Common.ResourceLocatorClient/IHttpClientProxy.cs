namespace TAGov.Common.ResourceLocatorClient
{
	public interface IHttpClientProxy
	{
		string Get(string uri, string endPoint);
		string Post<T>(string uri, string endPoint, T dto);
	}
}
