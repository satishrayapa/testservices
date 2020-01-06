using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.RevenueObject.Domain.Interfaces;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;

namespace TAGov.Services.Core.RevenueObject.API.Controllers.V1_1
{
    /// <summary>
    /// Revenue Object API.
    /// </summary>
    [ApiController]
    [Authorize(Constants.HasResourceVerb)]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/RevenueObjects")]
    public class RevenueObjectController : Controller
    {
        private readonly IRevenueObjectDomain _revenueObjectDomain;

        /// <summary>
        /// Constructor to Assesment Event API.
        /// </summary>
        /// <param name="revenueObjectDomain"></param>
        public RevenueObjectController(IRevenueObjectDomain revenueObjectDomain)
        {
            _revenueObjectDomain = revenueObjectDomain;
        }

        /// <summary>
        /// Gets RevenueObject DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effectiveDate">date format yyyy-MM-dd ISO8601 format</param>
        /// <returns></returns>
        [HttpGet, Route("{id}/EffectiveDate/{effectiveDate}")]
        [ProducesResponseType(typeof(RevenueObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id, DateTime effectiveDate)
        {
            var revenueObject = _revenueObjectDomain.Get(id, effectiveDate);
            return new ObjectResult(revenueObject);
        }

        /// <summary>
        /// Gets TAG DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effectiveDate">date format yyyy-MM-dd ISO8601 format</param>
        /// <returns></returns>
        [HttpGet, Route("RevenueObjectId/{id}/EffectiveDate/{effectiveDate}/TAG")]
        [ProducesResponseType(typeof(TAGDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetTAGByRevenueObjectIdAndEffectiveDate(int id, DateTime effectiveDate)
        {
            var tag = _revenueObjectDomain.GetTAGByRevenueObjectId(id, effectiveDate);
            return new ObjectResult(tag);
        }

        /// <summary>
        /// GetByPin.
        /// </summary>
        /// <param name="pin">PIN.</param>
        /// <returns>revenueObject.</returns>
        [HttpGet, Route("Pin/{pin}")]
        [ProducesResponseType(typeof(RevenueObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetByPin(string pin)
        {
            var revenueObject = _revenueObjectDomain.GetRevenueObjectByPin(pin);

            return new ObjectResult(revenueObject);
        }

        /// <summary>
        /// Gets TAG DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("RevenueObjectId/{id}/TAG")]
        [ProducesResponseType(typeof(TAGDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetTAGByRevenueObjectId(int id)
        {
            var tag = _revenueObjectDomain.GetTAGByRevenueObjectId(id, DateTime.Now.AddYears(1000));
            return new ObjectResult(tag);
        }

        /// <summary>
        /// Gets Revenue Object DTO with Situs Address.
        /// </summary>
        /// <param name="pin">query by pin</param>
        /// <returns>revenue object dto with situs address</returns>
        [HttpGet, Route("Pin/{pin}/WithSitus")]
        [ProducesResponseType(typeof(RevenueObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetWithSitusByPin(string pin)
        {
            var revenueObject = _revenueObjectDomain.GetRevenueObjectSitusAddressByPin(pin);
            return new ObjectResult(revenueObject);
        }
    }
}