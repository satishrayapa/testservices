using System;
using System.Threading.Tasks;
using TAGov.Common.Http;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class RevenueObjectRepository : RepositoryBase, IRevenueObjectRepository
  {
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public RevenueObjectRepository(
      IApplicationSettingsHelper applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper ) : base( "v1.1", applicationSettingsHelper, httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    public async Task<RevenueObjectDto> Get( int revenueObjectId, DateTime effectiveDate )
    {
      return await _httpClientWrapper.Get<RevenueObjectDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl, $"{Version}/RevenueObjects/{revenueObjectId}/EffectiveDate/{effectiveDate:O}" );
    }

    public async Task<TAGDto> GetTag( int revenueObjectId )
    {
      return await _httpClientWrapper.Get<TAGDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl, $"{Version}/RevenueObjects/RevenueObjectId/{revenueObjectId}/TAG" );
    }

    public async Task<RevenueObjectDto> GetByPin( string pin )
    {
      return await _httpClientWrapper.Get<RevenueObjectDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl, $"{Version}/RevenueObjects/Pin/{pin}" );
    }

    public async Task<RevenueObjectDto> GetWithSitusByPin( string pin )
    {
      return await _httpClientWrapper.Get<RevenueObjectDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl, $"{Version}/RevenueObjects/Pin/{pin}/WithSitus" );
    }
  }
}