using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class GrmEventDomain : IGrmEventDomain
  {

    private readonly IGrmEventRepository _grmEventRepository;

    public GrmEventDomain( IGrmEventRepository grmEventRepository )
    {
      _grmEventRepository = grmEventRepository;
    }

    public async Task<IEnumerable<GrmEventInformationDto>> GetOwnerGrmEvents( BaseValueSegmentDto baseValueSegmentDto )
    {
      // build search object
      var grmEventSearchDto = new GrmEventSearchDto();
      var idList = new List<int>();

      var firstTransaction = baseValueSegmentDto.FirstTransaction();

      // get owner grm event ids
      var owners = firstTransaction.BaseValueSegmentOwners;

      // ReSharper disable once PossibleInvalidOperationException
      var grmEventIds = owners.Where( o => o.GRMEventId.HasValue ).Select( owner => owner.GRMEventId.Value ).Distinct();
      idList.AddRange( grmEventIds );

      // get header grm event ids
      var headerValuesGrmEventIds = firstTransaction.BaseValueSegmentValueHeaders.Where( hv => hv.GRMEventId.HasValue ).Select( hv => hv.GRMEventId.Value ).Distinct();
      idList.AddRange( headerValuesGrmEventIds );

      grmEventSearchDto.GrmEventIdList.AddRange( idList.Distinct() );

      // call service with search object to get grm event associated to this base value segment
      var eventInformationDtos = _grmEventRepository.SearchAsync( grmEventSearchDto ).Result;

      var effectiveDate = baseValueSegmentDto.AsOf;
      var revenueObjectId = baseValueSegmentDto.RevenueObjectId;

      // call service with search object to get legal parties associated to this base value segment
      var grmEventInformationDtos = await _grmEventRepository.GetAsync( revenueObjectId, effectiveDate );

      // return unique set
      return eventInformationDtos.Union( grmEventInformationDtos, new GrmEventInformationDtoComparer() ).ToArray();
    }

    public async Task<IEnumerable<GrmEventInformationDto>> GetValueHeaderGrmEvents( BaseValueSegmentDto baseValueSegmentDto )
    {
      // build search object
      var grmEventSearchDto = new GrmEventSearchDto();
      // get header grm event ids
      var headerValuesGrmEventIds = baseValueSegmentDto.BaseValueSegmentTransactions.SelectMany( t => t.BaseValueSegmentValueHeaders ).Where( hv => hv.GRMEventId.HasValue ).Select( hv => hv.GRMEventId.Value ).Distinct();
      grmEventSearchDto.GrmEventIdList.AddRange( headerValuesGrmEventIds );

      // call service with search object to get grm event associated to this base value segment
      return await _grmEventRepository.SearchAsync( grmEventSearchDto );
    }

    public async Task<GrmEventDto> CreateBvsValueHeaderOverideGrmEvent( int revenueObjectId, DateTime effectiveDate )
    {
      DateTime currentDate = DateTime.Today;

      GrmEventDto grmEvent = new GrmEventDto
                             {
                               BillNumber = string.Empty,
                               BillTypeId = 0,
                               EffDate = effectiveDate,
                               EffTaxYear = 0,
                               EventDate = currentDate,
                               EventType = Constants.EventBvsValueHeaderOverride,
                               EventTypeShortDescription = string.Empty,
                               GRMModule = Constants.ModuleAA,
                               Info = string.Empty,
                               PIN = string.Empty,
                               RevObjId = revenueObjectId
                             };

      GrmEventGroupDto grmEventGroup = new GrmEventGroupDto()
                                       {
                                         DocNumber = string.Empty,
                                         EndDate = currentDate,
                                         EventGroupType = Constants.EventGroupBvsMaintenance,
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
                                                 CallingFunction = Constants.CallingFunction,
                                                 EffDate = effectiveDate,
                                                 EffTaxYear = 0,
                                                 IPAddr = GetIpAddr(),
                                                 MACAddr = GetMacAddr(), //TODO - Retrieval of MAC address 
                                                 StartTimestamp = currentDate,
                                                 TaskName = Constants.TaskName,
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

      return await _grmEventRepository.CreateGrmEvent( grmEventComponent );
    }

    private string GetIpAddr()
    {
      string ipAddress = string.Empty;

      string hostName = Dns.GetHostName();
      IPHostEntry ipEntry = Dns.GetHostEntryAsync( hostName ).Result;
      IPAddress[] ipAddresses = ipEntry.AddressList;
      if ( ipAddresses.Length > 0 )
      {
        ipAddress = ipAddresses[ 0 ].ToString();
      }

      return ipAddress;
    }

    private string GetMacAddr()
    {
      return "";
    }
  }
}