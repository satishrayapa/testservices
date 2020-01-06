using TAGov.Common.Swagger;

namespace TAGov.Services.Core.RevenueObject.API
{
    /// <summary>
    /// Swagger options.
    /// </summary>
    public class SwaggerOptions : ISwaggerOptions
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name => "Revenue Object API";

        /// <summary>
        /// Gets the description of the service.
        /// </summary>
        public string Description => "A Revenue Object API manage Revenue Objects.";
    }
}
