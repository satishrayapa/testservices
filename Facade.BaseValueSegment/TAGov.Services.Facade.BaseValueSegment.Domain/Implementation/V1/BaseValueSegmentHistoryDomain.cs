using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;
using SitusAddressDto = TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.SitusAddressDto;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class BaseValueSegmentHistoryDomain : IBaseValueSegmentHistoryDomain
  {
    private readonly IBaseValueSegmentRepository _baseValueSegmentRepository;
    private readonly IGrmEventRepository _grmEventRepository;
    private readonly ILegalPartyRepository _legalPartyRepository;
    private readonly IRevenueObjectRepository _revenueObjectRepository;
    private readonly ILogger _logger;

    public BaseValueSegmentHistoryDomain(
      IBaseValueSegmentRepository baseValueSegmentRepository,
      IGrmEventRepository grmEventRepository,
      ILegalPartyRepository legalPartyRepository,
      IRevenueObjectRepository revenueObjectRepository,
      ILogger logger )
    {
      _baseValueSegmentRepository = baseValueSegmentRepository;
      _grmEventRepository = grmEventRepository;
      _legalPartyRepository = legalPartyRepository;
      _revenueObjectRepository = revenueObjectRepository;
      _logger = logger;
    }


    public async Task<IEnumerable<BvsHistoryDetailDto>> GetBaseValueSegmentHistoryAsync( string pin, DateTime fromDate,
                                                                                         DateTime toDate )
    {
      List<BvsHistoryDetailDto> bvsHistoryDetails = new List<BvsHistoryDetailDto>();

      var revenueObject = _revenueObjectRepository.GetByPin( pin ).Result;
      if ( revenueObject == null )
      {
        return bvsHistoryDetails;
      }

      var revenueObjectId = revenueObject.Id;

      //Retrieve core Base Value Segment History entities which contains id's required to retrieve all details for 
      //Base Value Segment HistoryDetail
      var baseValueSegmentHistoryDtos =
        _baseValueSegmentRepository.GetBaseValueSegmentHistory( revenueObjectId, fromDate, toDate ).Result.ToList();
      if ( baseValueSegmentHistoryDtos.Count == 0 )
      {
        return bvsHistoryDetails;
      }

      //Retrieve GrmEvent Information 
      //
      //Build GrmEvent Search object
      var grmEventSearchDto = new GrmEventSearchDto();
      var grmEventIdList = new List<int>();
      // get owner grm event ids
      var ownerGrmEventIds = baseValueSegmentHistoryDtos.Select( t => t.OwnerGrmEventId ).Distinct();
      grmEventIdList.AddRange( ownerGrmEventIds );
      // get value header grm event ids
      var valueHeaderGrmEventIds = baseValueSegmentHistoryDtos.Select( t => t.ValueHeaderGrmEventId ).Distinct();
      grmEventIdList.AddRange( valueHeaderGrmEventIds );
      if ( grmEventIdList.Count == 0 )
      {
        return bvsHistoryDetails;
      }
      grmEventSearchDto.GrmEventIdList.AddRange( grmEventIdList.Distinct() );
      // call service with search object to get grm events
      var grmEventInformationDtos = ( await _grmEventRepository.SearchAsync( grmEventSearchDto ) ).ToList();
      var baseValueSegmentEventDtos =
        _baseValueSegmentRepository.GetEventsAsync( revenueObjectId ).Result.ToList();


      //Retrieve Original BVS Event and Date
      var firstBaseValueSegmentEventDto = baseValueSegmentEventDtos.Last();
      var grmFirstEventSearchDto = new GrmEventSearchDto();
      grmFirstEventSearchDto.GrmEventIdList.Add( firstBaseValueSegmentEventDto.GRMEventId.Value );

      //Retrieve Legal Party Roles
      IEnumerable<int> legalPartyRoleIds = baseValueSegmentHistoryDtos.Select( t => t.LegalPartyRoleId ).Distinct().ToList();

      //Retrieve SubComponent Details and Market Value/Restricted Value
      var subComponentDetailDtos = new List<SubComponentDetailDto>();
      var marketAndRestrictedValueDtos = new List<MarketAndRestrictedValueDto>();
      var legalPartyDocumentDtos = new List<LegalPartyDocumentDto>();

      var asOfDates = baseValueSegmentHistoryDtos.Where( t => t.AsOf >= fromDate && t.AsOf <= toDate ).Select( t => t.AsOf ).Distinct().OrderByDescending( t => t.Date );
      foreach ( DateTime asOfDate in asOfDates )
      {
        var subComponentDetailAsOfDate = _baseValueSegmentRepository.GetSubComponentDetails( revenueObjectId, asOfDate ).Result.ToList();
        subComponentDetailDtos.AddRange( subComponentDetailAsOfDate );

        var marketAndRestrictedValueAsOfDate = ( await _revenueObjectRepository.Get( revenueObjectId, asOfDate ) ).MarketAndRestrictedValues.ToList();
        marketAndRestrictedValueDtos.AddRange( marketAndRestrictedValueAsOfDate );


        var legalPartySearchDto = new LegalPartySearchDto();
        legalPartySearchDto.LegalPartyRoleIdList.AddRange( legalPartyRoleIds );
        legalPartySearchDto.EffectiveDate = asOfDate;
        var legalPartyDocumentsAsOfDate = ( await _legalPartyRepository.SearchAsync( legalPartySearchDto ) ).ToList();
        legalPartyDocumentDtos.AddRange( legalPartyDocumentsAsOfDate );
      }

      foreach ( BaseValueSegmentHistoryDto bvsHistoryDto in baseValueSegmentHistoryDtos )
      {
        var bvsHistoryDetail = new BvsHistoryDetailDto();

        //BaseValueSegment
        //
        bvsHistoryDetail.BaseValue = bvsHistoryDto.BaseValue;
        bvsHistoryDetail.BaseYear = bvsHistoryDto.BaseYear;
        bvsHistoryDetail.BeneficialInterestPercentage = bvsHistoryDto.BeneficialInterestPercentage;
        bvsHistoryDetail.BvsTransactionType = bvsHistoryDto.BvsTransactionType;

        //LegalParty
        //
        
        var currentLegalPartyDocument =
          legalPartyDocumentDtos.FirstOrDefault( t => t.LegalPartyRoleId == bvsHistoryDto.LegalPartyRoleId );
        if ( currentLegalPartyDocument == null )
        {
          _logger.LogWarning( $"Missing LegalPartyRoleId: {bvsHistoryDto.LegalPartyRoleId}" );
          continue;
        }

        bvsHistoryDetail.BeneficialInterest = currentLegalPartyDocument.LegalPartyDisplayName;
        bvsHistoryDetail.DocumentNumber = currentLegalPartyDocument.DocNumber;
        bvsHistoryDetail.PercentInterestGained = currentLegalPartyDocument.PctGain;

        //Component 
        //Same logic as in CreateComponents for handling not finding the subcomponents
        var currentComponent = subComponentDetailDtos.FirstOrDefault( t => t.SubComponentId == bvsHistoryDto.SubComponentId );
        if ( currentComponent != null )
        {
          bvsHistoryDetail.Component = currentComponent.Component;
          bvsHistoryDetail.SubComponent = currentComponent.SubComponent;
        }
        else
        {
          bvsHistoryDetail.Component = Constants.ComponentUnknownName;
          bvsHistoryDetail.SubComponent = Constants.SubComponentUnknownName;
        }

        //Market/Restricted Value
        //Same logic as in CreateComponents for handling not finding the subcomponents
        //
        bvsHistoryDetail.MarketValue = 0;
        bvsHistoryDetail.RestrictedValue = 0;
        if ( marketAndRestrictedValueDtos.Count != 0 )
        {
          var currentMmarketAndRestrictedValue = marketAndRestrictedValueDtos.FirstOrDefault( t => t.SubComponent == bvsHistoryDto.SubComponentId );
          if ( currentMmarketAndRestrictedValue != null )
          {
            bvsHistoryDetail.MarketValue = currentMmarketAndRestrictedValue.MarketValue;
            bvsHistoryDetail.RestrictedValue = currentMmarketAndRestrictedValue.RestrictedValue;
          }
        }

        //Event Date/Type
        //if (grmEventInformationDtos.Any(t => t.GrmEventId == bvsHistoryDto.OwnerGrmEventId))
        if ( grmEventInformationDtos.Any( t => t.GrmEventId == bvsHistoryDto.ValueHeaderGrmEventId ) )
        {
          var currentGrmEventInformation = grmEventInformationDtos.First( t => t.GrmEventId == bvsHistoryDto.ValueHeaderGrmEventId );
          bvsHistoryDetail.EventDate = currentGrmEventInformation.EffectiveDate;
          bvsHistoryDetail.EventType = currentGrmEventInformation.Description;
          bvsHistoryDetail.OriginalEventDate = currentGrmEventInformation.EffectiveDate;
          bvsHistoryDetail.OriginalEventType = currentGrmEventInformation.Description;
        }
        else
        {
          var currentGrmEventInformation = grmEventInformationDtos[ 0 ];
          bvsHistoryDetail.EventDate = currentGrmEventInformation.EffectiveDate;
          bvsHistoryDetail.EventType = currentGrmEventInformation.Description;
          bvsHistoryDetail.OriginalEventDate = currentGrmEventInformation.EffectiveDate;
          bvsHistoryDetail.OriginalEventType = currentGrmEventInformation.Description;
        }

        bvsHistoryDetails.Add( bvsHistoryDetail );
      }

      return bvsHistoryDetails;
    }

    public async Task<BvsPinHistoryDto> GetBaseValueSegmentPinHistoryAsync( string pin )
    {
      var revenueObject = await _revenueObjectRepository.GetWithSitusByPin( pin );
      var tag = await _revenueObjectRepository.GetTag( revenueObject.Id );

      var status = "Unknown";

      if ( revenueObject.EffectiveStatus.ToLower() == "a" )
      {
        status = "Active";
      }
      else if ( revenueObject.EffectiveStatus.ToLower() == "i" )
      {
        status = "Inactive";
      }

      var bvsPinHistoryDto = new BvsPinHistoryDto
                             {
                               Ain = revenueObject.Ain,
                               Pin = revenueObject.Pin,
                               RevenueAccount = revenueObject.Id,
                               SitusAddress = revenueObject.SitusAddress != null
                                                ? new SitusAddressDto
                                                  {
                                                    FreeFormAddress = revenueObject.SitusAddress.FreeFormAddress,
                                                    City = revenueObject.SitusAddress.City,
                                                    StateCode = revenueObject.SitusAddress.StateCode,
                                                    PostalCode = revenueObject.SitusAddress.PostalCode
                                                  }
                                                : null,
                               Status = status,
                               Tag = tag.Description
                             };

      return bvsPinHistoryDto;
    }
  }
}