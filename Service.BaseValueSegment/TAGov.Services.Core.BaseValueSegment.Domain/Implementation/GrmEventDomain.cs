using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class GrmEventDomain : IGrmEventDomain
  {
    private readonly IGrmEventRepository _grmEventRepository;

    public GrmEventDomain( IGrmEventRepository grmEventRepository )
    {
      _grmEventRepository = grmEventRepository;
    }

    public async Task<int[]> Create( BaseValueSegmentDto baseValueSegmentDto )
    {
      List<int> createdEventIds = new List<int>();

      foreach ( var transaction in baseValueSegmentDto.BaseValueSegmentTransactions )
      {
        var transactionEventIds = await CreateEventForTransaction( transaction );

        createdEventIds.AddRange( transactionEventIds );
      }

      // return created information
      return createdEventIds.ToArray();
    }

    public async Task<int[]> CreateForTransaction( BaseValueSegmentTransactionDto transaction )
    {
      var transactionEventIds = await CreateEventForTransaction( transaction );

      return transactionEventIds.ToArray();
    }

    private async Task<int[]> CreateEventForTransaction( BaseValueSegmentTransactionDto transaction )
    {
      GrmEventListCreateDto eventsToBeCreated = new GrmEventListCreateDto();

      // get grm event that need to be created
      GetOwnerCreateGrmEventInformation( transaction, eventsToBeCreated );

      GetHeaderValueCreateGrmEventInformation( transaction, eventsToBeCreated );

      // call grm events to create 
      var createdEvents = await _grmEventRepository.CreateAsync( eventsToBeCreated );

      // now map back
      MapOwnerCreatedGrmEventToBvs( createdEvents, transaction );

      MapHeaderValueCreatedGrmEventToBvs( createdEvents, transaction );

      return createdEvents.GrmEventList.Select( e => e.GrmEventId ).ToArray();

    }

    private static void MapHeaderValueCreatedGrmEventToBvs( GrmEventListCreateDto createdEvents, BaseValueSegmentTransactionDto transaction )
    {
      var headerValueCreated = createdEvents.GrmEventList.Where( o => o.ParentType == GrmEventParentType.HeaderValue );

      foreach ( var headerGrmEvent in headerValueCreated )
      {
        var header = transaction.BaseValueSegmentValueHeaders.Single( h => h.Id == headerGrmEvent.ParentId );

        header.GRMEventId = headerGrmEvent.GrmEventId;
      }
    }

    private static void MapOwnerCreatedGrmEventToBvs( GrmEventListCreateDto createdEvents, BaseValueSegmentTransactionDto transaction )
    {
      var ownerCreated = createdEvents.GrmEventList.Where( o => o.ParentType == GrmEventParentType.Owner );

      foreach ( var ownerGrmEvent in ownerCreated )
      {
        var owner = transaction.BaseValueSegmentOwners.Single( o => o.Id == ownerGrmEvent.ParentId );

        owner.GRMEventId = ownerGrmEvent.GrmEventId;
      }
    }

    private static void GetHeaderValueCreateGrmEventInformation( BaseValueSegmentTransactionDto transaction, GrmEventListCreateDto eventsToBeCreated )
    {
      var valueHeadersThatNeedEvents = transaction.BaseValueSegmentValueHeaders.Where( o => o.GRMEventId == null );

      var baseValueSegmentValueHeaderDtos = valueHeadersThatNeedEvents as BaseValueSegmentValueHeaderDto[] ??
                                            valueHeadersThatNeedEvents.ToArray();

      var notValidHeaderValues = baseValueSegmentValueHeaderDtos.Where( v => v.GrmEventInformation == null );

      if ( notValidHeaderValues.Any() )
        throw new BadRequestException( "No GRMEvent Id defined and no GRMEventCreateInformation was provided." );

      foreach ( var valueHeader in baseValueSegmentValueHeaderDtos )
      {
        eventsToBeCreated.GrmEventList.Add(
          new GrmEventCreateDto
          {
            ParentId = valueHeader.Id,
            ParentType = GrmEventParentType.HeaderValue,
            EffectiveDateTime = valueHeader.GrmEventInformation.EffectiveDateTime,
            RevenueObjectId = valueHeader.GrmEventInformation.RevenueObjectId,
            EventType = valueHeader.GrmEventInformation.EventType
          }
        );
      }
    }

    private static void GetOwnerCreateGrmEventInformation( BaseValueSegmentTransactionDto transaction, GrmEventListCreateDto eventsToBeCreated )
    {
      var ownersThatNeedEvents = transaction.BaseValueSegmentOwners.Where( o => o.GRMEventId == null );

      var baseValueSegmentOwnerDtos = ownersThatNeedEvents as BaseValueSegmentOwnerDto[] ?? ownersThatNeedEvents.ToArray();

      var notValidOwners = baseValueSegmentOwnerDtos.Where( v => v.GrmEventInformation == null );

      if ( notValidOwners.Any() )
        throw new BadRequestException( "No GRMEvent Id defined and no GRMEventCreateInformation was provided." );

      foreach ( var owner in baseValueSegmentOwnerDtos )
      {
        eventsToBeCreated.GrmEventList.Add(
          new GrmEventCreateDto
          {
            // ReSharper disable once PossibleInvalidOperationException
            ParentId = owner.Id.Value,
            ParentType = GrmEventParentType.Owner,
            EffectiveDateTime = owner.GrmEventInformation.EffectiveDateTime,
            RevenueObjectId = owner.GrmEventInformation.RevenueObjectId,
            EventType = owner.GrmEventInformation.EventType
          }
        );
      }
    }

    public void Delete( int[] ids )
    {
      _grmEventRepository.Delete( ids );
    }
  }
}
