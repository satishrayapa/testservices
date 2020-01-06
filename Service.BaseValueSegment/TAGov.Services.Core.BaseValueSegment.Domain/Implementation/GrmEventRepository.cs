using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Common;
using TAGov.Common.Http;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class GrmEventRepository : IGrmEventRepository
  {
    private readonly IUrlService _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;
    private string Url => _applicationSettingsHelper.GrmEventServiceApiUrl;

    public GrmEventRepository(
      IUrlService applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    public async Task<GrmEventListCreateDto> CreateAsync( GrmEventListCreateDto grmEventCreateInformation )
    {
      return await _httpClientWrapper.Post<GrmEventListCreateDto>( Url,
                                                                   "v1.1/GrmEvents/CreateGrmEvents", grmEventCreateInformation );
    }

    public async void Delete( IEnumerable<int> ids )
    {
      await _httpClientWrapper.Post<bool>( Url, "v1.1/GrmEvents/GrmEventInformation", ids );
    }
  }
}
