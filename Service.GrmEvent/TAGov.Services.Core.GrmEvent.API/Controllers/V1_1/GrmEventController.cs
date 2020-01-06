using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.GrmEvent.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.GrmEvent.API.Controllers.V1_1
{
    /// <summary>
    /// GRM Event API.
    /// </summary>
    [ApiController]
    [Authorize(Constants.HasResourceVerb)]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/GrmEvents")]
    public class GrmEventController : ControllerBase
    {
        private readonly IGrmEventDomain _grmEventDomain;

        /// <summary>
        /// Constructor to GRM Event API.
        /// </summary>
        /// <param name="grmEventDomain"></param>
        public GrmEventController(IGrmEventDomain grmEventDomain)
        {
            _grmEventDomain = grmEventDomain;
        }


        /// <summary>
        /// Gets GRM Event DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(GrmEventDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id)
        {
            var grmEvent = _grmEventDomain.Get(id);
            return new ObjectResult(grmEvent);
        }

        /// <summary>
        /// Gets GRM Event Information DTO.
        /// </summary>
        /// <param name="revObjId"></param>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        [HttpGet, Route("GrmEventInformation/RevObjId/{revObjId}/EffectiveDate/{effectiveDate}")]
        [ProducesResponseType(typeof(GrmEventInformationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int revObjId, DateTime effectiveDate)
        {
            var grmEvent = _grmEventDomain.GetGrmEventInfoByRevObjIdAndEffectiveDate(revObjId, effectiveDate);
            return new ObjectResult(grmEvent);
        }

        /// <summary>
        /// Creates a GrmEvent
        /// </summary>
        /// <param name="grmEventListCreate"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateGrmEvents")]
        [ProducesResponseType(typeof(GrmEventListCreateDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BadRequestException), (int)HttpStatusCode.BadRequest)]
        public IActionResult CreateGrmEvent([FromBody] GrmEventListCreateDto grmEventListCreate)
        {
            var grmEventList = _grmEventDomain.CreateGrmEvents(grmEventListCreate);

            return new ObjectResult(grmEventList);
        }

        /// <summary>
        /// Deletes a GrmEvent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public IActionResult Delete(int id)
        {
            _grmEventDomain.Delete(id);

            return new NoContentResult();
        }
    }
}
