using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.GrmEvent.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.GrmEvent.API.Controllers.V1_1
{
    /// <summary>
    /// GRM Event Information API.
    /// </summary>
    [ApiController]
    [Authorize(Constants.HasResourceVerb)]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/GrmEventInformation")]
    public class GrmEventInformationController : ControllerBase
    {
        private readonly IGrmEventDomain _grmEventDomain;

        /// <summary>
        /// Constructor to GRM Event Information API.
        /// </summary>
        /// <param name="grmEventDomain"></param>
        public GrmEventInformationController(IGrmEventDomain grmEventDomain)
        {
            _grmEventDomain = grmEventDomain;
        }

        /// <summary>
        /// Gets a list of GRM Event Information DTOs.
        /// </summary>
        /// <param name="grmEventSearchDto">Search criteria based on GRM Event Id List.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IList<GrmEventInformationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetGrmEventsByGrmEventSearchDto([FromBody] GrmEventSearchDto grmEventSearchDto)
        {
            IEnumerable<GrmEventInformationDto> grmEventRoles = _grmEventDomain.GetGrmEventInfo(grmEventSearchDto.GrmEventIdList.ToArray());
            return new ObjectResult(grmEventRoles);
        }
    }
}
