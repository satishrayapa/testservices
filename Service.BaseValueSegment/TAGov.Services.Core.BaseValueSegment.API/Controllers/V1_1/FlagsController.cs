using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.API.Controllers.V1_1
{
  /// <summary>
  /// Base Value Segment Flags API.
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/Flags" )]
  public class FlagsController : ControllerBase
  {
    private readonly IBaseValueSegmentFlagDomain _baseValueSegmentFlagDomain;

    /// <summary>
    /// Constructor for Base Value Segment Flags API.
    /// </summary>
    /// <param name="baseValueSegmentFlagDomain"></param>
    public FlagsController( IBaseValueSegmentFlagDomain baseValueSegmentFlagDomain )
    {
      _baseValueSegmentFlagDomain = baseValueSegmentFlagDomain;
    }

    /// <summary>
    /// Gets BaseValueSegment DTO associated with the RevenueObjectId and AssessmentEventDate
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <returns>Base Value Segment Flags associated with the Revenue Object Identifier.</returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}" )]
    [ProducesResponseType( typeof( BaseValueSegmentFlagDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId )
    {
      return new ObjectResult( await _baseValueSegmentFlagDomain.ListAsync( revenueObjectId ) );
    }
  }
}
