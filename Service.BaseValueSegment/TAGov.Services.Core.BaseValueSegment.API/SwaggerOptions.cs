using TAGov.Common.Swagger;

namespace TAGov.Services.Core.BaseValueSegment.API
{
  /// <summary>
  /// Swagger options.
  /// </summary>
  public class SwaggerOptions : ISwaggerOptions
  {
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    public string Name => "Base Value Segment API";

    /// <summary>
    /// Gets the description of the service.
    /// </summary>
    public string Description => "Base Value Segment API to manage Base Value Segments.";
  }
}
