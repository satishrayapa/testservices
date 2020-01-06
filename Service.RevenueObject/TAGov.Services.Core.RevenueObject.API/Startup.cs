using TAGov.Services.Core.RevenueObject.Domain.Implementation;
using TAGov.Services.Core.RevenueObject.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common;
using TAGov.Common.Logging;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.HealthCheck;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Common.Swagger;
using TAGov.Services.Core.RevenueObject.Domain.Mapping;
using TAGov.Services.Core.RevenueObject.Repository.Implementation.V1;
using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;
using TAGov.Services.Core.RevenueObject.Repository.Maps.V1;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TAGov.Services.Core.RevenueObject.API
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup class constructor.
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
        /// <param name="services">IServiceCollection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            Mappings.Init();

            // Add framework services.
            services.AddMvc();

            services.AddSingleton(provider => Configuration);
            services.AddTransient<IRevenueObjectDomain, RevenueObjectDomain>();
            services.AddTransient<IRevenueObjectRepository, RevenueObjectRepository>();
            services.AddTransient<IMarketAndRestrictedValueRepository, MarketAndRestrictedValueRepository>();
            services.AddTransient<ISwaggerOptions, SwaggerOptions>();
            services.AddTransient<DbContextOptionsBuilder>();
            services.AddApplicationInsights();
            var connectionString = Configuration.GetConnectionString("Aumentum");

            services.AddDbContext<RevenueObjectContext>(
              options => options.UseSqlServer(connectionString));

            const string check = "/hc/quick";
            var hc = new HealthCheckConfiguration();
            hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = connectionString });
            hc.Urls.Add(new HealthCheckUrl { Url = Configuration["Security:Authority"] + check });
            services.AddHealthCheckServices(hc);

            services.AddSharedExceptionHandler();
            services.AddSharedAuthorizationHandler(Configuration, ApiNames.ApiServiceRevenueObject);

            services.AddSwaggerGen<Startup>();
        }

        /// <summary>
        /// Gets the IConfigurationRoot.
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

            app.UseSwagger(provider);

            app.UseMvc();
        }
    }
}
