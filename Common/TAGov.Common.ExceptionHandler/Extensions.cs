using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TAGov.Common
{
	//https://blog.kloud.com.au/2016/03/23/aspnet-core-tips-and-tricks-global-exception-handling/
	public static class Extensions
	{
		public static IApplicationBuilder UseSharedExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					var error = context.Features.Get<IExceptionHandlerFeature>();
					if (error != null)
					{
						var exceptionHandler = context.RequestServices.GetService<IHttpExceptionHandler>();
						var result = exceptionHandler.Handle(error.Error);
						context.Response.StatusCode = result.StatusCode;

						string body = null;

						if (context.Response.ContentType == "application/xml")
						{
							context.Response.ContentType = context.Request.ContentType;
							body = ToXml(result.Body);
						}

						if (string.IsNullOrEmpty(body))
						{
							context.Response.ContentType = "application/json";
							body = ToJson(result.Body);
						}

						await context.Response.WriteAsync($"{body}");
					}
				});
			});

			return app;
		}

		private static string ToXml(ApiExceptionMessage apiExceptionMessage)
		{
			var xml = new XmlSerializer(typeof(ApiExceptionMessage));

			using (var writer = new StringWriter())
			{
				xml.Serialize(writer, apiExceptionMessage);
				return writer.ToString();
			}
		}

		/// <summary>
		/// Serialized version of ApiExceptionMessage in JSON.
		/// </summary>
		/// <returns>string.</returns>
		private static string ToJson(ApiExceptionMessage apiExceptionMessage)
		{
			return JsonConvert.SerializeObject(apiExceptionMessage);
		}

		public static void AddSharedExceptionHandler(this IServiceCollection services)
		{
			services.Add(new ServiceDescriptor(
				typeof(IHttpExceptionHandler),
				typeof(HttpExceptionHandler),
				ServiceLifetime.Singleton));
		}

		public static void AddCustomExceptionHandler<T>(this IServiceCollection services)
		{
			services.Add(new ServiceDescriptor(
				typeof(IHttpExceptionHandler),
				typeof(T),
				ServiceLifetime.Singleton));
		}
	}
}
