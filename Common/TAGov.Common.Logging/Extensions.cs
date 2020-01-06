using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TAGov.Common.Logging
{
  public static class Extensions
  {
    public static void AddLog4NetProvider(this ILoggerFactory loggerFactory)
    {
        loggerFactory.AddLog4Net();
    }

    public static void AddApplicationInsights(this IServiceCollection services)
    {
      services.AddApplicationInsightsTelemetry();
    }
  }
}
