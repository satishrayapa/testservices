using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common.HealthCheck;
using TAGov.Common.Logging;
using TAGov.Common.ResourceLocator.Domain.Implementation;
using TAGov.Common.ResourceLocator.Domain.Interfaces;
using TAGov.Common.ResourceLocator.Repository;
using TAGov.Common.ResourceLocator.Repository.Implementation;
using TAGov.Common.ResourceLocator.Repository.Interfaces;
using TAGov.Common.Security.Http.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TAGov.Common.Swagger;

namespace TAGov.Common.ResourceLocator.API
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
		/// Gets the IConfigurationRoot.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">IServiceCollection.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddMvc();

			services.AddSingleton(provider => Configuration);
			services.AddTransient<IResourceDomain, ResourceDomain>();
			services.AddTransient<IResourceRepository, ResourceRepository>();
			services.AddTransient<ISwaggerOptions, SwaggerOptions>();
			services.AddTransient<DbContextOptionsBuilder>();
			services.AddApplicationInsights();

			var connectionString = Configuration.GetConnectionString("Resource");
			services.AddDbContext<ResourceContext>(
				options => options.UseSqlServer(connectionString));

			services.AddSharedExceptionHandler();
			services.AddSharedAuthorizationHandler(Configuration, ApiNames.ApiCommonResourceLocator);

			const string check = "/hc/quick";
			var hc = new HealthCheckConfiguration();
			hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = connectionString });
			hc.Urls.Add(new HealthCheckUrl { Url = Configuration["Security:Authority"] + check });
			services.AddHealthCheckServices(hc);

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
			
			app.UseAuthentication().UseSharedExceptionHandler();

			app.UseStaticFiles();

			app.UseSwagger(provider);

			app.UseMvc();
		}
	}
}
