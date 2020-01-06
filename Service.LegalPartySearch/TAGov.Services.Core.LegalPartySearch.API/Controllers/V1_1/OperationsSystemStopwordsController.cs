using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.API.Controllers.V1_1
{
	/// <summary>
	/// System Stopwords operational API.
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
	[ApiVersion("1.1")]
	[Route("v{version:apiVersion}/Operations/SystemStopwords")]
	public class OperationsSystemStopwordsController : Controller
	{
		private readonly ISearchOperations _searchOperations;
		/// <summary>
		/// Operational details for stopword management.
		/// </summary>
		/// <param name="searchOperations"></param>
		public OperationsSystemStopwordsController(ISearchOperations searchOperations)
		{
			_searchOperations = searchOperations;
		}

		/// <summary>
		/// Enable system stopwords.
		/// </summary>
		[HttpPost]
		public async Task EnableStopwords()
		{
			await _searchOperations.EnableStopwords();
		}

		/// <summary>
		/// Disable system stopwords.
		/// </summary>
		[HttpDelete]
		public async Task DisableStopwords()
		{
			await _searchOperations.DisableStopwords();
		}
	}
}
