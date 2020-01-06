using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class GrmEventRepository : RepositoryBase, IGrmEventRepository
  {
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;
    private string Url => _applicationSettingsHelper.GrmEventServiceApiUrl;

    public GrmEventRepository(
      IApplicationSettingsHelper applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper ) : base( "v1.1", applicationSettingsHelper, httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    public async Task<IEnumerable<GrmEventInformationDto>> SearchAsync( GrmEventSearchDto grmEventSearchDto )
    {
      var grmEventInformationDtosEnumerable = await _httpClientWrapper.Post<IEnumerable<GrmEventInformationDto>>( Url,
                                                                                                                  $"{Version}/GrmEventInformation", grmEventSearchDto );

      if ( grmEventInformationDtosEnumerable == null )
        throw new RecordNotFoundException(
          string.Join( ",", grmEventSearchDto.GrmEventIdList.Select( x => x.ToString() ).ToArray() ),
          typeof( GrmEventInformationDto ), "The GRM Information could not be found GRM Event HeaderValueId List" );

      var grmEventInformationDtos = grmEventInformationDtosEnumerable.ToList();

      if ( grmEventInformationDtos.Count == 0 )
        throw new RecordNotFoundException(
          string.Join( ",", grmEventSearchDto.GrmEventIdList.Select( x => x.ToString() ).ToArray() ),
          typeof( GrmEventInformationDto ), "The GRM Information could not be found GRM Event HeaderValueId List" );

      return grmEventInformationDtos;
    }

    public async Task<IEnumerable<GrmEventInformationDto>> GetAsync( int revenueObjectId, DateTime asOf )
    {
      return ( await _httpClientWrapper.Get<IEnumerable<GrmEventInformationDto>>( Url,
                                                                                  $"{Version}/GrmEvents/GrmEventInformation/RevObjId/{revenueObjectId}/EffectiveDate/{asOf:yyyy-MM-dd}" ) ).ToList();
    }

    public async Task<GrmEventDto> CreateGrmEvent( GrmEventComponentDto grmEventComponent )
    {
      return await _httpClientWrapper.Post<GrmEventDto>( Url, $"{Version}/GrmEvents/CreateGrmEvent", grmEventComponent );
    }
  }
}