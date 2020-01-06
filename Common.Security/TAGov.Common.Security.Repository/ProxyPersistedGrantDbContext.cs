using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace TAGov.Common.Security.Repository
{
	public class ProxyPersistedGrantDbContext : PersistedGrantDbContext
    {
        private const string DefaultConnectionString = "Data Source=localhost;Database=Identity;Trusted_Connection=True;";
		private readonly bool _useInternal;
		public ProxyPersistedGrantDbContext() : base(new DbContextOptions<PersistedGrantDbContext>(), new OperationalStoreOptions())
		{
			// Parameterless constructor for EF migrations to work.
			_useInternal = true;
		}

		public ProxyPersistedGrantDbContext(DbContextOptionsBuilder<PersistedGrantDbContext> optionsBuilder) : base(optionsBuilder.Options, new OperationalStoreOptions()) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (_useInternal)
			{
			  var connectionString = Environment.GetEnvironmentVariable( ProxyConfigurationDbContext.CommonSecurityConnectionStringEnvironmentVariable ) ?? DefaultConnectionString;
				optionsBuilder.UseSqlServer(connectionString.Replace("\"", ""));
			}
			base.OnConfiguring(optionsBuilder);
		}
	}
}
