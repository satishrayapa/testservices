using TAGov.Common.Swagger;

namespace TAGov.Services.Facade.AssessmentHeader.API
{
  /// <summary>
  /// Swagger options.
  /// </summary>
  public class SwaggerOptions : ISwaggerOptions
  {
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    public string Name => "Assessment Header Facade API";

    /// <summary>
    /// Gets the description of the service.
    /// </summary>
    public string Description => "Assessment Header Facade API server that retrieves the Assessment Event related data.";
  }
}
