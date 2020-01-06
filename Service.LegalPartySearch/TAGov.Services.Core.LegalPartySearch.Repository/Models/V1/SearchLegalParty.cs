using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("LegalPartySearch", Schema = "search")]
	public class SearchLegalParty
	{
		public int Id { get; set; }

		public int? LegalPartyRoleId { get; set; }

		public int LegalPartyId { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string LegalPartyRole { get; set; }

		[Column(TypeName = "varchar(256)")]
		[MaxLength(256)]
		public string DisplayName { get; set; }

		[Column("Addr", TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string Address { get; set; }

		[MaxLength(32)]
		[Column("PIN", TypeName = "varchar(32)")]
		public string Pin { get; set; }

		[MaxLength(32)]
		[Column("UnformattedPIN", TypeName = "varchar(32)")]
		public string UnformattedPin { get; set; }

		[MaxLength(32)]
		[Column("AIN", TypeName = "varchar(32)")]
		public string Ain { get; set; }

		[Column("BegEffDate", TypeName = "datetime")]
		public DateTime EffectiveDate { get; set; }

		[Column("EffStatus", TypeName = "char(1)")]
		public string EffectiveStatus { get; set; }

		[Column("RevObjId")]
		public int? RevenueObjectId { get; set; }

		[Column("RevObjBegEffDate", TypeName = "datetime")]
		public DateTime? RevenueObjectEffectiveDate { get; set; }

		[Column("RevObjEffStatus", TypeName = "char(1)")]
		public string RevenueObjectEffectiveStatus { get; set; }

		[Column("AddrId")]
		public int? AddressId { get; set; }

		[Column("AddrBegEffDate", TypeName = "datetime")]
		public DateTime? AddressEffectiveDate { get; set; }

		[Column("AddrRoleId")]
		public int? AddressRoleId { get; set; }

		[Column("AddrRoleBegEffDate", TypeName = "datetime")]
		public DateTime? AddressRoleEffectiveDate { get; set; }

		[Column("AddrType", TypeName = "varchar(10)")]
		[MaxLength(10)]
		public string AddressType { get; set; }

		[Column("GeoCode", TypeName = "varchar(32)")]
		public string GeoCode { get; set; }

		public int? TagRoleId { get; set; }

		public DateTime? TagRoleBegEffDate { get; set; }

		public int? TagId { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string Tag { get; set; }

		[Column(TypeName = "smallint")]
		public short? TagBegEffYear { get; set; }

		public int? LegalPartyTypeId { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string LegalPartyType { get; set; }

		public int? LegalPartySubTypeId { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string LegalPartySubType { get; set; }

		[Column("AddrUnitNumber", TypeName = "varchar(8)")]
		[MaxLength(8)]
		public string AddressUnitNumber { get; set; }

		[Column("AddrStreetNumber")]
		public int? AddressStreetNumber { get; set; }

		[Column("AddrStreetName", TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string AddressStreetName { get; set; }

		public int? AppraisalSiteId { get; set; }

		[Column("AppraisalBegEffDate")]
		public DateTime? AppraisalBeginEffectiveDate { get; set; }

		[Column("AppraisalEffStatus", TypeName = "char(1)")]
		public string AppraisalEffectiveStatus { get; set; }

		public int? AppraisalRoleId { get; set; }

		[Column("AppraisalRoleBegEffDate")]
		public DateTime? AppraisalRoleBeginEffectiveDate { get; set; }

		[Column("AppraisalRoleEffStatus", TypeName = "char(1)")]
		public string AppraisalRoleEffectiveStatus { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string AppraisalSiteName { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string AppraisalClass { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string Neighborhood { get; set; }

		[Column("Mineral")]
		public bool? MineralIsNotNullWithValue { get; set; }

		[Column(TypeName = "varchar(64)")]
		[MaxLength(64)]
		public string Source { get; set; }

		public DateTime? LastUpdated { get; set; }

		[Column("StreetType", TypeName = "varchar(8)")]
		[MaxLength(8)]
		public string StreetType { get; set; }

		[Column(TypeName = "varchar(max)")]
		public string SearchAll { get; set; }

		public bool? PrimeAddress { get; set; }

		public bool? PrimeOwner { get; set; }

		[Column("AddrRoleEffStatus", TypeName = "char(1)")]
		public string AddressRoleEffectiveStatus { get; set; }

		[Column(TypeName = "varchar(32)")]
		public string City { get; set; }

		[Column(TypeName = "varchar(16)")]
		public string State { get; set; }

		[Column(TypeName = "varchar(16)")]
		public string PostalCode { get; set; }

		[Column("RevObjBegEffDateLatest", TypeName = "datetime")]
		public DateTime? RevenueObjectEffectiveDateLatest { get; set; }

		[MaxLength(32)]
		[Column("PINLatest", TypeName = "varchar(32)")]
		public string PinLatest { get; set; }
	}
}
