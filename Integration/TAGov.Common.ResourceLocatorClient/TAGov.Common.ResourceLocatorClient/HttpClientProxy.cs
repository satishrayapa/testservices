using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TAGov.Common.ResourceLocatorClient.Models;
using TAGov.Common.Security.SecurityClient;

namespace TAGov.Common.ResourceLocatorClient
{
	public class HttpClientProxy : IHttpClientProxy
	{
		private readonly ISecurityTokenServiceProxy _securityTokenServiceProxy;

		public HttpClientProxy(ISecurityTokenServiceProxy securityTokenServiceProxy)
		{
			_securityTokenServiceProxy = securityTokenServiceProxy;
		}

		public string Get(string uri, string endPoint)
		{
			try
			{
				var httpClient = GetHttpClient(uri);

				return httpClient.GetStringAsync(endPoint).Result;
			}
			catch (Exception e)
			{
				if (e.InnerException is TaskCanceledException &&
					!((TaskCanceledException)e.InnerException).CancellationToken.IsCancellationRequested)
				{
					throw new TimeoutException("The request to the resource has timed out.", e);
				}
				throw;
			}
		}

		public string Post<T>(string uri, string endPoint, T dto)
		{
			try
			{
				var httpClient = GetHttpClient(uri);

				HttpResponseMessage result = httpClient.PostAsync(endPoint, new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")).Result;

				string responseInJson;

				HandleErrors(result, out responseInJson);

				return responseInJson;
			}
			catch (Exception e)
			{
				if (e.InnerException is TaskCanceledException &&
					!((TaskCanceledException)e.InnerException).CancellationToken.IsCancellationRequested)
				{
					throw new TimeoutException("The request to the resource has timed out.", e);
				}
				throw;
			}
		}

		private void HandleErrors(HttpResponseMessage result, out string responseInJson)
		{
			responseInJson = result.Content.ReadAsStringAsync().Result;
			try
			{
				result.EnsureSuccessStatusCode();
			}
			catch (HttpRequestException httpRequestException)
			{
				if (httpRequestException.Message.Contains("400 (Bad Request)") &&
					!string.IsNullOrEmpty(responseInJson))
				{
					var error = JsonConvert.DeserializeObject<ErrorDto>(responseInJson);

					throw new InvalidProgramException(error.Message);
				}
				throw;
			}

		}

		private HttpClient GetHttpClient(string uri)
		{
			if (!uri.EndsWith("/")) uri += "/";

			var httpClient = new HttpClient { BaseAddress = new Uri(uri) };

			var accessToken = _securityTokenServiceProxy.GetAccessToken();

			if (!string.IsNullOrEmpty(accessToken))
				httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return httpClient;
		}
	}
}