using TAGov.Common.Http;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public abstract class RepositoryBase : IRepositoryBase
  {
    protected RepositoryBase( string version,
                              IApplicationSettingsHelper applicationSettingsHelper,
                              IHttpClientWrapper httpClientWrapper )
    {
      Version = version;
      ApplicationSettingsHelper = applicationSettingsHelper;
      HttpClientWrapper = httpClientWrapper;
    }

    public IApplicationSettingsHelper ApplicationSettingsHelper { get; }

    public IHttpClientWrapper HttpClientWrapper { get; }

    public string Version { get; }
  }
}