using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class BeneificialInterestBaseValueSegmentDomain : IBeneificialInterestBaseValueSegmentDomain
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IBaseValueSegmentProvider _baseValueSegmentProvider;
    private readonly IGrmEventDomain _grmEventDomain;
    private readonly ILegalPartyDomain _legalPartyDomain;
    private readonly IAssessmentEventRepository _assessmentEventRepository;

    public BeneificialInterestBaseValueSegmentDomain( IBaseValueSegmentRepository baseValueSegmentRepository,
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

    public async Task<IEnumerable<BeneficialInterestEventDto>> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOf )
    {
      return await _baseValueSegmentRepository.GetBeneficialInterestsByRevenueObjectId( revenueObjectId, asOf );
    }

    public async Task<BeneificialInterestDto> Get( int assessmentEventId )
    {
      var beneificialInterestDto = new BeneificialInterestDto();

      var results = await _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId );

      AssessmentEventDto assessmentEvent = await _assessmentEventRepository.Get( assessmentEventId );

      if ( results.Item1 != null )
        beneificialInterestDto.CurrentBaseValueSegment = await CreateReadBvsTransactionByOwners( assessmentEvent.EventDate, results.Item1, assessmentEvent.AsmtEventType );

      if ( results.Item2 != null )
        beneificialInterestDto.PreviousBaseValueSegment = await CreateReadBvsTransactionByOwners( assessmentEvent.EventDate, results.Item2, assessmentEvent.AsmtEventType );

      return beneificialInterestDto;
    }

    private async Task<BvsOwnerDto> CreateReadBvsTransactionByOwners( DateTime assessmentEventDate, BaseValueSegmentDto baseValueSegmentDto, int assessmentEventType )
    {
      var bvsDto = baseValueSegmentDto.ToBvsOwnerDto();

      // get the legal parties and document information associated to base value segment
      var legalPartyRoleDocuments = ( await _legalPartyDomain.GetLegalPartyRoleDocuments( baseValueSegmentDto ) ).ToList();

      // get the unique set of events defined for the this bvs and revenue object
      var events = ( await _grmEventDomain.GetOwnerGrmEvents( baseValueSegmentDto ) ).ToList();

      events.PopulateEvent( bvsDto );

      var firstTransaction = baseValueSegmentDto.FirstTransaction();

      // take the first transaction for the bvs
      bvsDto.Owners = await CreateOwners( firstTransaction, events, legalPartyRoleDocuments, assessmentEventDate, assessmentEventType );

      bvsDto.ValueHeaders = GetBaseValueSegmentHeaderEvents( baseValueSegmentDto, firstTransaction );

      bvsDto.BaseValueSegmentTransactionTypeDescription = firstTransaction.BaseValueSegmentTransactionType.Description;

      return bvsDto;
    }

    private async Task<OwnerDto[]> CreateOwners( BaseValueSegmentTransactionDto bvsTransaction,
                                                 List<GrmEventInformationDto> grmEventInformationDtos,
                                                 List<LegalPartyDocumentDto> legalPartyRoleDocuments,
                                                 DateTime assessmentEventDate, int assessmentEventType )
    {
      var ownerDtos = new List<OwnerDto>();

      // now build segments and base value segment per owner
      foreach ( var bvsOwner in bvsTransaction.BaseValueSegmentOwners )
      {
        // now build assessment event dto
        var owner = bvsOwner.ToOwner();

        grmEventInformationDtos.PopulateEvent( owner, bvsOwner.GRMEventId );
        legalPartyRoleDocuments.PopulateOwner( owner, bvsOwner );

        // the legal party role document may not tie back
        if ( string.IsNullOrEmpty( owner.BeneficialInterest ) )
        {
          var legalPartyRole = ( await _legalPartyDomain.GetLegalPartyRole( bvsOwner.LegalPartyRoleId, assessmentEventDate ) );

          owner.BeneficialInterest = legalPartyRole.LegalParty.DisplayName;
        }

        owner.IsOverride = bvsOwner.IsOverride;
        ownerDtos.Add( owner );

        // cast so we can add our base value segments
        var ownerEvents = new List<OwnerValueDto>();

        foreach ( var ownerValue in bvsOwner.BaseValueSegmentOwnerValueValues )
        {
          // get the values associated with the owner value
          var valueHeaderDtos = bvsTransaction.BaseValueSegmentValueHeaders.Where( v => ownerValue.BaseValueSegmentValueHeaderId == v.Id );

          foreach ( var valueHeaderDto in valueHeaderDtos )
          {
            // now build new assessment dto
            var ownerValueDto = new OwnerValueDto
                                {
                                  ValueHeaderId = ownerValue.BaseValueSegmentValueHeaderId, // NEW HOW TO SELECT ITEM IN COMBO
                                  OwnerId = bvsOwner.Id.Value,
                                  // ReSharper disable once PossibleInvalidOperationException
                                  OwnerValueId = ownerValue.Id.Value,

                                  //GrmEventId = valueHeaderDto.GRMEventId, //OLD HOW TO SELECT ITEM IN COMBO
                                  BaseYear = valueHeaderDto.BaseYear,
                                  BaseValue = decimal.Truncate( ownerValue.BaseValue )
                                };

            grmEventInformationDtos.PopulateEvent( ownerValueDto, valueHeaderDto.GRMEventId );

            var fbyvDetail = _baseValueSegmentRepository.GetFactorBaseYearValueDetail( assessmentEventDate,
                                                                                       valueHeaderDto.BaseYear, ownerValue.BaseValue, assessmentEventType );

            ownerValueDto.Fbyv = fbyvDetail.Result.Fbyv;
            ownerValueDto.IsOverride = valueHeaderDto.BaseValueSegmentValues.Any( x => x.IsOverride == true );
            ownerEvents.Add( ownerValueDto );
          }
        }

        owner.OwnerValues = ownerEvents.ToArray();
      }

      //Add default sorting 
      ownerDtos = ownerDtos.OrderBy( x => x.BeneficialInterest )
                           .ThenBy( x => x.EventDate ).ToList();

      return ownerDtos.ToArray();
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