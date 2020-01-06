using System;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace TAGov.Common.Security.SecurityClient
{
	public class JwtTokenRequestProcessor : IHttpHandler, IReadOnlySessionState
	{

		public JwtTokenRequestProcessor()
		{
			IsReusable = true;
		}

		public void ProcessRequest(HttpContext context)
		{
			var config = new SecurityConfiguration();
			var internalLogger = new InternalLogger(config);
			var webContext = new WebContext();

			var tokenRequestClient = new JwtTokenRequestClient(config, internalLogger, new UserProfileId(webContext, internalLogger), new JwtTokenCache(webContext));

			var result = tokenRequestClient.ProcessByUserProfileId();
			if (result != null)
			{
				var response = context.Response;
				response.Write(JsonConvert.SerializeObject(new
				{
					result.AccessToken,
					result.ExpiresIn,
					result.TokenType
				}));

				response.Cache.SetCacheability(HttpCacheability.NoCache);
				response.Cache.SetNoStore();
			}
		}

		public bool IsReusable { get; }
		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		{
			throw new InvalidOperationException();
		}

		public void EndProcessRequest(IAsyncResult result)
		{

		}
	}
}
