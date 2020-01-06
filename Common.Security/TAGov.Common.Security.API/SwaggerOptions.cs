using TAGov.Common.Swagger;

namespace TAGov.Common.Security.API
{
    /// <summary>
    /// Swagger options.
    /// </summary>
    public class SwaggerOptions : ISwaggerOptions
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name => "Security API";

        /// <summary>
        /// Gets the description of the service.
        /// </summary>
        public string Description => "Security API that provides support to managing secured resources.";
    }
}
