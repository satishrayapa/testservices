using System;
using System.Collections.Generic;
using System.Linq;
using TAGov.Common;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.GrmEvent.Domain.Constants;
using TAGov.Services.Core.GrmEvent.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Domain.Mapping;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Repository.Interfaces;

namespace TAGov.Services.Core.GrmEvent.Domain.Implementation
{
  public class GrmEventDomain : IGrmEventDomain
  {
    private readonly IGrmEventRepository _grmEventRepository;

    public GrmEventDomain( IGrmEventRepository grmEventRepository )
    {
      _grmEventRepository = grmEventRepository;
    }

    public GrmEventDto Get( int id )
    {
      id.ThrowBadRequestExceptionIfInvalid( "GrmEventId" );

      var grmEvent = _grmEventRepository.Get( id );

      grmEvent.ThrowRecordNotFoundExceptionIfNull( new IdInfo( "GrmEventId", id ) );

      return grmEvent.ToDomain();
    }

    public IEnumerable<GrmEventInformationDto> GetGrmEventInfo( int[] grmEventIdList )
    {
      if ( grmEventIdList.ToList().Any( id => id < 1 ) )
        throw new BadRequestException( $"grmEventIdList {string.Join( ",", grmEventIdList )} are invalid." );

      var grmEventRepoModels = _grmEventRepository.GetGrmEventInfo( grmEventIdList ).ToList();

      if ( grmEventRepoModels.Count == 0 )
      {
        throw new RecordNotFoundException( "", typeof( Repository.Models.V1.GrmEvent ),
                                           $"The grmEventIdList {string.Join( ",", grmEventIdList )} does not contain any valid Ids." );
      }

      return grmEventRepoModels.ToDomain();
    }

    public IEnumerable<GrmEventInformationDto> GetGrmEventInfoByRevObjIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfInvalid( "RevenueObjectId" );

      var grmEventInformation = _grmEventRepository.GetGrmEventInfoByRevObjIdAndEffectiveDate( revenueObjectId, effectiveDate );

      if ( grmEventInformation == null )
        throw new RecordNotFoundException( revenueObjectId.ToString(), typeof( Repository.Models.V1.GrmEventInformation ),
                                           string.Format( "revenueObjectId {0} is missing.", revenueObjectId ) );

      return grmEventInformation.ToDomain();
    }

    public IEnumerable<SubComponentValueDto> GetSubComponentValues( string pin, DateTime effectiveDate )
    {
      return _grmEventRepository.GetSubComponentValues( pin, effectiveDate ).Select( x => x.ToDomain() );
    }

    public GrmEventListCreateDto CreateGrmEvents( GrmEventListCreateDto grmEventListCreate )
    {
      var grmEventComponentCreateListDto = new List<GrmEventComponentCreateDto>();

      foreach ( var grmEventCreate in grmEventListCreate.GrmEventList )
      {
        grmEventCreate.RevenueObjectId.ThrowBadRequestExceptionIfInvalid( "RevenueObjectId" );

        var grmEventComponentDto = HydrateGrmEventRelatedEntities( grmEventCreate.RevenueObjectId, grmEventCreate.EffectiveDateTime );

        grmEventComponentCreateListDto.Add( new GrmEventComponentCreateDto
                                            {
                                              GrmEventComponentDto = grmEventComponentDto,
                                              GrmEventCreateDto = grmEventCreate
                                            } );
      }

      try
      {
        var grmEventCreateListDto = _grmEventRepository.CreateGrmEvents( grmEventComponentCreateListDto.ToEntity() ).ToDomain().ToList();
        return new GrmEventListCreateDto
               {
                 GrmEventList = grmEventCreateListDto
               };
      }
      catch ( Exception e )
      {
        throw new InternalServerErrorException( e.Message );
      }
    }

    public void Delete( int grmEventId )
    {
      _grmEventRepository.Delete( grmEventId );
    }

    private GrmEventComponentDto HydrateGrmEventRelatedEntities( int revenueObjectId, DateTime effectiveDate )
    {
      DateTime currentDate = DateTime.Today;

      GrmEventDto grmEvent = new GrmEventDto
                             {
                               BillNumber = string.Empty,
                               BillTypeId = 0,
                               EffDate = effectiveDate,
                               EffTaxYear = 0,
                               EventDate = currentDate,
                               EventType = GrmConstants.EventBvsValueHeaderOverride,
                               EventTypeShortDescription = string.Empty,
                               GRMModule = GrmConstants.ModuleAA,
                               Info = string.Empty,
                               PIN = string.Empty,
                               RevObjId = revenueObjectId
                             };

      GrmEventGroupDto grmEventGroup = new GrmEventGroupDto()
                                       {
                                         DocNumber = string.Empty,
                                         EndDate = currentDate,
                                         EventGroupType = GrmConstants.EventGroupBvsMaintenance,
                                         Info = string.Empty,
                                         ParentGRMEVentGroupId = 0,
                                         StartDate = currentDate
                                       };

      TransactionDetailDto transactionDetail = new TransactionDetailDto
                                               {
                                                 ObjectType = 0
                                               };

      TransactionHeaderDto transactionHeader = new TransactionHeaderDto
                                               {
                                                 ChangeTimestamp = currentDate,
                                                 CallingFunction = GrmConstants.CallingFunction,
                                                 EffDate = effectiveDate,
                                                 EffTaxYear = 0,
                                                 IPAddr = "", //this.getIpAddr(), //There might not be any value in getting the IP Address as this will be the IP of the service
                                                 MACAddr = "", //this.getMacAddr(), //There might not be any value in getting the MAC as this will be the MAC of the machine
                                                 StartTimestamp = currentDate,
                                                 TaskName = GrmConstants.TaskName,
                                                 TranId = 0,
                                                 TranType = 0,
                                                 UserProfileId = 0, //TODO - When Security implementation is complete
                                                 WorkstationId = Environment.MachineName,
                                               };

      GrmEventComponentDto grmEventComponent = new GrmEventComponentDto()
                                               {
                                                 GrmEvent = grmEvent,
                                                 GrmEventGroup = grmEventGroup,
                                                 TransactionDetail = transactionDetail,
                                                 TransactionHeader = transactionHeader
                                               };

      return grmEventComponent;
    }

    // Kept for reference only. There might not be any value in getting the IP Address as this will be the IP of the service
    //private string getIpAddr()
    //{
    //	string ipAddress = string.Empty;

    //	string hostName = Dns.GetHostName();
    //	IPHostEntry ipEntry = Dns.GetHostEntryAsync(hostName).Result;
    //	IPAddress[] ipAddresses = ipEntry.AddressList;
    //	if (ipAddresses.Length > 0)
    //	{
    //		ipAddress = ipAddresses[0].ToString();
    //	}

    //	return ipAddress;
    //}

    //private string getMacAddr()
    //{
    //	return "";
    //}


  }
}
