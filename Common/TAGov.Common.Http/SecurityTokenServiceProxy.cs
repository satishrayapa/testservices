using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;

namespace TAGov.Common.Http
{
	public class SecurityTokenServiceProxy : ISecurityTokenServiceProxy
	{
		private const string NOT_SET = "NOT SET";
		private IHttpContextAccessor _contextAccessor;
		private HttpContext _context { get { return _contextAccessor.HttpContext; } }
		
		public SecurityTokenServiceProxy(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}

		public string GetAccessToken()
		{
			if (this._context !=  null)
			{
				if (this._context.Request.Headers.ContainsKey("Authorization"))
				{
					var authorizationHeader = this._context.Request.Headers["Authorization"];

					if (authorizationHeader.Count > 0 )
					{
						string token = authorizationHeader[0];

						if (token.ToUpper().StartsWith("BEARER "))
						{
							return token;
						}
					}
				}
			}

			return null;

		}
	}
}
