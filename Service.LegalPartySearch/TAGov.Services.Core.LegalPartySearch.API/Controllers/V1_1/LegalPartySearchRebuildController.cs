using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.API.Controllers.V1_1
{
   
	/// <summary>
	/// LegalPartySearch LegalPartyRebuild API
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
	[ApiVersion("1.1")]
	[Route("v{version:apiVersion}/SearchLegalParty/Rebuild")]
	public class LegalPartySearchRebuildController : Controller
	{
		private readonly IRebuildSearchLegalParty _rebuildSearchLegalParty;

		/// <summary>
		/// Constructor for the LegalPartyRebuild API
		/// </summary>
		/// <param name="rebuildSearchLegalParty"></param>
		public LegalPartySearchRebuildController(IRebuildSearchLegalParty rebuildSearchLegalParty)
		{
			_rebuildSearchLegalParty = rebuildSearchLegalParty;
		}

		/// <summary>
		/// Rebuild SearchLegalParty.
		/// </summary>
		/// <param name="rebuildSearchLegalPartyDto">List of Id.</param>
		/// <returns></returns>
		[HttpPost]
		public async Task Do([FromBody]RebuildSearchLegalPartyDto rebuildSearchLegalPartyDto)
		{
			await _rebuildSearchLegalParty.DoAsync(rebuildSearchLegalPartyDto);
		}
	}
}
