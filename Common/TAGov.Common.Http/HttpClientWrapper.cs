using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TAGov.Common.Exceptions;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TAGov.Common.Http
{
	public class HttpClientWrapper : IHttpClientWrapper
	{
		private readonly ISecurityTokenServiceProxy _securityTokenServiceProxy;
		private readonly ILogger _logger;

		public HttpClientWrapper(ISecurityTokenServiceProxy securityTokenServiceProxy, ILoggerFactory loggerFactory)
		{
			_securityTokenServiceProxy = securityTokenServiceProxy;
			_logger = loggerFactory.CreateLogger("httplistener");
		}

		public async Task<T> Get<T>(string baseUri, string requestUri)
		{
			// TODO: Make HttpClient instantiation into a reuseable object
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUri);

				var accessToken = _securityTokenServiceProxy.GetAccessToken();

				_logger.LogInformation("============= HTTP GET ==============");
				_logger.LogInformation("baseUri: " + baseUri);
				_logger.LogInformation("requestUri: " + requestUri);
				_logger.LogInformation("Token: " + accessToken);

				if (!string.IsNullOrEmpty(accessToken))
					client.DefaultRequestHeaders.Add("Authorization", accessToken);

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.GetAsync(requestUri);

				await AssertAndHandleHttpException(response, baseUri, requestUri);

				var jsonResult = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(jsonResult);
			}
		}

		/// <summary>
		/// The intent of this handler is to try to show the facades using it with some detailed error message.
		/// </summary>
		/// <param name="httpResponseMessage"></param>
		/// <param name="baseUri"></param>
		/// <param name="requestUri"></param>
		private async Task AssertAndHandleHttpException(HttpResponseMessage httpResponseMessage, string baseUri, string requestUri)
		{
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
					throw new BadRequestException($"BaseUri:{baseUri}, requestUri:{requestUri} caused a Request Exception with status code:{httpResponseMessage.StatusCode}. Details: {await httpResponseMessage.Content.ReadAsStringAsync()}");

				if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
					throw new NotFoundException($"BaseUri:{baseUri}, requestUri:{requestUri} caused a Request Exception with status code:{httpResponseMessage.StatusCode}. Details: {await httpResponseMessage.Content.ReadAsStringAsync()}");

				if ( httpResponseMessage.StatusCode == HttpStatusCode.Forbidden )
					throw new ForbiddenException( $"BaseUri:{baseUri}, requestUri:{requestUri} caused a Request Exception with status code:{httpResponseMessage.StatusCode}.  Details: {await httpResponseMessage.Content.ReadAsStringAsync()}" );

				if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
					throw new UnauthorizedException($"BaseUri:{baseUri}, requestUri:{requestUri} caused a Request Exception with status code:{httpResponseMessage.StatusCode}.  Details: {await httpResponseMessage.Content.ReadAsStringAsync()}");

				// Throw a default error message because we cannot handle it.
				httpResponseMessage.EnsureSuccessStatusCode();
			}
		}

		public async Task<T> Post<T>(string baseUri, string requestUri, object data)
		{
			// TODO: Make HttpClient instantiation into a reuseable object
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUri);

				var accessToken = _securityTokenServiceProxy.GetAccessToken();

				_logger.LogInformation("============= HTTP POST ==============");
				_logger.LogInformation("baseUri: " + baseUri);
				_logger.LogInformation("requestUri: " + requestUri);
				_logger.LogInformation("Token: " + accessToken);

				if (!string.IsNullOrEmpty(accessToken))
					client.DefaultRequestHeaders.Add("Authorization", accessToken);

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string postBody = JsonConvert.SerializeObject(data);

				var response = await client.PostAsync(requestUri, new StringContent(postBody, Encoding.UTF8, "application/json"));

				await AssertAndHandleHttpException(response, baseUri, requestUri);

				var jsonResult = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(jsonResult);
			}
		}

		public async Task Put(string baseUri, string requestUri, object data)
		{
			// TODO: Make HttpClient instantiation into a reuseable object
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUri);

				var accessToken = _securityTokenServiceProxy.GetAccessToken();

				_logger.LogInformation("============= HTTP PUT ==============");
				_logger.LogInformation("baseUri: " + baseUri);
				_logger.LogInformation("requestUri: " + requestUri);
				_logger.LogInformation("Token: " + accessToken);

				if (!string.IsNullOrEmpty(accessToken))
					client.DefaultRequestHeaders.Add("Authorization", accessToken);

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string putBody = JsonConvert.SerializeObject(data);

				var response = await client.PutAsync(requestUri, new StringContent(putBody, Encoding.UTF8, "application/json"));

				await AssertAndHandleHttpException(response, baseUri, requestUri);
			}
		}

		public async Task Delete(string baseUri, string requestUri)
		{
			// TODO: Make HttpClient instantiation into a reuseable object
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUri);

				var accessToken = _securityTokenServiceProxy.GetAccessToken();

				_logger.LogInformation("============= HTTP DELETE ==============");
				_logger.LogInformation("baseUri: " + baseUri);
				_logger.LogInformation("requestUri: " + requestUri);
				_logger.LogInformation("Token: " + accessToken);

				if (!string.IsNullOrEmpty(accessToken))
					client.DefaultRequestHeaders.Add("Authorization", accessToken);

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.DeleteAsync(requestUri);

				await AssertAndHandleHttpException(response, baseUri, requestUri);
			}
		}
	}
}
