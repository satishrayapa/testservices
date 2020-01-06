using System.Collections.Generic;
using System.Security.Claims;

namespace TAGov.Common.Security.Domain.Interfaces
{
    public interface IClaimsProvider
    {
	    IEnumerable<Claim> GetClaims(int userProfileLoginId);
	    IEnumerable<Claim> GetFullApplicationClaims();
	    IEnumerable<Claim> GetFullSecurityClaims();
    }
}
