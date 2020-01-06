using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TAGov.Common;
using TAGov.Common.HealthCheck;
using TAGov.Common.Http;
using TAGov.Common.Logging;
using TAGov.Common.Swagger;

namespace TAGov.Services.Facade.AssessmentHeader.API
{
  /// <summary>
  /// Startup class for Web API.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Startup.
    /// </summary>
    /// <param name="configuration">Interface used to setup configuration sources</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Gets the IConfigurationRoot.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">for adding services</param>
    public void ConfigureServices( IServiceCollection services )
    {
      services.AddSingleton( provider => Configuration );
      services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

      // add version 1 support
      services.AddTransient<Domain.Interfaces.V1.IApplicationSettingsHelper, Domain.Implementation.V1.ApplicationSettingsHelper>();
      services.AddTransient<Domain.Interfaces.V1.IAssessmentHeaderDomain, Domain.Implementation.V1.AssessmentHeaderDomain>();
      services.AddTransient<Domain.Interfaces.V1.IAssessmentHeaderDomain, Domain.Implementation.V1.AssessmentHeaderDomain>();
      services.AddApplicationInsights();

      const string check = "hc/quick";
      var hc = new HealthCheckConfiguration();
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:assessmentEventServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:revenueObjectServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:legalPartyServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:baseValueSegmentServiceApiUrl" ] + check } );
      services.AddHealthCheckServices( hc );

      services.AddTransient<ISwaggerOptions, SwaggerOptions>();

      // Add framework services.
      services.AddMvc();
      services.AddSharedExceptionHandler();

      services.AddHttpJwtAccessor();

      services.AddSwaggerGen<Startup>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">pipeline to configure</param>
    /// <param name="loggerFactory">logger creator</param>
    /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
    public void Configure( IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
    {
      app.UseSharedHealthCheck();

      loggerFactory.AddLog4NetProvider();
      app.UseSharedExceptionHandler().UseMvc();

      app.UseStaticFiles();

      app.UseSwagger(provider);

      app.UseMvc();
    }
  }
}
