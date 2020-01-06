using TAGov.Common.Http;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IRepositoryBase
  {
    IApplicationSettingsHelper ApplicationSettingsHelper { get; }

    IHttpClientWrapper HttpClientWrapper { get; }

    string Version { get; }
  }
}