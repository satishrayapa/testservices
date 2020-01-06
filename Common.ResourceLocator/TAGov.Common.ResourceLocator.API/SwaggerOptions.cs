using TAGov.Common.Swagger;

namespace TAGov.Common.ResourceLocator.API
{
	/// <summary>
	/// Swagger options.
	/// </summary>
	public class SwaggerOptions : ISwaggerOptions
	{		
		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		public string Name => "Resource API";

		/// <summary>
		/// Gets the description of the service.
		/// </summary>
		public string Description => "Resource API that provides support to the various microservices on resource information.";
	}
}
