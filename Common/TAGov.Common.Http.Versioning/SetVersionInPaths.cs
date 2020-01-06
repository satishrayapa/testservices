using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TAGov.Common.Http.Versioning
{
    public class SetVersionInPaths : IDocumentFilter
	{
		/// <summary>
		/// Apply
		/// </summary>
		/// <param name="swaggerDoc"></param>
		/// <param name="context"></param>
		public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
		{
			swaggerDoc.Paths = swaggerDoc.Paths
				.ToDictionary(
					path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
					path => path.Value
				);
		}
	}
}
