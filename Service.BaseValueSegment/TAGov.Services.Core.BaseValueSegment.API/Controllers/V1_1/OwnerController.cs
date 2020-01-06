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
  /// Beneficial Interest and Owner API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/Owners" )]
  public class OwnerController : ControllerBase
  {
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Constructor for the Owner API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public OwnerController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Get BeneficialInterestEvent DTO associated with the RevenueObjectId and AsOfDate
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="asOfDate"></param>
    /// <returns></returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/AsOfDate/{asOfDate}" )]
    [ProducesResponseType( typeof( BeneficialInterestEventDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      var beneficialInterests = _baseValueSegmentDomain.GetBeneficialInterestsByRevenueObjectId( revenueObjectId, asOfDate );

      return new ObjectResult( beneficialInterests );
    }
  }
}
