using TAGov.Common.Swagger;

namespace TAGov.Services.Facade.BaseValueSegment.API
{
  /// <summary>
  /// Swagger options.
  /// </summary>
  public class SwaggerOptions : ISwaggerOptions
  {
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    public string Name => "Base Value Segment Facade API";

    /// <summary>
    /// Gets the description of the service.
    /// </summary>
    public string Description => "Base Value Segment Facade API server that retrieves the Base Value Segment related data.";
  }
}
