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
  /// Base Value Segment Transaction API.
  /// </summary>
  [ApiController]
  [Authorize( Constants.HasResourceVerb )]
  [ApiVersion( "1.1" )]
  [Route( "v{version:apiVersion}/BaseValueSegments/Transactions" )]
  public class BaseValueSegmentTransactionController : ControllerBase
  {
    private const string GetRouteName = "GetTransaction";
    private readonly IBaseValueSegmentTransactionDomain _baseValueSegmentTransactionDomainDomain;

    /// <summary>
    /// Constructor for Base Value Segment Transaction API.
    /// </summary>
    /// <param name="baseValueSegmentTransactionDomain"></param>
    public BaseValueSegmentTransactionController( IBaseValueSegmentTransactionDomain baseValueSegmentTransactionDomain )
    {
      _baseValueSegmentTransactionDomainDomain = baseValueSegmentTransactionDomain;
    }

    /// <summary>
    /// Creates a BaseValueSegment Transaction Dto.
    /// </summary>
    /// <param name="baseValueSegmentTransactionDto">baseValueSegmentTransactionDto.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType( typeof( BaseValueSegmentTransactionDto ), ( int ) HttpStatusCode.Created )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( DuplicateRecordException ), ( int ) HttpStatusCode.Conflict )]
    public async Task<IActionResult> Create( [FromBody] BaseValueSegmentTransactionDto baseValueSegmentTransactionDto )
    {
      baseValueSegmentTransactionDto = await _baseValueSegmentTransactionDomainDomain.CreateAsync( baseValueSegmentTransactionDto );

      return CreatedAtRoute( GetRouteName, new { id = baseValueSegmentTransactionDto.Id }, baseValueSegmentTransactionDto );
    }

    /// <summary>
    /// Gets BaseValueSegment DTO
    /// </summary>
    /// <param name="id">Identifier of Base Value Segment</param>
    /// <returns>Base Value Segment with the identifier specified</returns>
    [HttpGet, Route( "{id}", Name = GetRouteName )]
    [ProducesResponseType( typeof( BaseValueSegmentTransactionDto ), ( int ) HttpStatusCode.OK )]
    [ProducesResponseType( typeof( BadRequestException ), ( int ) HttpStatusCode.BadRequest )]
    [ProducesResponseType( typeof( NotFoundException ), ( int ) HttpStatusCode.NotFound )]
    public async Task<IActionResult> Get( int id )
    {
      var baseValueSegment = await _baseValueSegmentTransactionDomainDomain.GetAsync( id );

      return new ObjectResult( baseValueSegment );
    }
  }
}
