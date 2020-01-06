using System.Text.RegularExpressions;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public static class Extensions
	{
		public static bool CaseInsensitiveContains(this string source, string toCheck)
		{
			var reg = new Regex(@"\b" + toCheck.Replace(".", @"[\.]?") + @"\b", RegexOptions.IgnoreCase);
			return reg.IsMatch(source);
		}
	}
}