using System;

namespace TAGov.Search
{
	public class SearchLegalPartyDto
	{
		public int Id { get; set; }

		public int LegalPartyId { get; set; }

		public int LegalPartyRoleId { get; set; }

		public string LegalPartyRole { get; set; }

		public string DisplayName { get; set; }

		public string Pin { get; set; }

		public string Address { get; set; }

		public string Ain { get; set; }

		public DateTime EffectiveDate { get; set; }

		public bool IsActive { get; set; }

		public int? RevenueObjectId { get; set; }

		public DateTime? RevenueObjectEffectiveDate { get; set; }

		public string RevenueObjectEffectiveStatus { get; set; }

		public int? AddressId { get; set; }

		public DateTime? AddressEffectiveDate { get; set; }

		public int? AddressRoleId { get; set; }

		public DateTime? AddressRoleEffectiveDate { get; set; }

		public string AddressType { get; set; }

		public string GeoCode { get; set; }

		public int? TagRoleId { get; set; }

		public DateTime? TagRoleBegEffDate { get; set; }

		public int? TagId { get; set; }

		public string Tag { get; set; }

		public short? TagBegEffYear { get; set; }

		public int? LegalPartyTypeId { get; set; }

		public string LegalPartyType { get; set; }

		public int? LegalPartySubTypeId { get; set; }

		public string LegalPartySubType { get; set; }

		public string AddressUnitNumber { get; set; }

		public int? AddressStreetNumber { get; set; }

		public string AddressStreetName { get; set; }

		public int? AppraisalSiteId { get; set; }

		public DateTime? AppraisalBeginEffectiveDate { get; set; }

		public string AppraisalEffectiveStatus { get; set; }

		public int? AppraisalRoleId { get; set; }

		public DateTime? AppraisalRoleBeginEffectiveDate { get; set; }

		public string AppraisalRoleEffectiveStatus { get; set; }

		public string AppraisalSiteName { get; set; }

		public string AppraisalClass { get; set; }

		public string Neighborhood { get; set; }

		public string StreetType { get; set; }

		public bool? PrimeAddress { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string PostalCode { get; set; }
	}
}
