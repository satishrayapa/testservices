using System;
using System.Collections.Generic;
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
  /// Base Value Segment API
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegments" )]
  public class BaseValueSegmentController : ControllerBase
  {
    private const string GetRouteName = "Get";
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;

    /// <summary>
    /// Constructor for Base Value Segment API
    /// </summary>
    /// <param name="baseValueSegmentDomain"></param>
    public BaseValueSegmentController( IBaseValueSegmentDomain baseValueSegmentDomain )
    {
      _baseValueSegmentDomain = baseValueSegmentDomain;
    }

    /// <summary>
    /// Creates a BaseValueSegment
    /// </summary>
    /// <param name="baseValueSegment">Base Value Segment</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType( typeof( BaseValueSegmentDto ), ( int ) HttpStatusCode.Created )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( DuplicateRecordException ), ( int ) HttpStatusCode.Conflict )]
    public async Task<IActionResult> Create( [FromBody] BaseValueSegmentDto baseValueSegment )
    {
      baseValueSegment = await _baseValueSegmentDomain.CreateAsync( baseValueSegment );

      return CreatedAtRoute( GetRouteName, new { id = baseValueSegment.Id }, baseValueSegment );
    }

    /// <summary>
    /// Gets BaseValueSegment DTO
    /// </summary>
    ///// <param name="id">Identifier of Base Value Segment</param>
    /// <returns>Base Value Segment with the identifier specified</returns>
    [HttpGet, Route( "{id}", Name = GetRouteName )]
    [ProducesResponseType( typeof( BaseValueSegmentDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult Get( int id )
    {
      return new ObjectResult( _baseValueSegmentDomain.Get( id ) );
    }

    /// <summary>
    /// Gets BaseValueSegment DTO associated with the RevenueObjectId and AssessmentEventDate
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="assessmentEventDate"></param>
    /// <returns>Base Value Segment associated with the Revenue Object Identifier and Assessment Event Date</returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/AssessmentEventDate/{assessmentEventDate}" )]
    [ProducesResponseType( typeof( BaseValueSegmentDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId, DateTime assessmentEventDate )
    {
      return new ObjectResult( _baseValueSegmentDomain.GetByRevenueObjectIdAndAssessmentEventDate( revenueObjectId, assessmentEventDate ) );
    }

    /// <summary>
    /// Gets a list of BaseValueSegment DTOs based on revenueObjectId, asOf date and sequence number.
    /// </summary>
    /// <param name="revenueObjectId">Identifier of Base Value Segment</param>
    /// <param name="asOf"></param>
    /// <param name="sequenceNumber"></param>
    /// <returns>Base Value Segment with the identifier specified</returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/AsOf/{asOf}/SequenceNumber/{sequenceNumber}" )]
    [ProducesResponseType( typeof( BaseValueSegmentInfoDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetSingle( int revenueObjectId, DateTime asOf, int sequenceNumber )
    {
      return new ObjectResult( _baseValueSegmentDomain.Get( revenueObjectId, asOf, sequenceNumber ) );
    }

    /// <summary>
    /// Gets a list of BaseValueSegment DTOs based on revenueObjectId.
    /// </summary>
    /// <param name="revenueObjectId">Identifier of Base Value Segment</param>		
    /// <returns>Base Value Segment with the identifier specified</returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}" )]
    [ProducesResponseType( typeof( IEnumerable<BaseValueSegmentInfoDto> ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public IActionResult GetList( int revenueObjectId )
    {
      return new ObjectResult( _baseValueSegmentDomain.List( revenueObjectId ) );
    }
  }
}
