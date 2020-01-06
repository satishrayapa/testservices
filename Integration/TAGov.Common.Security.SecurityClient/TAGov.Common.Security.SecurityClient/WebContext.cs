using System;
using System.Web;
using System.Web.Caching;

namespace TAGov.Common.Security.SecurityClient
{
	public class WebContext : IWebContext
	{
		public bool IsExist()
		{
			return HttpContext.Current != null;
		}

		public object GetSessionProfileLoginId()
		{
			return HttpContext.Current.Session["ProfileLoginId"];
		}

		public void AddToCache<T>(string key, T value, int timeoutInSeconds)
		{
			HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddSeconds(timeoutInSeconds),
				TimeSpan.Zero, CacheItemPriority.Normal, null);
		}

		public T GetFromCache<T>(string key)
		{
			return (T)HttpContext.Current.Cache[key];
		}
	}
}