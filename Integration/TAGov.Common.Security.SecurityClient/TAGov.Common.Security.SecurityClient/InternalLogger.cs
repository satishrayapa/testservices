using System;
using System.Globalization;

namespace TAGov.Common.Security.SecurityClient
{
	public class InternalLogger : IInternalLogger
	{
		private readonly ISecurityConfiguration _securityConfiguration;

		public InternalLogger(ISecurityConfiguration securityConfiguration)
		{
			_securityConfiguration = securityConfiguration;
		}

		public void AppendLog(string text)
		{
			var logLocation = _securityConfiguration.LogLocation;

			if (string.IsNullOrEmpty(logLocation)) return;

			System.IO.File.AppendAllText(logLocation,
				DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - " + text + "\r\n");
		}
	}
}