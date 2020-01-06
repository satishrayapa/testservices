using System;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository
{
	public class SearchLegalPartyContext : DbContext
	{
		public const string SearchLegalPartyContextConnectionStringEnvironmentVariable = "service.legalpartysearch.connectionString";

		private const string DefaultConnectionString = "Data Source=localhost;Database=Aumentum;Trusted_Connection=True;";
		private readonly bool _useInternal;

		public SearchLegalPartyContext()
		{
			// Parameterless constructor for EF migrations to work.
			_useInternal = true;

			SetupTimeoutCommand();
		}

		public SearchLegalPartyContext(DbContextOptions<SearchLegalPartyContext> options) :
			base(options)
		{

		}

		private void SetupTimeoutCommand()
		{
			var timeoutCommandString = Environment.GetEnvironmentVariable("service.legalpartysearch.commandTimeout");
			if (!string.IsNullOrEmpty(timeoutCommandString))
			{
				if (int.TryParse(timeoutCommandString, out int timeoutCommand))
					Database.SetCommandTimeout(timeoutCommand);
			}
		}

		protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
		{
			if ( _useInternal )
			{
				var connectionString = Environment.GetEnvironmentVariable( SearchLegalPartyContextConnectionStringEnvironmentVariable ) ?? DefaultConnectionString;
				optionsBuilder.UseSqlServer( connectionString.Replace( "\"", "" ) );
			}

#if DEBUG
			optionsBuilder.EnableSensitiveDataLogging();
#endif

			base.OnConfiguring( optionsBuilder );
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// For the purpose of writing unit test, we are adding this try/catch
			// because UseInMemoryDatabase does not understand "default value", so it
			// is going to throw a MissingMethodException.
			try
			{
				modelBuilder.Entity<SearchLegalParty>()
					.Property(p => p.EffectiveStatus)
					.HasDefaultValue("A");

				modelBuilder.Entity<SystemStopword>()
					.HasKey(x => new { x.LanguageId, x.Stopword });
			}
			catch (MissingMethodException)
			{

			}

			modelBuilder.Entity<SearchLegalParty>()
				.HasIndex(p => new { p.LegalPartyRoleId, p.EffectiveDate });

			modelBuilder.Entity<SearchLegalParty>()
				.HasIndex(p => new { p.LegalPartyId });
		}

		public DbSet<SearchLegalParty> LegalParties { get; set; }

		public DbSet<SystemStopword> SystemStopwords { get; set; }

		public DbSet<AumentumChangeTrackingVersion> AumentumChangeTrackingVersion { get; set; }

		public DbSet<AumentumChangeTrackingVersionSaved> AumentumChangeTrackingVersionSaved { get; set; }

		public DbSet<CrawlProgress> Crawls { get; set; }
	}
}
