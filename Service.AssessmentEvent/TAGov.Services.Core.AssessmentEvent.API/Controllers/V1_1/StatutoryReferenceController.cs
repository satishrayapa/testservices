using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.AssessmentEvent.Domain.Interfaces;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.API.Controllers.V1_1
{
  /// <summary>
  /// StatutoryReference API.
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/StatutoryReference" )]
  public class StatutoryReferenceController : ControllerBase
  {
    private readonly IStatutoryReferenceDomain _statutoryReferenceDomain;

    /// <summary>
    /// Constructor to StatutoryReference API.
    /// </summary>
    /// <param name="statutoryReferenceDomain"></param>
    public StatutoryReferenceController( IStatutoryReferenceDomain statutoryReferenceDomain )
    {
      _statutoryReferenceDomain = statutoryReferenceDomain;
    }

    /// <summary>
    /// Get StatutoryReference by assessmentEventTransactionId.
    /// </summary>
    /// <param name="assessmentEventTransactionId">assessmentEventTransactionId.</param>
    /// <returns>StatutoryReferenceDto</returns>
    [HttpGet, Route("{assessmentEventTransactionId}")]
    [ProducesResponseType(typeof(StatutoryReferenceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
    public StatutoryReferenceDto GetByAssessmentEventTransactionId( int assessmentEventTransactionId )
    {
      return _statutoryReferenceDomain.GetStatutoryReferenceByAssessmentTransactionId(assessmentEventTransactionId);
    }
  }
}
