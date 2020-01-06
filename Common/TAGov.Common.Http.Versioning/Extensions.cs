using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TAGov.Common.Http.Versioning
{
	public static class Extensions
	{
		public static void ConfigureSwaggerWithVersioning(this SwaggerGenOptions options)
		{
            // TODO: Fix versioning.
			//options.DocInclusionPredicate((docName, apiDesc) =>
			//{
			//	var versions = apiDesc.ControllerAttributes()
			//		.OfType<ApiVersionAttribute>()
			//		.SelectMany(attr => attr.Versions);

			//	return versions.Any(v => $"v{v.ToString()}" == docName);
			//});

			//options.OperationFilter<RemoveVersionParameters>();
			//options.DocumentFilter<SetVersionInPaths>();
		}
	}
}
