using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.Security.Repository.Models
{
	[Table("AppFunction")]
	public class AppFunctionForQuery
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public string Name { get; set; }

		public string AppFunctionType { get; set; }

		public string App { get; set; }

		public string ParentName { get; set; }

    public int ParentId { get; set; }
	}
}
