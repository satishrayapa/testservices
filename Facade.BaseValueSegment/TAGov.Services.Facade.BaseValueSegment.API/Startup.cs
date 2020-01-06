using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common;
using TAGov.Common.Caching;
using TAGov.Common.HealthCheck;
using TAGov.Common.Http;
using TAGov.Common.Logging;
using TAGov.Common.Swagger;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.API
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

      // add version 1.1 support
      services.AddTransient<IApplicationSettingsHelper, ApplicationSettingsHelper>();
      services.AddTransient<IBaseValueSegmentRepository, BaseValueSegmentRepository>();
      services.AddTransient<IRevenueObjectRepository, RevenueObjectRepository>();
      services.AddTransient<IAssessmentEventRepository, AssessmentEventRepository>();
      services.AddTransient<IGrmEventRepository, GrmEventRepository>();
      services.AddTransient<ILegalPartyRepository, LegalPartyRepository>();
      services.AddTransient<ILegalPartyDomain, LegalPartyDomain>();
      services.AddTransient<IGrmEventDomain, GrmEventDomain>();
      services.AddTransient<IRevenueObjectDomain, RevenueObjectDomain>();
      services.AddTransient<IBeneificialInterestBaseValueSegmentDomain, BeneificialInterestBaseValueSegmentDomain>();
      services.AddTransient<IBeneificialInterestDetailBaseValueSegmentDomain, BeneificialInterestDetailBaseValueSegmentDomain>();
      services.AddTransient<ISubComponentBaseValueSegmentDomain, SubComponentBaseValueSegmentDomain>();
      services.AddTransient<IBaseValueSegmentDomain, BaseValueSegmentDomain>();
      services.AddTransient<IBaseValueSegmentProvider, BaseValueSegmentProvider>();
      services.AddTransient<IBaseValueSegmentHistoryDomain, BaseValueSegmentHistoryDomain>();
      services.AddTransient(
        provider => provider.GetService<ILoggerFactory>().CreateLogger( "FacadeBaseValueSegment" ) );
      services.AddApplicationInsights();
      services.AddSharedCaching();
      services.AddSharedExceptionHandler();

      // Add framework services.
      services.AddMvc( options =>
                       {
                         // for all controllers, tell client browser not to cache
                         options.Filters.Add( new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None } );
                       } );

      services.AddHttpJwtAccessor();

      const string check = "hc/quick";
      var hc = new HealthCheckConfiguration();
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:assessmentEventServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:revenueObjectServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:legalPartyServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:baseValueSegmentServiceApiUrl" ] + check } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration[ "ServiceApiUrls:grmEventServiceApiUrl" ] + check } );
      services.AddHealthCheckServices( hc );
      services.AddTransient<ISwaggerOptions, SwaggerOptions>();
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

      app.UseSwagger( provider );

      app.UseMvc();
    }
  }
}
