using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace TAGov.Common.Security.Repository
{
    public class ProxyConfigurationDbContext : ConfigurationDbContext
    {
        public const string CommonSecurityConnectionStringEnvironmentVariable = "common.security.connectionString";

        private const string DefaultConnectionString = "Data Source=localhost;Database=Identity;Trusted_Connection=True;";
        private readonly bool _useInternal;

        public ProxyConfigurationDbContext() : base(new DbContextOptions<ConfigurationDbContext>(), new ConfigurationStoreOptions())
        {
            // Parameterless constructor for EF migrations to work.
            _useInternal = true;
        }

        public ProxyConfigurationDbContext(DbContextOptionsBuilder<ConfigurationDbContext> optionsBuilder) : base(optionsBuilder.Options, new ConfigurationStoreOptions()) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_useInternal)
            {
                var connectionString = Environment.GetEnvironmentVariable(CommonSecurityConnectionStringEnvironmentVariable) ?? DefaultConnectionString;
                optionsBuilder.UseSqlServer(connectionString.Replace("\"", ""));
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
