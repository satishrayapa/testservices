using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common;
using TAGov.Common.HealthCheck;
using TAGov.Common.Http;
using TAGov.Common.Logging;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Common.Swagger;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Repository;
using TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;

namespace TAGov.Services.Core.BaseValueSegment.API
{
  /// <summary>
  /// Startup class which configures the request pipeline that handles
  /// all requests made to the application
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="configuration">Interface used to setup configuration sources</param>
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Gets the Configuration Root
    /// </summary>
    public IConfiguration Configuration { get; }


    /// <summary>
    /// Called before Configure method
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    public void ConfigureServices( IServiceCollection services )
    {
      Mappings.Init();

      // Add framework services.
      services.AddMvc();

      services.AddSingleton(provider => Configuration);
      services.AddTransient<IBaseValueSegmentDomain, BaseValueSegmentDomain>();
      services.AddTransient<IBaseValueSegmentRepository, BaseValueSegmentRepository>();
      services.AddTransient<IAumentumDomain, AumentumDomain>();
      services.AddTransient<ICaliforniaConsumerPriceIndexRepository, CaliforniaConsumerPriceIndexRepository>();
      services.AddTransient<ISysTypeRepository, SysTypeRepository>();
      services.AddTransient<IBaseValueSegmentTransactionDomain, BaseValueSegmentTransactionDomain>();
      services.AddTransient<IBaseValueSegmentTransactionRepository, BaseValueSegmentTransactionRepository>();
      services.AddTransient<IBaseValueSegmentFlagDomain, BaseValueSegmentFlagDomain>();
      services.AddTransient<IBaseValueSegmentFlagRepository, BaseValueSegmentFlagRepository>();
      services.AddTransient<IGrmEventDomain, GrmEventDomain>();
      services.AddTransient<IGrmEventRepository, GrmEventRepository>();
      services.AddTransient<IUrlService, UrlService>();
      services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
      services.AddTransient<DbContextOptionsBuilder>();
      services.AddTransient<ISwaggerOptions, SwaggerOptions>();
      services.AddApplicationInsights();
      var connectionString = Configuration.GetConnectionString("Aumentum");

      services.AddDbContext<BaseValueSegmentQueryContext>(
        options => options.UseSqlServer(connectionString));

      services.AddDbContext<AumentumContext>(
        options => options.UseSqlServer(connectionString));

      services.AddSharedExceptionHandler();
      services.AddSharedAuthorizationHandler(Configuration, ApiNames.ApiServiceBaseValueSegment);

      const string check = "/hc/quick";
      var hc = new HealthCheckConfiguration();
      hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = connectionString });
      hc.Urls.Add(new HealthCheckUrl { Url = Configuration["Security:Authority"] + check });
      services.AddHealthCheckServices(hc);

      services.AddHttpJwtAccessor();

      services.AddSwaggerGen<Startup>();
    }


    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Allows cofiguration of request pipeline by allowing the addition of middlware components</param>
    /// <param name="loggerFactory">Allows logging configuratin</param>
    /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
    public void Configure( IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider )
    {
      app.UseSharedHealthCheck();

      loggerFactory.AddLog4NetProvider();
      app.UseAuthentication().UseSharedExceptionHandler().UseMvc();

      app.UseStaticFiles();

      app.UseSwagger(provider);

      app.UseMvc();
    }
  }
}
