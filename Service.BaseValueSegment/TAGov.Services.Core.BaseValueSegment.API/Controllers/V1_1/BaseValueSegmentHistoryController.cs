using System;
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
  /// BaseValueSegmentHistory API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegmentHistory" )]
  public class BaseValueSegmentHistoryController : ControllerBase
  {
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Construction for the BaseValueSegmentHistory API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public BaseValueSegmentHistoryController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Get the BaseValueSegmentHistory DTO associated with the RevenueObjectId and between the specified fromDate and toDate 
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/FromDate/{fromDate}/ToDate/{toDate}" )]
    [ProducesResponseType( typeof( BaseValueSegmentHistoryDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    public IActionResult GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate, DateTime toDate )
    {
      var baseValueSegmentHistory =
        _baseValueSegmentDomain.GetBaseValueSegmentHistory( revenueObjectId, fromDate, toDate );

      return new ObjectResult( baseValueSegmentHistory );
    }
  }
}
