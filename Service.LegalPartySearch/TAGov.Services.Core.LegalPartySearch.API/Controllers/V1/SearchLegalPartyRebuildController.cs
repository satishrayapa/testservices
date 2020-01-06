using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.API.Controllers.V1
{
	/// <summary>
	/// LegalPartySearch LegalPartyRebuild API
	/// </summary>
	[ApiVersion("1")]
	[Route("v{version:apiVersion}/SearchLegalParty/Rebuild")]
	public class SearchLegalPartyRebuildController : Controller
	{
		private readonly IRebuildSearchLegalParty _rebuildSearchLegalParty;

		/// <summary>
		/// Constructor for the LegalPartyRebuild API
		/// </summary>
		/// <param name="rebuildSearchLegalParty"></param>
		public SearchLegalPartyRebuildController(IRebuildSearchLegalParty rebuildSearchLegalParty)
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
