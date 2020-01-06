using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAGov.Services.Core.LegalPartySearch.API.Controllers.V1_1
{
	/// <inheritdoc />
	/// <summary>
	/// LegalPartySearch LegalPartyRebuildAll API
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
	[ApiVersion("1.1")]
	[Route("v{version:apiVersion}/SearchLegalParty/RebuildAll")]
    public class LegalPartySearchRebuildAllController : Controller
    {
	    private readonly IRebuildSearchLegalParty _rebuildSearchLegalParty;

	    /// <summary>
	    /// Constructor for the LegalPartyRebuildAll API
	    /// </summary>
	    /// <param name="rebuildSearchLegalParty"></param>
	    public LegalPartySearchRebuildAllController(IRebuildSearchLegalParty rebuildSearchLegalParty)
	    {
		    _rebuildSearchLegalParty = rebuildSearchLegalParty;
	    }

	    /// <summary>
	    /// Rebuild SearchLegalParty for all ids.
	    /// </summary>
	    /// <returns></returns>
	    [HttpPost]
	    public async Task Do()
	    {
		    await _rebuildSearchLegalParty.DoAsync();
	    }
    }
}
