using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common;
using TAGov.Common.HealthCheck;
using TAGov.Common.Logging;
using TAGov.Common.Paging;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Common.Swagger;
using TAGov.Services.Core.LegalPartySearch.API.Configurations;
using TAGov.Services.Core.LegalPartySearch.Domain.Implementation;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Mappings;
using TAGov.Services.Core.LegalPartySearch.Repository;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.API
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
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// Gets the IConfigurationRoot.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// Called before Configure method
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">IServiceCollection</param>
		public void ConfigureServices(IServiceCollection services)
		{
			Mappings.Init();

			// Add framework services.
			services.AddMvc();
			services.AddApiVersioning(o => o.ReportApiVersions = true);
			//Add support for common exception handling
			services.AddSharedExceptionHandler();
			services.AddSharedAuthorizationHandler(Configuration, ApiNames.ApiServiceLegalPartySearch);

			services.AddSingleton(provider => Configuration);

			services.AddTransient<IRebuildSearchLegalParty, RebuildSearchLegalParty>();
			services.AddTransient<ISearchLegalPartyDomain, SearchLegalPartyDomain>();
			services.AddTransient<IAumentumRepository, AumentumRepository>();
//			services.AddTransient<ISearchLegalPartyRepository, FreeTextSearchLegalPartyRepository>();
			services.AddTransient<ISearchLegalPartyRepository, ContainsSearchLegalPartyRepository>();
			services.AddTransient<IDefaultSearchProviderConfiguration, DefaultSearchProviderConfiguration>();
			services.AddTransient<ISearchProviderSelector, SearchProviderSelector>();
			services.AddTransient<ICommandTimeoutConfiguration, CommandTimeoutConfiguration>();
			services.AddTransient<ISearchResultsConfiguration, SearchResultsConfiguration>();
			services.AddTransient<IRebuildSearchLegalPartyIndexRepository, RebuildSearchLegalPartyIndexRepository>();
			services.AddScoped<IConfiguration>(p => Configuration);
			services.AddScoped<IPagingInfo, PagingInfo>();
			services.AddTransient<DbContextOptionsBuilder>();
			services.AddTransient(provider => provider.GetService<ILoggerFactory>().CreateLogger("LegalPartySearch"));

			var connectionString = Configuration.GetConnectionString("Aumentum");

			services.AddDbContext<SearchLegalPartyContext>(options => options.UseSqlServer(connectionString));
			services.AddDbContext<AumentumContext>(options => options.UseSqlServer(connectionString));
			services.AddTransient<ISearchOperations, SearchOperations>();

			const string check = "/hc/quick";				
			var hc = new HealthCheckConfiguration();
			hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = connectionString });
			hc.Urls.Add(new HealthCheckUrl { Url = Configuration["Security:Authority"] + check });						
			services.AddHealthCheckServices(hc);

			var assembly = Assembly.GetEntryAssembly();

			services.AddSwaggerGen( "Legal Party Search API", "v1.1", $"A Legal Party Search API to provide support for the retrieval of Legal Parties in Aumentum (Version: {assembly.GetName().Version}).", Configuration );
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app">Allows cofiguration of request pipeline by allowing the addition of middlware components</param>
		/// <param name="env">Environment.</param>
		/// <param name="loggerFactory">Allows logging configuratin</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseSharedHealthCheck();

			loggerFactory.AddLog4NetProvider();
			app.UseAuthentication().UseSharedExceptionHandler().UseMvc();

			app.UseStaticFiles();

			app.UseSwagger( Configuration );

			app.UseMvc();
		}
	}
}
