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
  /// BaseValueSegmentConclusionr API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegmentConclusions" )]
  public class BaseValueSegmentConclusionController : ControllerBase
  {
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Constructor for the Owner API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public BaseValueSegmentConclusionController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Get BaseValueSegmentConclusion DTO associated with the RevenueObjectId and EffectiveDate
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/EffectiveDate/{effectiveDate}" )]
    [ProducesResponseType( typeof( BaseValueSegmentConclusionDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetBaseValueSegmentConclusions( int revenueObjectId, DateTime effectiveDate )
    {
      var baseValueSegmentConclusions =
        _baseValueSegmentDomain.GetBaseValueSegmentConclusions( revenueObjectId, effectiveDate );

      return new ObjectResult( baseValueSegmentConclusions );
    }
  }
}