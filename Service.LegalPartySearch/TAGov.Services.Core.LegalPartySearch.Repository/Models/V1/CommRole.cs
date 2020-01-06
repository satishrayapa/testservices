using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("CommRole")]
	public class CommRole
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[Key]
		[Column("BegEffDate", Order = 1)]
		public DateTime BeginEffectiveDate { get; set; }

		[Required]
		[StringLength(1)]
		[Column("EffStatus")]
		public string EffectiveStatus { get; set; }

		public int CommId { get; set; }

		public int ObjectType { get; set; }

		public int ObjectId { get; set; }
	}
}
