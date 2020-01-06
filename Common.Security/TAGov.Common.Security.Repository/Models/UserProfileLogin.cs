using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.Security.Repository.Models
{
	[Table("UserProfileLogin")]
	public class UserProfileLogin
	{
		[Key]
		public int Id { get; set; }

		public int UserProfileId { get; set; }
	}
}
