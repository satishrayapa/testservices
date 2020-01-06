using TAGov.Common.Swagger;

namespace TAGov.Services.Core.LegalParty.API
{
    /// <summary>
    /// Swagger options.
    /// </summary>
    public class SwaggerOptions : ISwaggerOptions
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name => "Legal Party API";

        /// <summary>
        /// Gets the description of the service.
        /// </summary>
        public string Description => "Legal Party API to manage Records related services.";
    }
}
