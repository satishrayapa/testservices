using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.LegalParty.Domain.Interfaces;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;

namespace TAGov.Services.Core.LegalParty.API.Controllers.V1_1
{
    /// <summary>
    /// Legal Party API.
    /// </summary>
    [ApiController]
    [Authorize(Constants.HasResourceVerb)]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/LegalParties")]
    public class LegalPartyController : ControllerBase
    {
        private readonly ILegalPartyDomain _legalPartyDomain;
        private readonly ILegalPartyOfficialDocumentDomain _legalPartyOfficialDocumentDomain;

        /// <summary>
        /// Constructor to Legal Party API.
        /// </summary>
        /// <param name="legalPartyDomain"></param>
        /// <param name="legalPartyOfficialDocumentDomain"></param>
        public LegalPartyController(ILegalPartyDomain legalPartyDomain, ILegalPartyOfficialDocumentDomain legalPartyOfficialDocumentDomain)
        {
            _legalPartyDomain = legalPartyDomain;
            _legalPartyOfficialDocumentDomain = legalPartyOfficialDocumentDomain;
        }

        /// <summary>
        /// Gets LegalPartyRole DTOs based on Revenue Object Id and Effective Date.
        /// </summary>
        /// <param name="revenueObjectId">Revenue Object Id related to Legal Party</param>
        /// <param name="effectiveDate">date time format yyyy-MM-DDTHH:mm:ss.fffffffK ISO8601 format</param>
        /// <returns></returns>
        [HttpGet, Route("LegalPartyRoles/RevenueObjectId/{revenueObjectId}/EffectiveDate/{effectiveDate}")]
        [ProducesResponseType(typeof(IList<LegalPartyRoleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate(int revenueObjectId, DateTime effectiveDate)
        {
            var legalParty = _legalPartyDomain.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate(revenueObjectId, effectiveDate);
            return new ObjectResult(legalParty);
        }

        /// <summary>
        /// Gets a list of LegalPartyRoleDocument DTOs.
        /// </summary>
        /// <param name="legalPartySearchDto">Search criteria based on Legal Party Role Id List.</param>
        /// <returns></returns>
        [HttpPost, Route("Documents")]
        [ProducesResponseType(typeof(IList<LegalPartyDocumentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLegalPartyRoleDocuments([FromBody] LegalPartySearchDto legalPartySearchDto)
        {
            var legalPartyRoles = await _legalPartyOfficialDocumentDomain.ListAsync(legalPartySearchDto.LegalPartyRoleIdList, legalPartySearchDto.EffectiveDate);
            return new ObjectResult(legalPartyRoles);
        }

        /// <summary>
        /// Gets a list of LegalPartyRole DTOs.
        /// </summary>
        /// <param name="legalPartyRoleIdList">Search criteria based on Legal Party Role Id List and Effective Date..</param>
        /// <returns></returns>
        [HttpPost, Route("Roles")]
        [ProducesResponseType(typeof(IList<LegalPartyRoleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
        public IActionResult GetLegalPartyRoles([FromBody] int[] legalPartyRoleIdList)
        {
            var legalPartyRoles = _legalPartyDomain.GetLegalPartyRolesById(legalPartyRoleIdList);
            return new ObjectResult(legalPartyRoles);
        }
    }
}
