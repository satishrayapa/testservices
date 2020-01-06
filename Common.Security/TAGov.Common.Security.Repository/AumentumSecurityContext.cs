using System;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository
{
    public class AumentumSecurityContext : DbContext
    {
        public const string AumentumSecurityConnectionStringEnvironmentVariable = "aumentumSecurity.connectionString";

        private const string DefaultConnectionString = "Data Source=localhost;Database=Aumentum;Trusted_Connection=True;";
        private readonly bool _useInternal;

        public AumentumSecurityContext()
        {
            // Parameterless constructor for EF migrations to work.
            _useInternal = true;
        }

        public AumentumSecurityContext(DbContextOptionsBuilder<AumentumSecurityContext> optionsBuilder) : base(optionsBuilder.Options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_useInternal)
            {
                var connectionString = Environment.GetEnvironmentVariable(AumentumSecurityConnectionStringEnvironmentVariable) ?? DefaultConnectionString;
                optionsBuilder.UseSqlServer(connectionString.Replace("\"", ""));
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AppFunction> Permissions { get; set; }
    }
}
