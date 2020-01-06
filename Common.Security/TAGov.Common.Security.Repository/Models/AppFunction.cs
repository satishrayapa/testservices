using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.Security.Repository.Models
{
	[Table("AppFunction")]
	public class AppFunction
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[Column("TranId")]
		public long TransactionId { get; set; }

		[Column("Name", TypeName = "char(64)")]
		public string Name { get; set; }

		[Column("AppFunctionType", TypeName = "char(32)")]
		public string AppFunctionType { get; set; }

		public string App { get; set; }

		[Column("LongDescr", TypeName = "varchar(256)")]
		public string LongDescription { get; set; }

		[Column("ParentName", TypeName = "char(64)")]
		public string ParentName { get; set; }

		public int ParentId { get; set; }

		public short IsMenuItem { get; set; }

		public int SecAppId { get; set; }

		public short IgnoreSec { get; set; }

		public int SecSyncKey { get; set; }

		public int ObjectType { get; set; }

		public int FieldSysType { get; set; }

		[Column("FieldValue", TypeName = "varchar(25)")]
		public string FieldValue { get; set; }
	}
}
