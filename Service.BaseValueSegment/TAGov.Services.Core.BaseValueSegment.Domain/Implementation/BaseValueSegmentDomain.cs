using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class BaseValueSegmentDomain : BaseValueSegmentDomainBase, IBaseValueSegmentDomain
  {
    private readonly IBaseValueSegmentRepository _bvsRepository;
    private readonly IGrmEventDomain _grmEventDomain;

    public BaseValueSegmentDomain( IBaseValueSegmentRepository bvsRepository, IGrmEventDomain grmEventDomain )
    {
      _bvsRepository = bvsRepository;
      _grmEventDomain = grmEventDomain;
    }

    public async Task<BaseValueSegmentDto> CreateAsync( BaseValueSegmentDto baseValueSegmentDto )
    {
      if ( baseValueSegmentDto.Id.HasValue )
      {
        throw new BadRequestException( string.Format( "Id {0} is invalid.", baseValueSegmentDto.Id ) );
      }

      if ( baseValueSegmentDto.BaseValueSegmentTransactions.Count == 0 )
      {
        throw new BadRequestException( "No Base Value Segment Transaction defined for the Base Value Segment" );
      }

      foreach ( var transaction in baseValueSegmentDto.BaseValueSegmentTransactions )
      {
        if ( transaction.BaseValueSegmentOwners == null ||
             transaction.BaseValueSegmentOwners.Count == 0 )
        {
          throw new BadRequestException( "No Base Value Segment Owners defined for the Base Value Segment Transaction" );
        }

        if ( transaction.BaseValueSegmentValueHeaders == null ||
             transaction.BaseValueSegmentValueHeaders.Count == 0 )
        {
          throw new BadRequestException( "No Base Value Segment Value Headers defined for the Base Value Segment Transaction" );
        }
      }

      // We are going to create a brand new Base Value Segment. In order for EF to know,
      // we are going to set all Primary Keys to 0.

      var eventsCreated = await _grmEventDomain.Create( baseValueSegmentDto );

      try
      {
        var entity = baseValueSegmentDto.ToEntity();

        entity.Id = 0;

        var baseValueSegmentOwnerValues = new List<BaseValueSegmentOwnerValue>();

        foreach ( var baseValueSegmentTransaction in entity.BaseValueSegmentTransactions )
        {
          PrepareTransactionForCreate( baseValueSegmentTransaction, false );
        }

        foreach ( var assessmentRevisionBaseValueSegmentDto in entity.BaseValueSegmentAssessmentRevisions )
        {
          assessmentRevisionBaseValueSegmentDto.Id = 0;
          assessmentRevisionBaseValueSegmentDto.BaseValueSegmentId = 0;
        }

        foreach ( var entityBaseValueSegmentTransaction in entity.BaseValueSegmentTransactions )
        {
          DiscoverOwnerValuesForSavingInTransaction( () =>
                                                       _bvsRepository.GetUserTransactionType(),
                                                     () => _bvsRepository.GetUserDeletedTransactionType(),
                                                     entityBaseValueSegmentTransaction, baseValueSegmentOwnerValues );
        }

        foreach ( var entityBaseValueSegmentAssessmentRevision in entity.BaseValueSegmentAssessmentRevisions )
        {
          var newStatusType = _bvsRepository.GetNewStatusType();
          entityBaseValueSegmentAssessmentRevision.BaseValueSegmentStatusType = newStatusType;
          entityBaseValueSegmentAssessmentRevision.BaseValueSegmentStatusTypeId = newStatusType.Id;
        }

        // Client side code does not necessary set this value when passing over,
        // In the case of when an assessment event does not have any current BVS, then
        // this issue will be seen. We are thus covering this case on the server side.
        foreach ( var entityBaseValueSegmentTransaction in entity.BaseValueSegmentTransactions )
        {
          if ( string.IsNullOrEmpty( entityBaseValueSegmentTransaction.EffectiveStatus ) )
          {
            entityBaseValueSegmentTransaction.EffectiveStatus = "A";
          }
        }

        await _bvsRepository.CreateAsync( entity, baseValueSegmentOwnerValues );

        return entity.ToDomain();

      }
      catch
      {
        _grmEventDomain.Delete( eventsCreated );
        throw;
      }

    }

    public BaseValueSegmentDto Get( int id )
    {
      id.ThrowBadRequestExceptionIfInvalid( "BaseValueSegmentId" );

      var bvs = _bvsRepository.Get( id );
      bvs.ThrowRecordNotFoundExceptionIfNull( new IdInfo( "BaseValueSegmentId", id ) );
      return bvs.ToDomain();
    }

    public BaseValueSegmentDto GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId, DateTime assessmentEventDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      var bvs = _bvsRepository.GetByRevenueObjectIdAndAssessmentEventDate( revenueObjectId, assessmentEventDate );

      bvs.ThrowRecordNotFoundExceptionIfNull(
        new IdInfo( "revenueObjectId", revenueObjectId ),
        new IdInfo( "assessmentEventDate", assessmentEventDate ) );

      return bvs.ToDomain();
    }

    public IEnumerable<BaseValueSegmentEventDto> GetBvsEventsByRevenueObjectId( int revenueObjectId )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      var baseValueSegments = _bvsRepository.GetBvsEventsByRevenueObjectId( revenueObjectId ).ToList();
      if ( !baseValueSegments.Any() )
      {
        throw new RecordNotFoundException( revenueObjectId.ToString(),
                                           typeof( Repository.Models.V1.BaseValueSegment ),
                                           string.Format( "No Base Value Segment Event records with RevenueObjectId {0}.", revenueObjectId ) );
      }

      return baseValueSegments.Select( x => x.ToDomain() );
    }

    public async Task<IEnumerable<SubComponentDetailDto>> GetSubComponentDetailsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      var subComponentDetails = ( await _bvsRepository.GetSubComponentDetailsByRevenueObjectId( revenueObjectId, asOfDate ) ).ToList();

      if ( !subComponentDetails.Any() )
      {
        throw new RecordNotFoundException( revenueObjectId.ToString(),
                                           typeof( SubComponentDetail ),
                                           string.Format( "No SubComponent records with RevenueObjectId {0}.", revenueObjectId ) );
      }

      return subComponentDetails.ToDomain();
    }

    public IEnumerable<BeneficialInterestEventDto> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      var beneficialInterests = _bvsRepository.GetBeneficialInterestsByRevenueObjectId( revenueObjectId, asOfDate ).ToList();

      if ( !beneficialInterests.Any() )
      {
        throw new RecordNotFoundException( revenueObjectId.ToString(),
                                           typeof( BeneficialInterestEvent ),
                                           string.Format( "No Owner records with RevenueObjectId {0}.", revenueObjectId ) );
      }

      return beneficialInterests.ToDomain();
    }

    public IEnumerable<BaseValueSegmentConclusionDto> GetBaseValueSegmentConclusions( int revenueObjectId,
                                                                                      DateTime effectiveDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      IList<BaseValueSegmentConclusion> bvsConclusions = _bvsRepository
        .GetBaseValueSegmentConclusions( revenueObjectId, effectiveDate ).ToList();

      IList<BaseValueSegmentConclusionDto> bvsConclusionsDtos = bvsConclusions.ToDomain();

      return bvsConclusionsDtos;
    }

    public BaseValueSegmentInfoDto Get( int revenueObjectId, DateTime asOf, int sequenceNumber )
    {
      var item = _bvsRepository.Get( revenueObjectId, asOf, sequenceNumber );

      item.ThrowRecordNotFoundExceptionIfNull(
        new IdInfo( "revenueObjectId", revenueObjectId ),
        new IdInfo( "asOf", asOf ),
        new IdInfo( "sequenceNumber", sequenceNumber ) );

      return item.ToInfoDomain();
    }

    public IEnumerable<BaseValueSegmentInfoDto> List( int revenueObjectId )
    {
      return _bvsRepository.List( revenueObjectId ).Select( x => x.ToInfoDomain() ).ToList();
    }

    public IEnumerable<BaseValueSegmentHistoryDto> GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate,
                                                                               DateTime toDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfRevenueObjectIdInvalid();

      return _bvsRepository.GetBaseValueSegmentHistory( revenueObjectId, fromDate, toDate ).ToDomain();
    }

  }
}