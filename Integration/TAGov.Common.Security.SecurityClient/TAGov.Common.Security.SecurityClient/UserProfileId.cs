using System;
using System.Web;

namespace TAGov.Common.Security.SecurityClient
{
	public class UserProfileId : IUserProfileId
	{
		private readonly IWebContext _webContext;
		private readonly IInternalLogger _internalLogger;

		public UserProfileId(IWebContext webContext, IInternalLogger internalLogger)
		{
			_webContext = webContext;
			_internalLogger = internalLogger;
		}

		public bool IsAuthenticated(out int profileLoginId)
		{
			_internalLogger.AppendLog("Entering IsAuthenticated check.");

			if (!_webContext.IsExist())
			{
				_internalLogger.AppendLog("HttpContext is null!");
				profileLoginId = -1;
				return false;
			}

			var sessionProfileLoginId = _webContext.GetSessionProfileLoginId();
			if (sessionProfileLoginId != null)
			{
				profileLoginId = Convert.ToInt32(sessionProfileLoginId);
				_internalLogger.AppendLog("ProfileLoginId: " + profileLoginId);
				return true;
			}

			_internalLogger.AppendLog("ProfileLoginId is null!");
			profileLoginId = -1;
			return false;
		}
	}
}