using Microsoft.EntityFrameworkCore;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository
{
	/// <summary>
	/// This is used exclusively for performing lookups against the current Aumentum database.
	/// </summary>
	public class AumentumSecurityQueryContext : DbContext
	{
		public AumentumSecurityQueryContext(DbContextOptions<AumentumSecurityQueryContext> options) : base(options) { }

		public DbSet<UserProfileRole> UsersRoles { get; set; }

		public DbSet<RoleFunction> RolesPermissions { get; set; }

		public DbSet<AppFunctionForQuery> Permissions { get; set; }

		public DbSet<UserProfileLogin> UserLogins { get; set; }
	}
}
