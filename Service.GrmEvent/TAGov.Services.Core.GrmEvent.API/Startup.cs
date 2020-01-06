using TAGov.Services.Core.GrmEvent.Domain.Implementation;
using TAGov.Services.Core.GrmEvent.Domain.Interfaces;
using TAGov.Services.Core.GrmEvent.Repository.Implementation;
using TAGov.Services.Core.GrmEvent.Repository.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TAGov.Common;
using TAGov.Common.HealthCheck;
using TAGov.Common.Logging;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Services.Core.GrmEvent.Domain.Mapping;
using TAGov.Services.Core.GrmEvent.Repository;
using TAGov.Common.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TAGov.Services.Core.GrmEvent.API
{
  /// <summary>
  /// Startup class.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Startup class constructor.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Gets the IConfiguration.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      Mappers.Init();

      // Add framework services.
      services.AddMvc();

      services.AddSingleton( provider => Configuration );
      services.AddTransient<IGrmEventDomain, GrmEventDomain>();
      services.AddTransient<IGrmEventRepository, GrmEventRepository>();
      services.AddTransient<ISwaggerOptions, SwaggerOptions>();
      services.AddTransient<DbContextOptionsBuilder>();
      services.AddApplicationInsights();
      var connectionString = Configuration.GetConnectionString( "Aumentum" );
      services.AddDbContext<GrmEventContext>(
        options => options.UseSqlServer( connectionString ));

      services.AddSharedExceptionHandler();
      services.AddSharedAuthorizationHandler( Configuration, ApiNames.ApiServiceBaseValueSegment );

      const string check = "/hc/quick";
      var hc = new HealthCheckConfiguration();
      hc.SqlConnections.Add( new HealthCheckSqlConnection { ConnectionString = connectionString } );
      hc.Urls.Add( new HealthCheckUrl { Url = Configuration["Security:Authority"] + check } );
      services.AddHealthCheckServices( hc );

      services.AddSwaggerGen<Startup>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">IApplicationBuilder.</param>
    /// <param name="loggerFactory">ILoggerFactory.</param>
    /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
    {
      app.UseSharedHealthCheck();

      loggerFactory.AddLog4NetProvider();
      app.UseAuthentication().UseSharedExceptionHandler().UseMvc();

      app.UseStaticFiles();

      app.UseSwagger( provider );

      app.UseMvc();
    }
  }
}
