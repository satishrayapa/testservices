using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;

namespace TAGov.Common.HealthCheck
{
  public static class Extensions
  {
    public const int DefaultSqlCacheInSeconds = 5;
    public const int DefaultUrlCacheInSeconds = 3;
    public static IWebHostBuilder UseSharedHealthChecks(this IWebHostBuilder webHostBuilder)
    {
      webHostBuilder.UseHealthChecks("/hc");
      return webHostBuilder;
    }

    public static IServiceCollection AddHealthCheckServices(
      this IServiceCollection services, HealthCheckConfiguration healthCheckConfiguration)
    {
      services.AddHealthChecks(checks =>
      {
        int counter = 0;
        healthCheckConfiguration.SqlConnections.ForEach(x =>
        {
          counter++;

          checks.AddSqlCheck($"CheckConnection{counter}", x.ConnectionString, TimeSpan.FromSeconds(
            x.CacheInSeconds ?? DefaultSqlCacheInSeconds));
        });

        healthCheckConfiguration.Urls.ForEach(x =>
        {
          checks.AddUrlCheck(x.Url, TimeSpan.FromSeconds(
            x.CacheInSeconds ?? DefaultUrlCacheInSeconds));
        });
      });
      return services;
    }

    public static IApplicationBuilder UseSharedHealthCheck(this IApplicationBuilder app)
    {
      app.MapWhen(context =>
        context.Request.Method == HttpMethods.Get &&
        context.Request.Path.Value.EndsWith("/hc/quick"), map => map.Run(async ctx =>
      {
        ctx.Response.Headers["content-type"] = "application/json";
        await ctx.Response.WriteAsync("{ \"result\": \"OK\" }");
      }));

      return app;
    }
  }
}
