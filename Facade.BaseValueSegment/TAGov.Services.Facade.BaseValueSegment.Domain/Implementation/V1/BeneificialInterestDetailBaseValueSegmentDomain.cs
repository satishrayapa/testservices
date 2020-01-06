using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class BeneificialInterestDetailBaseValueSegmentDomain : IBeneificialInterestDetailBaseValueSegmentDomain
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IBaseValueSegmentProvider _baseValueSegmentProvider;
    private readonly IGrmEventDomain _grmEventDomain;
    private readonly ILegalPartyDomain _legalPartyDomain;
    private readonly IAssessmentEventRepository _assessmentEventRepository;

    public BeneificialInterestDetailBaseValueSegmentDomain( IBaseValueSegmentRepository baseValueSegmentRepository,
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


    public async Task<BeneificialInterestDetailDto> Get( int assessmentEventId )
    {
      var beneificialInterestDetailDto = new BeneificialInterestDetailDto();

      var results = await _baseValueSegmentProvider.GetCurrentAndPrevious( assessmentEventId );

      //there may not be a base value segment--if there is not then don't get the details
      if ( results.Item1 != null )
      {
        var assessmentEvent = await _assessmentEventRepository.Get( assessmentEventId );

        beneificialInterestDetailDto.CurrentBaseValueSegment = await CreateReadBvsTransactionByDetails( assessmentEvent.EventDate, results.Item1 );
      }

      return beneificialInterestDetailDto;
    }

    private async Task<BvsDetailDto> CreateReadBvsTransactionByDetails( DateTime assessmentEventDate, BaseValueSegmentDto baseValueSegmentDto )
    {
      var bvsDto = new BvsDetailDto { Source = baseValueSegmentDto };

      // get the legal parties and document information associated to base value segment
      var legalPartyRoleDocuments = ( await _legalPartyDomain.GetLegalPartyRoleDocuments( baseValueSegmentDto ) ).ToList();

      // get the unique set of events defined for the this bvs and revenue object
      var events = ( await _grmEventDomain.GetOwnerGrmEvents( baseValueSegmentDto ) ).ToList();

      events.PopulateEvent( bvsDto );

      // take the first transaction for the bvs
      bvsDto.Details = ( await CreateDetails( baseValueSegmentDto.FirstTransaction(), events, legalPartyRoleDocuments, assessmentEventDate, baseValueSegmentDto.RevenueObjectId ) );

      return bvsDto;
    }

    private async Task<DetailDto[]> CreateDetails( BaseValueSegmentTransactionDto bvsTransaction,
                                                   List<GrmEventInformationDto> grmEventInformationDtos,
                                                   List<LegalPartyDocumentDto> legalPartyDocumentDtos,
                                                   DateTime assessmentEventDate,
                                                   int baseValueSegmentRevObjId )
    {
      var list = new List<DetailDto>();

      // now build segments and base value segment per owner
      foreach ( var bvsOwner in bvsTransaction.BaseValueSegmentOwners )
      {
        // per meeting  with bob do not error out if grm even information is missing, return unknown
        var grmOwnerEventInformationDto = grmEventInformationDtos.FirstOrDefault( grm => grm.GrmEventId == bvsOwner.GRMEventId );

        DateTime? ownerEventDate = null;
        string ownerEventType;
        string ownerEventName;

        if ( grmOwnerEventInformationDto != null )
        {
          ownerEventName = grmOwnerEventInformationDto.Description;
          ownerEventType = grmOwnerEventInformationDto.EventType;
          ownerEventDate = grmOwnerEventInformationDto.EffectiveDate;
        }
        else
        {
          ownerEventName = Constants.EventUnknownName;
          ownerEventType = Constants.EventUnknownName;
        }

        var allTheOwnersDocuments = legalPartyDocumentDtos.Where( x => x.LegalPartyRoleId == bvsOwner.LegalPartyRoleId ).ToList();

        foreach ( var ownerValue in bvsOwner.BaseValueSegmentOwnerValueValues )
        {
          // get the values associated with the owner value
          var baseValueHeaders = bvsTransaction.BaseValueSegmentValueHeaders.Where( v => ownerValue.BaseValueSegmentValueHeaderId == v.Id );

          foreach ( var bvsValueHeader in baseValueHeaders )
          {

            // per meeting with bob do not error out if grm even information is missing, return unknown
            var grmValueHeaderEventInformationDto = grmEventInformationDtos.FirstOrDefault( grm => grm.GrmEventId == bvsValueHeader.GRMEventId );

            // HACK: this should set but the migration process, bob told arpan to set revobject
            // to zero for dummy grmevents created for bvs, need to discuss
            grmValueHeaderEventInformationDto.RevenueObjectId = baseValueSegmentRevObjId;

            string valueHeaderEventName;
            string valueHeaderEventType;
            DateTime? valueHeaderEventDate = null;

            if ( grmOwnerEventInformationDto != null )
            {
              valueHeaderEventName = grmValueHeaderEventInformationDto.Description;
              valueHeaderEventType = grmValueHeaderEventInformationDto.EventType;
              valueHeaderEventDate = grmValueHeaderEventInformationDto.EffectiveDate;
            }
            else
            {
              valueHeaderEventName = Constants.EventUnknownName;
              valueHeaderEventType = Constants.EventUnknownName;
            }

            var subComponentDetailDtos = _baseValueSegmentRepository.GetSubComponentDetails( baseValueSegmentRevObjId, assessmentEventDate ).Result.ToList();

            foreach ( var value in bvsValueHeader.BaseValueSegmentValues )
            {
              var subComponentDetail = subComponentDetailDtos.FirstOrDefault( x => x.SubComponentId == value.SubComponent );

              if ( subComponentDetail is null )
              {
                continue;
              }

              var ownerSpecificEventDocument = allTheOwnersDocuments.SingleOrDefault( x => x.GrmEventId == bvsOwner.GRMEventId );

              string beneficialInterest;
              string docNumber;

              decimal? percentageInterestGain = null;

              if ( ownerSpecificEventDocument != null )
              {
                beneficialInterest = ownerSpecificEventDocument.LegalPartyDisplayName;
                docNumber = ownerSpecificEventDocument.DocNumber;
                percentageInterestGain = ownerSpecificEventDocument.PctGain;
              }
              else
              {
                var legalPartyRole = ( await _legalPartyDomain.GetLegalPartyRole( bvsOwner.LegalPartyRoleId, assessmentEventDate ) );

                beneficialInterest = legalPartyRole.LegalParty.DisplayName;
                docNumber = Constants.DocumentUnknownName;
              }

              // now build dto
              var detail = new DetailDto
                           {
                             BeneficialInterest = beneficialInterest,
                             DocNumber = docNumber,
                             PercentageInterestGain = percentageInterestGain,

                             OwnershipEventName = ownerEventName,
                             OwnershipEventType = ownerEventType,
                             OwnershipEventDate = ownerEventDate,

                             BaseValueSegmentEventName = valueHeaderEventName,
                             BaseValueSegmentEventType = valueHeaderEventType,
                             BaseValueSegmentEventDate = valueHeaderEventDate,

                             BiPercentage = bvsOwner.BeneficialInterestPercent,

                             ComponentName = subComponentDetail.Component,
                             SubComponentName = subComponentDetail.SubComponent,
                             SubComponentId = subComponentDetail.SubComponentId,
                             BaseYear = bvsValueHeader.BaseYear,
                             OwnerIsOverride = bvsOwner.IsOverride,
                             ComponentIsOverride = bvsValueHeader.BaseValueSegmentValues.Any( x => x.IsOverride == true && x.SubComponent == subComponentDetail.SubComponentId ),
                             OwnerId = bvsOwner.Id,
                             AssessmentEventDate = assessmentEventDate,
                             ValueHeaderId = bvsValueHeader.Id
                           };

              list.Add( detail );
            }
          }
        }
      }

      return list.ToArray();
    }
  }
}