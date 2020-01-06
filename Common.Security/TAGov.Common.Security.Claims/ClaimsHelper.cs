using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TAGov.Common.Security.Claims
{
	public static class ClaimsHelper
	{
		private const string ClaimTypeUrl = "http://thomsonreuters.com/tagov";

		public static Claim ToClaim(string applicationName, string resource, bool canGet, bool canPost,
			bool canPut, bool canDelete)
		{
			var key = GetKey(applicationName, resource);
			const string value = "GET:{0}|POST:{1}|PUT:{2}|DELETE:{3}";

			return new Claim(key, string.Format(value, canGet, canPost, canPut, canDelete));
		}

		private static string GetKey(string applicationName, string resource)
		{
			// Case insensitive.
			return $"{ClaimTypeUrl}/{applicationName}/{resource}".ToLowerInvariant();
		}

		public static Claim GetClaim(this IEnumerable<Claim> claims, string applicationName, string resource)
		{
			var key = GetKey(applicationName, resource);
			return claims.SingleOrDefault(x => x.Type == key);
		}

		private static Dictionary<string, bool> GetPermissions(string value)
		{
			var dictionary = new Dictionary<string, bool>();
			value.Split('|').ToList().ForEach(item =>
			{
				var pair = item.Split(':');
				dictionary.Add(pair[0], Convert.ToBoolean(pair[1]));
			});
			return dictionary;
		}

		public static bool CanGet(this Claim claim)
		{
			var dictionary = GetPermissions(claim.Value);
			return dictionary["GET"];
		}

		public static bool CanPost(this Claim claim)
		{
			var dictionary = GetPermissions(claim.Value);
			return dictionary["POST"];
		}

		public static bool CanPut(this Claim claim)
		{
			var dictionary = GetPermissions(claim.Value);
			return dictionary["PUT"];
		}

		public static bool CanDelete(this Claim claim)
		{
			var dictionary = GetPermissions(claim.Value);
			return dictionary["DELETE"];
		}
	}
}
