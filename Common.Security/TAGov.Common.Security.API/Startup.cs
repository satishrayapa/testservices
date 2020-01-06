using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TAGov.Common.HealthCheck;
using TAGov.Common.Logging;
using TAGov.Common.Security.Domain.Implementation;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Http.Authorization;
using TAGov.Common.Security.Repository;
using TAGov.Common.Security.Repository.Implementation;
using TAGov.Common.Security.Repository.Interfaces;
using TAGov.Common.Swagger;

namespace TAGov.Common.Security.API
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = MigrationsAssemblyReference.Name;

            var identityServer = services.AddIdentityServer()
                .AddOperationalStore(builder => builder.ConfigureDbContext = ctx => ctx.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(builder => builder.ConfigureDbContext = ctx => ctx.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)));

            var certName = Configuration["Security:CertName"];
            if (string.IsNullOrEmpty(certName))
            {
                identityServer.AddDeveloperSigningCredential(true, Configuration["Security:SigningFilename"]);
            }
            else
            {
                var isCertLocationCurrentUser = Convert.ToBoolean(Configuration["Security:IsCertLocationCurrentUser"]);

                identityServer.AddSigningCredential(certName, isCertLocationCurrentUser ?
                    StoreLocation.CurrentUser : StoreLocation.LocalMachine);
            }
            services.AddMvc();

            services.AddSingleton(provider => Configuration);

            services.AddTransient<DbContextOptionsBuilder>();

            var permissionsConnectionString = Configuration.GetConnectionString("Permissions");
            services.AddDbContext<AumentumSecurityQueryContext>(
                options => options.UseSqlServer(permissionsConnectionString));

            services.AddDbContext<ProxyConfigurationDbContext>(
                options => options.UseSqlServer(connectionString));
            
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IAppFunctionForQueryRepository, AppFunctionForQueryRepository>();
            services.AddTransient<IClaimsProvider, ClaimsProvider>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IClaimsService, ServiceClientClaimsService>();
            services.AddTransient<IApiResourceRepository, ApiResourceRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IClientDomain, ClientDomain>();
            services.AddTransient<ISwaggerOptions, SwaggerOptions>();
            
            services.AddApplicationInsights();
            services.AddSharedExceptionHandler();
            services.AddSharedAuthorizationHandler(Configuration, ApiNames.ApiCommonSecurity);

            var hc = new HealthCheckConfiguration();
            hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = connectionString });
            hc.SqlConnections.Add(new HealthCheckSqlConnection { ConnectionString = permissionsConnectionString });
            services.AddHealthCheckServices(hc);

            services.AddSwaggerGen<Startup>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder.</param>
        /// <param name="provider">provider.</param>
        /// <param name="loggerFactory">ILoggerFactory.</param>
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            app.UseSharedHealthCheck();

            loggerFactory.AddLog4NetProvider();

            app.UseAuthentication().UseSharedExceptionHandler();

            app.UseStaticFiles();

            app.UseSwagger(provider);

            app.UseIdentityServer();

            app.UseMvc();
        }
    }
}

