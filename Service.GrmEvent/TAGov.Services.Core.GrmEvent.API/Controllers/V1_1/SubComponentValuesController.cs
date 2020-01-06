using System;
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
    /// Subcomponent values API.
    /// </summary>
    [ApiController]
    [Authorize(Constants.HasResourceVerb)]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/SubComponentValues")]
    public class SubComponentValuesController : ControllerBase
    {
        private readonly IGrmEventDomain _grmEventDomain;

        /// <summary>
        /// Constructor to Sub Component Values API.
        /// </summary>
        /// <param name="grmEventDomain"></param>
        public SubComponentValuesController(IGrmEventDomain grmEventDomain)
        {
            _grmEventDomain = grmEventDomain;
        }

        /// <summary>
        /// Gets list of SubComponentValues DTOs.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        [HttpGet, Route("pin/{pin}/EffectiveDate/{effectiveDate}")]
        [ProducesResponseType(typeof(IEnumerable<SubComponentValueDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(string pin, DateTime effectiveDate)
        {
            return new ObjectResult(_grmEventDomain.GetSubComponentValues(pin, effectiveDate));
        }
    }
}
