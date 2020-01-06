using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TAGov.Common.Http.Versioning
{
	/// <summary>
	/// RemoveVersionParameters.cs
	/// </summary>
	public class RemoveVersionParameters : IOperationFilter
	{
		/// <summary>
		/// Apply
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="context"></param>
		public void Apply(Operation operation, OperationFilterContext context)
		{
			var versionParameter = operation.Parameters.Single(p => p.Name == "version");
			operation.Parameters.Remove(versionParameter);
		}
	}
}
