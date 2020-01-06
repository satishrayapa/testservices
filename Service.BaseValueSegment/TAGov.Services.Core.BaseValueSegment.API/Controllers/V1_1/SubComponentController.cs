using System;
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
  /// SubComponent API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/SubComponents" )]
  public class SubComponentController : ControllerBase
  {
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Constructor for the SubComponent API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public SubComponentController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Gets SubComponentDetail DTO associated with the RevenueObjectId and AsOfDate
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="asOfDate">date format yyyy-MM-dd ISO8601</param>
    /// <returns></returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/AsOfDate/{asOfDate}" )]
    [ProducesResponseType( typeof( SubComponentDetailDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetSubComponentDetailsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      return new ObjectResult( await _baseValueSegmentDomain.GetSubComponentDetailsByRevenueObjectId( revenueObjectId, asOfDate ) );
    }
  }
}
