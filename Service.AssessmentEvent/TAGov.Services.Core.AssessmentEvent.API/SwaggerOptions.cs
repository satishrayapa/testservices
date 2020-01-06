using TAGov.Common.Swagger;

namespace TAGov.Services.Core.AssessmentEvent.API
{
  /// <summary>
  /// Swagger options.
  /// </summary>
  public class SwaggerOptions : ISwaggerOptions
  {
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    public string Name => "Assessment Event API";

    /// <summary>
    /// Gets the description of the service.
    /// </summary>
    public string Description => "Assessment Event API manages assessment event based API.";
  }
}
