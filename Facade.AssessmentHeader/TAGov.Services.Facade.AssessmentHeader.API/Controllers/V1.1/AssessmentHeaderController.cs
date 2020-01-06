using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.AssessmentHeader.API.Controllers.V1._1
{
  /// <summary>
  /// Assessment Header Facade Service API.
  /// </summary>
  [ApiController]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/AssessmentHeaders" )]
  public class AssessmentHeaderController : ControllerBase
  {
    private readonly IAssessmentHeaderDomain _assessmentHeaderDomain;

    /// <summary>
    /// Constructor to Assessment Event API
    /// </summary>
    /// <param name="assessmentHeaderDomain"></param>
    public AssessmentHeaderController( IAssessmentHeaderDomain assessmentHeaderDomain )
    {
      _assessmentHeaderDomain = assessmentHeaderDomain;
    }

    /// <summary>
    /// Gets Assessment Event data for the Id provided
    /// </summary>
    /// <param name="assessmentEventId"></param>
    /// <returns></returns>
    [HttpGet, Route( "{assessmentEventId}" )]
    [ProducesResponseType( typeof( Domain.Models.V1.AssessmentHeader ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> Get( int assessmentEventId )
    {
      return new ObjectResult( await _assessmentHeaderDomain.Get( assessmentEventId ) );
    }
  }
}
