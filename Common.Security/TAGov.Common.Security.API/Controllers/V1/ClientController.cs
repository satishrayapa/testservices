using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Domain.Models.V1;
using TAGov.Common.Security.Http.Authorization;

namespace TAGov.Common.Security.API.Controllers.V1
{
	/// <summary>
	/// Client API.
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
    [ApiController]
    [ApiVersion("1")]
	[Route("v{version:apiVersion}/Clients")]
	public class ClientController : ControllerBase
	{
		private readonly IClientDomain _clientDomain;

		/// <summary>
		/// Constructor to Client API.
		/// </summary>
		/// <param name="clientDomain"></param>
		public ClientController(IClientDomain clientDomain)
		{
			_clientDomain = clientDomain;
		}

		/// <summary>
		/// Create a new Client.
		/// </summary>
		/// <param name="clientDto">ClientDto in body.</param>
		[HttpPost]
		[ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Post([FromBody]ClientDto clientDto)
		{
			await _clientDomain.Create(clientDto);

			return Ok();
		}

		/// <summary>
		/// Updates an existing ClientDto.
		/// </summary>
		/// 
		/// <param name="id">Username of client.</param>
		/// <param name="clientDto">ClientDto in body.</param>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Put(string id, [FromBody]ClientDto clientDto)
		{
			if (id != clientDto.Username)
				throw new BadRequestException("Route Id does not match with client username.");

			await _clientDomain.Update(clientDto);

			return Ok();
		}
	}
}
