using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("SitusAddrRole")]
	public class SitusAddressRole
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[Key]
		[Column("BegEffDate", Order = 1)]
		public DateTime BeginEffectiveDate { get; set; }

		public int ObjectId { get; set; }

		public int ObjectType { get; set; }

		[Column("EffStatus")]
		public string EffectiveStatus { get; set; }

		[Column("SitusAddrId")]
		public int SitusAddressId { get; set; }
	}
}
