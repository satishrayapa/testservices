using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{

	[Table("TagRole")]
	public class TagRole
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

		[Column("TagId")]
		public int TaxAuthorityGroupId { get; set; }

		public int ObjectId { get; set; }

		public int ObjectType { get; set; }		
	}
}
