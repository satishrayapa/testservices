﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.API.Controllers.V1
{
	/// <summary>
	/// LegalPartySearch API
	/// </summary>
	[ApiVersion("1")]
	[Route("v{version:apiVersion}/SearchLegalParties")]
	public class SearchLegalPartiesController : Controller
	{
		private readonly ISearchLegalPartyDomain _searchLegalPartyDomain;

		/// <summary>
		/// Constructor for the LegalPartySearch API
		/// </summary>
		/// <param name="searchLegalPartyDomain"></param>
		public SearchLegalPartiesController(ISearchLegalPartyDomain searchLegalPartyDomain)
		{
			_searchLegalPartyDomain = searchLegalPartyDomain;
		}
		
		/// <summary>
		/// Searches for LegalParties.
		/// </summary>
		/// <param name="searchLegalPartyQueryDto"></param>
		[HttpPost, Route("")]
		[ProducesResponseType(typeof(IEnumerable<SearchLegalPartyDto>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadRequestException), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(NotFoundException), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Search([FromBody]SearchLegalPartyQueryDto searchLegalPartyQueryDto)
		{
			return new ObjectResult(await _searchLegalPartyDomain.SearchAsync(searchLegalPartyQueryDto));
		}
	}
}
