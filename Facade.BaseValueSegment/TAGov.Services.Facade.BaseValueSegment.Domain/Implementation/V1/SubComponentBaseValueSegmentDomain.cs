using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class SubComponentBaseValueSegmentDomain : ISubComponentBaseValueSegmentDomain
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IBaseValueSegmentProvider _baseValueSegmentProvider;
    private readonly IGrmEventDomain _grmEventDomain;
    private readonly ILegalPartyDomain _legalPartyDomain;
    private readonly IAssessmentEventRepository _assessmentEventRepository;

    public SubComponentBaseValueSegmentDomain( IBaseValueSegmentRepository baseValueSegmentRepository,
                                               IBaseValueSegmentProvider baseValueSegmentProvider,
                                               IGrmEventDomain grmEventDomain,
                                               ILegalPartyDomain legalPartyDomain,
                                               IAssessmentEventRepository assessmentEventRepository )
    {
      _baseValueSegmentRepository = baseValueSegmentRepository;
      _baseValueSegmentProvider = baseValueSegmentProvider;
      _grmEventDomain = grmEventDomain;
      _legalPartyDomain = legalPartyDomain;
      _assessmentEventRepository = assessmentEventRepository;
    }

    public async Task<SubComponentDto> Get( int assessmentEventId )
    {
      var subComponentDto = new SubComponentDto();
      var results = await _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId );

      var assessmentEvent = await _assessmentEventRepository.Get( assessmentEventId );

      if ( results.Item1 != null )
        subComponentDto.CurrentBaseValueSegment = await CreateReadBvsTransactionByComponents( assessmentEvent.EventDate, results.Item1, assessmentEvent.AsmtEventType );

      if ( results.Item2 != null )
        subComponentDto.PreviousBaseValueSegment = await CreateReadBvsTransactionByComponents( assessmentEvent.EventDate, results.Item2, assessmentEvent.AsmtEventType );

      return subComponentDto;
    }

    private async Task<BvsComponenDto> CreateReadBvsTransactionByComponents( DateTime assessmentEventDate, BaseValueSegmentDto baseValueSegmentDto, int assessmentEventType )
    {
      var bvsDto = new BvsComponenDto { Source = baseValueSegmentDto };

      // get the events defined for the this bvs based on the SubComponentand revenue object
      var events = ( await _grmEventDomain.GetValueHeaderGrmEvents( baseValueSegmentDto ) ).ToList();
      var conclusionHeaderEvents = _baseValueSegmentRepository.GetConclusionsData( baseValueSegmentDto.RevenueObjectId, baseValueSegmentDto.AsOf ).Result;

      foreach ( var grmEvent in events )
      {
        var conclusionEvent = conclusionHeaderEvents.FirstOrDefault( conclusion => conclusion.GrmEventId == grmEvent.GrmEventId );
        if ( conclusionEvent != null )
        {
          grmEvent.EventType = conclusionEvent.Description;
        }
      }

      events.PopulateEvent( bvsDto );

      // take the first transaction for the bvs
      var firstTransaction = baseValueSegmentDto.FirstTransaction();

      bvsDto.Components = CreateComponents( firstTransaction, events, baseValueSegmentDto.AsOf, baseValueSegmentDto.RevenueObjectId, assessmentEventDate, assessmentEventType );

      bvsDto.ValueHeaders = GetBaseValueSegmentHeaderEvents( baseValueSegmentDto, firstTransaction );

      bvsDto.BaseValueSegmentTransactionTypeDescription = firstTransaction.BaseValueSegmentTransactionType.Description;

      // get the legal parties and document information associated to base value segment
      var legalPartyRoleDocuments = ( await _legalPartyDomain.GetLegalPartyRoleDocuments( baseValueSegmentDto ) ).ToList();

      bvsDto.Owners = firstTransaction.BaseValueSegmentOwners.ToList().Select( bvsOwner =>
                                                                               {
                                                                                 var doc = legalPartyRoleDocuments.First( d => d.LegalPartyRoleId == bvsOwner.LegalPartyRoleId );

                                                                                 // We are only populating 3 fields for the sub components because only the 3 fields are used for
                                                                                 // determing what's the base value, market values for each BI based on the ordering of these 3 
                                                                                 // fields. These values are used to reallocate base values/ market values if they are changed by 
                                                                                 // the user in the UI.
                                                                                 return new OwnerDto
                                                                                        {
                                                                                          // We must have an Owner Id.
                                                                                          // ReSharper disable once PossibleInvalidOperationException
                                                                                          OwnerId = bvsOwner.Id.Value,
                                                                                          BeneficialInterest = doc.LegalPartyDisplayName,
                                                                                          BiPercentage = bvsOwner.BeneficialInterestPercent

                                                                                        };
                                                                               } ).ToArray();

      return bvsDto;
    }

    private ComponentDto[] CreateComponents( BaseValueSegmentTransactionDto bvsTransaction,
                                             List<GrmEventInformationDto> grmEventInformationDtos,
                                             DateTime baseValueSegmentAsOfDate,
                                             int baseValueSegmentRevObjId,
                                             DateTime assessmentEventDate,
                                             int assessmentEventType )
    {
      var componentDtos = new List<ComponentDto>();

      // now build segments and base value segment per subComponents
      foreach ( var bvsValueHeader in bvsTransaction.BaseValueSegmentValueHeaders )
      {
        // per meeting  with bob do not error out if grm even information is missing, return unknown
        var grmEventInformationDto = grmEventInformationDtos.FirstOrDefault( grm => grm.GrmEventId == bvsValueHeader.GRMEventId );
        if ( grmEventInformationDto == null )
        {
          continue;
        }

        grmEventInformationDto.RevenueObjectId = baseValueSegmentRevObjId;

        var eventName = grmEventInformationDto.Description;
        var eventType = grmEventInformationDto.EventType;
        var eventDate = string.Format( "{0:d}", grmEventInformationDto.EventDate );
        var effectiveDate = string.Format( "{0:d}", grmEventInformationDto.EffectiveDate );

        var subComponentDetailDtos = _baseValueSegmentRepository.GetSubComponentDetails( baseValueSegmentRevObjId, baseValueSegmentAsOfDate ).Result.ToList();

        // Build Component dto
        var component = new ComponentDto
                        {
                          EventName = eventName,
                          EventType = eventType,
                          EventDate = eventDate,
                          EffectiveDate = effectiveDate,
                          EventId = bvsValueHeader.GRMEventId,
                          BaseYear = bvsValueHeader.BaseYear,
                          FbyvAsOfYear = ( grmEventInformationDto.EventDate.Month < 7 ) ? grmEventInformationDto.EventDate.Year : grmEventInformationDto.EventDate.Year + 1,
                          ValueHeaderId = bvsValueHeader.Id
                        };

        // Build Component Details
        var componentDetails = new List<ComponentDetailDto>();
        foreach ( var value in bvsValueHeader.BaseValueSegmentValues )
        {
          var subComponentDetail = subComponentDetailDtos.FirstOrDefault( x => x.SubComponentId == value.SubComponent );

          int componentId = -1;
          int subComponentId = -1;
          string componentName = Constants.ComponentUnknownName;
          string subComponentName = Constants.SubComponentUnknownName;

          if ( subComponentDetail != null )
          {
            componentId = subComponentDetail.ComponentTypeId;
            subComponentId = subComponentDetail.SubComponentId;
            componentName = subComponentDetail.Component;
            subComponentName = subComponentDetail.SubComponent;
          }


          // build new component detail dto
          var componentDetail = new ComponentDetailDto
                                {
                                  ComponentId = componentId,
                                  Component = componentName,
                                  SubComponentId = subComponentId,
                                  SubComponent = subComponentName,
                                  BaseValue = decimal.Truncate( value.ValueAmount ),
                                  // ReSharper disable once PossibleInvalidOperationException
                                  ValueId = value.Id.Value
                                };
          var fbyvDetail = _baseValueSegmentRepository.GetFactorBaseYearValueDetail( assessmentEventDate,
                                                                                     bvsValueHeader.BaseYear, value.ValueAmount, assessmentEventType );

          componentDetail.Fbyv = fbyvDetail.Result.Fbyv;
          componentDetail.IsOverride = value.IsOverride;
          componentDetails.Add( componentDetail );
        }

        component.IsOverride = bvsValueHeader.BaseValueSegmentValues.All( x => x.IsOverride == true );
        component.ComponentDetails = componentDetails;
        componentDtos.Add( component );
      }

      //Add default sorting 
      componentDtos = componentDtos.OrderBy( x => x.BaseYear )
                                   .ThenBy( x => x.EffectiveDate ).ToList();

      return componentDtos.ToArray();
    }

    private HeaderValue[] GetBaseValueSegmentHeaderEvents( BaseValueSegmentDto baseValueSegmentDto, BaseValueSegmentTransactionDto transaction )
    {
      var headerRelatedEvents = _grmEventDomain.GetValueHeaderGrmEvents( baseValueSegmentDto ).Result.ToArray();

      var conclusionHeaderEvents = _baseValueSegmentRepository.GetConclusionsData( baseValueSegmentDto.RevenueObjectId, baseValueSegmentDto.AsOf ).Result.ToList();

      var valueHeaders = new List<HeaderValue>();

      foreach ( var baseValueSegmentValueHeaderDto in transaction.BaseValueSegmentValueHeaders )
      {
        var headerRelatedEvent = headerRelatedEvents.Single( he => he.GrmEventId == baseValueSegmentValueHeaderDto.GRMEventId );
        BaseValueSegmentConclusionDto conclusionEvent = null;

        if ( conclusionHeaderEvents.Count > 0 )
        {
          conclusionEvent = conclusionHeaderEvents.FirstOrDefault( conclusion => conclusion.GrmEventId == headerRelatedEvent.GrmEventId );
        }

        var headerValue = new HeaderValue
                          {
                            HeaderValueId = baseValueSegmentValueHeaderDto.Id,
                            GrmEventId = headerRelatedEvent.GrmEventId,
                            DisplayName = headerRelatedEvent.EventType + " " + headerRelatedEvent.EffectiveDate.Year,
                            BaseYear = baseValueSegmentValueHeaderDto.BaseYear,
                            EventDate = headerRelatedEvent.EventDate,
                            EventType = headerRelatedEvent.EventType,
                            EffectiveDate = headerRelatedEvent.EffectiveDate
                          };

        if ( conclusionEvent != null )
        {
          headerValue.DisplayName = conclusionEvent.Description + " " + conclusionEvent.ConclusionDate.Year;
          headerValue.EventType = conclusionEvent.Description;
        }

        valueHeaders.Add( headerValue );
      }

      return valueHeaders.OrderByDescending( vh => vh.BaseYear ).ToArray();
    }
  }
}