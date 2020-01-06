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
  /// CaliforniaConsumerPriceIndex API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/CAConsumerPriceIndexes" )]
  public class CaliforniaConsumerPriceIndexController : ControllerBase
  {
    private readonly IAumentumDomain _aumentumDomain;

    /// <summary>
    /// Constructor for California Consumer Price Index API
    /// </summary>
    /// <param name="aumentumDomain"></param>
    public CaliforniaConsumerPriceIndexController( IAumentumDomain aumentumDomain )
    {
      _aumentumDomain = aumentumDomain;
    }

    /// <summary>
    /// Retreive all CaliforniaConsumerPriceIndexDtos
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route( "" )]
    [ProducesResponseType( typeof( CaliforniaConsumerPriceIndexDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetAllCaConsumerPriceIndexes()
    {
      var caConsumerPriceIndexes = _aumentumDomain.GetAllCaConsumerPriceIndexes();

      return new ObjectResult( caConsumerPriceIndexes );
    }

    /// <summary>
    /// Gets FactorBaseYearValueDetailDto
    /// </summary>
    /// <param name="assessmentDate"></param>
    /// <param name="baseYear"></param>
    /// <param name="baseValue"></param>
    /// <param name="assessmentEventType"></param>
    /// <returns></returns>
    [HttpGet, Route( "AssessmentDate/{assessmentDate}/BaseYear/{baseYear}/BaseValue/{baseValue}/AssessmentEventType/{assessmentEventType}" )]
    [ProducesResponseType( typeof( FactorBaseYearValueDetailDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetFactoredBasedYearValue( DateTime assessmentDate, int baseYear, decimal baseValue, int assessmentEventType )
    {
      var fbyvDetail = _aumentumDomain.GetFactoredBasedYearValue( assessmentDate, baseYear, baseValue, assessmentEventType );

      return new ObjectResult( fbyvDetail );
    }

    /// <summary>
    /// Gets FactorBaseYearValueRequestDto
    /// </summary>
    /// <param name="factorBaseYearValueRequestDtos">Search criteria for return related fbyv information.</param>
    /// <returns></returns>
    [HttpPost, Route( "" )]
    [ProducesResponseType( typeof( FactorBaseYearValueRequestDto[] ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetFactoredBasedYearValues( [FromBody] FactorBaseYearValueRequestDto[] factorBaseYearValueRequestDtos )
    {
      return new ObjectResult( _aumentumDomain.GetFactoredBasedYearValue( factorBaseYearValueRequestDtos ) );
    }
  }
}
