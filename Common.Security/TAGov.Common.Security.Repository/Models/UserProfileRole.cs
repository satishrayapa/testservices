using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.Security.Repository.Models
{
	[Table("UserProfileRole")]
	public class UserProfileRole
	{
		public int Id { get; set; }

		public int UserProfileId { get; set; }

		public int RoleId { get; set; }
	}
}
