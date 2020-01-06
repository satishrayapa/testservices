using TAGov.Common.Swagger;

namespace TAGov.Services.Core.GrmEvent.API
{
    /// <summary>
    /// Swagger options.
    /// </summary>
    public class SwaggerOptions : ISwaggerOptions
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name => "GRM Event API";

        /// <summary>
        /// Gets the description of the service.
        /// </summary>
        public string Description => "GRM Event API to manage GRM Event.";
    }
}
