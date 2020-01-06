using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.AssessmentEvent.Domain.Interfaces;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.API.Controllers.V1_1
{
  /// <summary>
  /// Assessment Event API.
  /// </summary>
  [ApiController]
  [Authorize(Constants.HasResourceVerb)]
  [ApiVersion("1.1")]
  [Route("v{version:apiVersion}/AssessmentEvents")]
  public class AssessmentEventController : ControllerBase
  {
    private readonly IAssessmentEventDomain _assessmentEventDomain;

    /// <summary>
    /// Constructor to Assessment Event API.
    /// </summary>
    /// <param name="assessmentEventDomain"></param>
    public AssessmentEventController(IAssessmentEventDomain assessmentEventDomain)
    {
      _assessmentEventDomain = assessmentEventDomain;
    }

    /// <summary>
    /// Gets AssessmentEvent DTO.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route("{id}")]
    [ProducesResponseType(typeof(AssessmentEventDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(int id)
    {
      var assessmentEvent = await _assessmentEventDomain.GetAsync(id);
      return new ObjectResult(assessmentEvent);
    }

    /// <summary>
    /// Gets the Assessment Revision DTO corresponding to the AssessmentRevisionEventId passed in.
    /// </summary>
    /// <param name="id">identifier to AssessmentRevisionEvent to use to get AssessmentRevision</param>
    /// <param name="assessmentEventDate">assessment event date</param>
    /// <returns>AssessmentRevision</returns>
    [HttpGet, Route("AssessmentRevisionEventId/{id}/AssessmentEventDate/{assessmentEventDate}/AssessmentRevision/")]
    [ProducesResponseType(typeof(AssessmentRevisionDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    public IActionResult GetAssessmentRevisionByAssessmentRevisionEventId(int id, DateTime assessmentEventDate)
    {
      return new ObjectResult(_assessmentEventDomain.GetAssessmentRevisionByAssessmentRevisionEventId(id, assessmentEventDate));
    }

    /// <summary>
    /// Gets AssessmentEvent DTOs by revenue object id and event date.
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="eventDate"></param>
    /// <returns></returns>
    [HttpGet, Route("RevenueObjectId/{revenueObjectId}/EventDate/{eventDate}")]
    [ProducesResponseType(typeof(IEnumerable<AssessmentEventDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    public IActionResult Get(int revenueObjectId, DateTime eventDate)
    {
      return new ObjectResult(_assessmentEventDomain.List(revenueObjectId, eventDate));
    }

    /// <summary>
    /// Get a list of AssessmentEvent DTOs with the same Revenue Object Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route("{id}/RevenueObjectBasedAssessmentEvents")]
    [ProducesResponseType(typeof(IEnumerable<RevenueObjectBasedAssessmentEventDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ListRevenueObjectBasedAssessmentEvents(int id)
    {
      return new ObjectResult(await _assessmentEventDomain.ListAsync(id));
    }

    /// <summary>
    /// Get a IsEditableFlag DTOs.
    /// </summary>
    /// <param name="id">Assessment event Id.</param>
    /// <returns></returns>
    [HttpGet, Route("{id}/IsEditableFlag")]
    [ProducesResponseType(typeof(AssessmentEventIsEditableFlagDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetIsEditableFlag(int id)
    {
      return new ObjectResult(await _assessmentEventDomain.GetIsEditableFlag(id));
    }

    /// <summary>
    /// Get a EffectiveDate DTO for Assessment Event.
    /// </summary>
    /// <param name="id">Assessment event Id.</param>
    /// <returns></returns>
    [HttpGet, Route("{id}/EffectiveDate")]
    [ProducesResponseType(typeof(AssessmentEventEffectiveDateDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetEffectiveDate(int id)
    {
      return new ObjectResult(await _assessmentEventDomain.GetEffectiveDate(id));
    }

    /// <summary>
    /// Get the current AssessmentRevisionIdDto.
    /// </summary>
    /// <param name="id">Assessment event Id.</param>
    /// <returns></returns>
    [HttpGet, Route("{id}/Revision/Current")]
    [ProducesResponseType(typeof(AssessmentRevisionIdDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetCurrentRevisionId(int id)
    {
      return new ObjectResult(await _assessmentEventDomain.GetCurrentRevisionId(id));
    }
  }
}
