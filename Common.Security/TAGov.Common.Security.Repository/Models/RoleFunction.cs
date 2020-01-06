using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.Security.Repository.Models
{
	[Table("RoleFunction")]
	public class RoleFunction
	{
		public int Id { get; set; }
		public int RoleId { get; set; }
		public int AppFunctionId { get; set; }
		public short CanCreate { get; set; }
		public short CanView { get; set; }
		public short CanModify { get; set; }
		public short CanDelete { get; set; }
	}
}
