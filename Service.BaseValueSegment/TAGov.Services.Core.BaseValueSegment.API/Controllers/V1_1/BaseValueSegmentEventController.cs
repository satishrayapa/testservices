using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.API.Controllers.V1_1
{
  /// <summary>
  /// Base Value Segment Event API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegmentEvents" )]
  public class BaseValueSegmentEventController : ControllerBase
  {
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Constructor for Base Value Segment Event API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public BaseValueSegmentEventController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Get a list of BaseValueSegmentEvent DTO associated with the RevenueObjectId
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <returns>Base Value Segment Events associated with the RevenueObject identifier</returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}" )]
    [ProducesResponseType( typeof( BaseValueSegmentEventDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    public IActionResult GetBvsEventsByRevenueObjectId( int revenueObjectId )
    {
      var baseValueSegment = _baseValueSegmentDomain.GetBvsEventsByRevenueObjectId( revenueObjectId );

      return new ObjectResult( baseValueSegment );
    }

  }
}
