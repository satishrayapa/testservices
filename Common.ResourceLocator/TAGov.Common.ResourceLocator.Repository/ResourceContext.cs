using System;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.ResourceLocator.Repository.Models.V1;

namespace TAGov.Common.ResourceLocator.Repository
{
	public class ResourceContext : DbContext
	{
		public const string CommonResourceLocatorConnectionStringEnvironmentVariable = "common.resourceLocator.connectionString";
		private const string DefaultConnectionString = "Data Source=localhost;Database=Resource;Trusted_Connection=True;";
		private readonly bool _useInternal;

		public ResourceContext()
		{
			// Parameterless constructor for EF migrations to work.
			_useInternal = true;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (_useInternal)
			{
				var connectionString = Environment.GetEnvironmentVariable(CommonResourceLocatorConnectionStringEnvironmentVariable) ?? DefaultConnectionString;
				optionsBuilder.UseSqlServer(connectionString.Replace("\"", ""));
			}

			base.OnConfiguring(optionsBuilder);
		}

		public ResourceContext(DbContextOptionsBuilder optionsBuilder) : base(optionsBuilder.Options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Resource>().HasKey(entity => new { entity.Key, Parition = entity.Partition });
		}

		public DbSet<Resource> Resources { get; set; }

	}
}
