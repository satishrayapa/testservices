using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Domain.Models.V1;
using TAGov.Common.Security.Http.Authorization;

namespace TAGov.Common.Security.API.Controllers.V1
{
	/// <summary>
	/// ClientInfo API.
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
	[ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/ClientInfos")]
	public class ClientInfoController : ControllerBase
	{
		private readonly IClientDomain _clientDomain;
		/// <summary>
		/// Constructor to ClientInfo API.
		/// </summary>
		/// <param name="clientDomain"></param>
		public ClientInfoController(IClientDomain clientDomain)
		{
			_clientDomain = clientDomain;
		}

		/// <summary>
		/// Gets a list of avaliable ClientInfoDtos.
		/// </summary>
		/// <returns>ClientDto list.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ClientInfoDto>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get()
		{
			return new ObjectResult(await _clientDomain.List());
		}

		/// <summary>
		/// Gets a single ClientInfoDto by username.
		/// </summary>
		/// <param name="username">Username of client.</param>
		/// <returns>ClientDto.</returns>
		[HttpGet("{username}")]
		[ProducesResponseType(typeof(ClientInfoDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(string username)
		{
			return new ObjectResult(await _clientDomain.Get(username));
		}
	}
}
