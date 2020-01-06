using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("LegalPartyRole")]
	public class LegalPartyRole
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public int ObjectType { get; set; }

		[Key]
		[Column("BegEffDate", Order = 1)]
		public DateTime BeginEffectiveDate { get; set; }

		[Column("EffStatus")]
		public string EffectiveStatus { get; set; }

		public int LegalPartyId { get; set; }
		public int ObjectId { get; set; }
	}
}
