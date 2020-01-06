using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.API.Controllers.V1._1
{
  /// <summary>
  /// Base Value Segment Facade
  /// </summary>
  [ApiController]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegments" )]
  public class BaseValueSegmentController : ControllerBase
  {
    private readonly IBeneificialInterestBaseValueSegmentDomain _beneificialInterestBaseValueSegmentDomain;
    private readonly ISubComponentBaseValueSegmentDomain _subComponentBaseValueSegmentDomain;
    private readonly IBeneificialInterestDetailBaseValueSegmentDomain _beneificialInterestDetailBaseValueSegmentDomain;
    private readonly IBaseValueSegmentDomain _baseValueSegmentDomain;
    private readonly IGrmEventDomain _grmEventDomain;
    private readonly IBaseValueSegmentHistoryDomain _baseValueSegmentHistoryDomain;
    private const string GetBasedOnAssessmentEventIdRouteName = "GetBasedOnAssessmentEventId";

    /// <summary>
    /// Constructor for BaseValueSegmentController.
    /// </summary>
    public BaseValueSegmentController(
      IBeneificialInterestBaseValueSegmentDomain beneificialInterestBaseValueSegmentDomain,
      ISubComponentBaseValueSegmentDomain subComponentBaseValueSegmentDomain,
      IBeneificialInterestDetailBaseValueSegmentDomain beneificialInterestDetailBaseValueSegmentDomain,
      IBaseValueSegmentDomain baseValueSegmentDomain,
      IGrmEventDomain grmEventDomain,
      IBaseValueSegmentHistoryDomain baseValueSegmentHistoryDomain )
    {
      _beneificialInterestBaseValueSegmentDomain = beneificialInterestBaseValueSegmentDomain;
      _subComponentBaseValueSegmentDomain = subComponentBaseValueSegmentDomain;
      _beneificialInterestDetailBaseValueSegmentDomain = beneificialInterestDetailBaseValueSegmentDomain;
      _baseValueSegmentDomain = baseValueSegmentDomain;
      _grmEventDomain = grmEventDomain;
      _baseValueSegmentHistoryDomain = baseValueSegmentHistoryDomain;
    }

    /// <summary>
    /// Gets BeneficialInterests BaseValueSegments.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route( "{id}/BeneficialInterests" )]
    [ProducesResponseType( typeof( BeneificialInterestDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetBeneficialInterestByAssessmentId( int id )
    {
      return new ObjectResult( await _beneificialInterestBaseValueSegmentDomain.Get( id ) );
    }

    /// <summary>
    /// Gets SubComponents BaseValueSegments.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route( "{id}/SubComponents" )]
    [ProducesResponseType( typeof( SubComponentDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetSubComponentByAssessmentId( int id )
    {
      return new ObjectResult( await _subComponentBaseValueSegmentDomain.Get( id ) );
    }

    /// <summary>
    /// Gets BeneficialInterestDetails BaseValueSegments.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route( "{id}/BeneficialInterestDetails" )]
    [ProducesResponseType( typeof( BeneificialInterestDetailDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetBeneficialInterestDetailByAssessmentId( int id )
    {
      return new ObjectResult( await _beneificialInterestDetailBaseValueSegmentDomain.Get( id ) );
    }

    /// <summary>
    /// Get BeneficialInterests By RevenueObjectId.
    /// </summary>
    /// <param name="revenueObjectid"></param>
    /// <param name="asOf"></param>
    /// <returns></returns>
    [HttpGet, Route( "RevenueObjectId/{revenueObjectId}/AsOf/{asOf}" )]
    [ProducesResponseType( typeof( BeneficialInterestEventDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> GetBeneficialInterestsByRevenueObjectId( int revenueObjectid, DateTime asOf )
    {
      return new ObjectResult( await _beneificialInterestBaseValueSegmentDomain.GetBeneficialInterestsByRevenueObjectId( revenueObjectid, asOf ) );
    }

    /// <summary>
    /// Saves a base value segment.
    /// </summary>
    /// <param name="baseValueSegmentId"></param>
    /// <param name="assessmentEventId"></param>
    /// <param name="baseValueSegmentDto"></param>
    /// <returns></returns>
    [HttpPost, Route( "{baseValueSegmentId}/AssessmentEventId/{assessmentEventId}" )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( ApiExceptionMessage ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> Save( int baseValueSegmentId, int assessmentEventId, [FromBody] BaseValueSegmentDto baseValueSegmentDto )
    {
      baseValueSegmentDto = await _baseValueSegmentDomain.SaveAsync( assessmentEventId, baseValueSegmentDto );

      return CreatedAtRoute( GetBasedOnAssessmentEventIdRouteName, new { baseValueSegmentId, assessmentEventId }, baseValueSegmentDto );
    }

    /// <summary>
    /// Gets BaseValueSegment DTO
    /// </summary>
    /// <param name="baseValueSegmentId">Identifier of Base Value Segment</param>
    /// <returns>Base Value Segment with the identifier specified</returns>
    [HttpGet, Route( "{baseValueSegmentId}", Name = GetBasedOnAssessmentEventIdRouteName )]
    [ProducesResponseType( typeof( BaseValueSegmentDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> Get( int baseValueSegmentId )
    {
      return new ObjectResult( await _baseValueSegmentDomain.GetAsync( baseValueSegmentId ) );
    }

    /// <summary>
    /// Creates a GRM Event for BVS Value Header Override 
    /// </summary>
    /// <param name="revenueObjectId"></param>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    [HttpPost, Route( "RevenueObjectId/{revenueObjectId}/EffectiveDate/{effectiveDate}/CreateBvsValueHdrOvrGrmEvent" )]
    [ProducesResponseType( typeof( GrmEventDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( DuplicateRecordException ), ( int ) HttpStatusCode.Conflict )]
    public async Task<IActionResult> CreateBvsValueHeaderOverideGrmEvent( int revenueObjectId, DateTime effectiveDate )
    {
      return new ObjectResult( await _grmEventDomain.CreateBvsValueHeaderOverideGrmEvent( revenueObjectId, effectiveDate ) );
    }

    /// <summary>
    /// Gets the BVS History for the specified Revenue Object within the specified date range
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    [HttpGet, Route( "Pin/{pin}/FromDate/{fromDate}/ToDate/{toDate}/BvsHistory" )]
    [ProducesResponseType( typeof( BvsHistoryDetailDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    public async Task<IActionResult> GetBaseValueSegmentHistoryDetail( string pin, DateTime fromDate, DateTime toDate )
    {
      return new ObjectResult( await _baseValueSegmentHistoryDomain.GetBaseValueSegmentHistoryAsync( pin, fromDate, toDate ) );
    }

    /// <summary>
    /// Gets the BVS History for the specified Revenue Object within the specified date range
    /// </summary>
    /// <param name="pin"></param>
    /// <returns></returns>
    [HttpGet, Route( "pin/{pin}/PinHistory" )]
    [ProducesResponseType( typeof( BvsHistoryDetailDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    public async Task<IActionResult> GetBaseValuePinHistory( string pin )
    {
      return new ObjectResult( await _baseValueSegmentHistoryDomain.GetBaseValueSegmentPinHistoryAsync( pin ) );
    }
  }
}