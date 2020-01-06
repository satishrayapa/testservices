using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository
{
	public class AumentumContext : DbContext
	{
		public AumentumContext(DbContextOptions<AumentumContext> options) :
			base(options)
		{			
		}

		public DbSet<CommRole> CommRoles { get; set; }

		public DbSet<LegalPartyRole> LegalPartyRoles { get; set; }

		public DbSet<SitusAddressRole> SitusAddressRoles { get; set; }

		public DbSet<TagRole> TaxAuthorityGroupRoles { get; set; }

		public DbSet<AppraisalSiteRole> AppraisalSiteRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AppraisalSiteRole>()
				.HasKey(c => new { c.Id, c.BeginEffectiveDate });

			modelBuilder.Entity<TagRole>()
				.HasKey(c => new { c.Id, c.BeginEffectiveDate });

			modelBuilder.Entity<SitusAddressRole>()
				.HasKey(c => new { c.Id, c.BeginEffectiveDate });

			modelBuilder.Entity<LegalPartyRole>()
				.HasKey(c => new { c.Id, c.BeginEffectiveDate });

			modelBuilder.Entity<CommRole>()
				.HasKey(c => new { c.Id, c.BeginEffectiveDate });
		}
	}
}
