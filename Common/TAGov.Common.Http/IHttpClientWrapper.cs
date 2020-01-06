using System.Threading.Tasks;

namespace TAGov.Common.Http
{
	public interface IHttpClientWrapper
	{
		Task<T> Get<T>(string baseUri, string requestUri);

		Task<T> Post<T>(string baseUri, string requestUri, object data);

		Task Put(string baseUri, string requestUri, object data);

		Task Delete(string baseUri, string requestUri);
	}
}
