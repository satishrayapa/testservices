using System.Collections.Generic;
using System.Linq;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public static class Extensions
  {
    public static void PopulateEvent( this List<GrmEventInformationDto> events, BvsDto bvsDto )
    {
      var firstEventDto = events.FirstOrDefault();

      // grab first event defined for bvs, per meeting all owners will have same events
      // this value is the event defined for the header of the previous event
      if ( firstEventDto != null )
      {
        bvsDto.EventName = firstEventDto.Description;
        bvsDto.EventType = firstEventDto.EventType;
        bvsDto.EventDate = firstEventDto.EventDate;
        bvsDto.EffectiveDate = firstEventDto.EffectiveDate;
      }
      else
      {
        bvsDto.EventName = Constants.EventUnknownName;
        bvsDto.EventType = Constants.EventUnknownName;
        bvsDto.EventDate = null;
        bvsDto.EffectiveDate = null;
      }
    }

    public static void PopulateEvent( this List<GrmEventInformationDto> events, OwnerDto ownerDto, int? grmEventId )
    {
      // per meeting  with bob do not error out if grm even information is missing, return unknown
      var grmEventInformationDto = events.FirstOrDefault( grm => grm.GrmEventId == grmEventId );

      if ( grmEventInformationDto != null )
      {
        ownerDto.EventName = grmEventInformationDto.Description;
        ownerDto.EventType = grmEventInformationDto.EventType;
        ownerDto.EventDate = grmEventInformationDto.EffectiveDate;
      }
      else
      {
        ownerDto.EventName = Constants.EventUnknownName;
        ownerDto.EventType = Constants.EventUnknownName;
        ownerDto.EventDate = null;
      }
    }

    public static void PopulateEvent( this List<GrmEventInformationDto> events, OwnerValueDto ownerValueDto, int? grmEventId )
    {
      // per meeting  with bob do not error out if grm even information is missing, return unknown
      var grmEventInformationDto = events.FirstOrDefault( grm => grm.GrmEventId == grmEventId );

      if ( grmEventInformationDto != null )
      {
        ownerValueDto.EventName = grmEventInformationDto.Description;
        ownerValueDto.EventType = grmEventInformationDto.EventType;
        ownerValueDto.EventDate = grmEventInformationDto.EffectiveDate;
      }
      else
      {
        ownerValueDto.EventName = Constants.EventUnknownName;
        ownerValueDto.EventType = Constants.EventUnknownName;
        ownerValueDto.EventDate = null;
      }
    }

    public static BaseValueSegmentTransactionDto FirstTransaction( this BaseValueSegmentDto baseValueSegmentDto )
    {
      // sort transactions by id desc, we are going to pop the first two, first will be current next will be previous
      return baseValueSegmentDto.BaseValueSegmentTransactions.OrderByDescending( t => t.Id ).First();
    }

    public static BvsOwnerDto ToBvsOwnerDto( this BaseValueSegmentDto baseValueSegmentDto )
    {
      return new BvsOwnerDto { Source = baseValueSegmentDto };
    }

    public static void PopulateOwner( this List<LegalPartyDocumentDto> documents, OwnerDto ownerDto, BaseValueSegmentOwnerDto bvsOwner )
    {
      var allTheOwnersDocuments = documents.Where( x => x.LegalPartyRoleId == bvsOwner.LegalPartyRoleId );

      var ownerSpecificEventDocument = allTheOwnersDocuments.SingleOrDefault( x => x.GrmEventId == bvsOwner.GRMEventId );

      if ( ownerSpecificEventDocument != null )
      {
        ownerDto.BeneficialInterest = ownerSpecificEventDocument.LegalPartyDisplayName;
        ownerDto.DocNumber = ownerSpecificEventDocument.DocNumber;
        ownerDto.DocType = ownerSpecificEventDocument.DocType;
        ownerDto.PercentageInterestGain = ownerSpecificEventDocument.PctGain;
      }
      else
      {
        ownerDto.BeneficialInterest = string.Empty;
        ownerDto.DocNumber = Constants.DocumentUnknownName;
        ownerDto.DocType = string.Empty;
        ownerDto.PercentageInterestGain = null;
      }
    }

    public static OwnerDto ToOwner( this BaseValueSegmentOwnerDto owner )
    {
      return new OwnerDto
             {
               // ReSharper disable once PossibleInvalidOperationException
               OwnerId = owner.Id.Value,
               BiPercentage = owner.BeneficialInterestPercent,
               LegalPartyRoleId = owner.LegalPartyRoleId
             };
    }
  }
}