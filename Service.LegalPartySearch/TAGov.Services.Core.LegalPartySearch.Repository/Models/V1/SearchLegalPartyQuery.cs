namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	public class SearchLegalPartyQuery
	{
		public bool? ExcludeDisplayName { get; set; }

		public bool? ExcludeAddress { get; set; }

		public bool? ExcludePin { get; set; }

		public bool? ExcludeAin { get; set; }

		public bool? ExcludeGeoCode { get; set; }

		public bool? ExcludeTag { get; set; }

		public bool ExcludeSearchAll { get; set; }

		public bool? RevenueObjectIdIsNotNull { get; set; }

		public bool? RevenueObjectIsActive { get; set; }

		public bool? AppraisalSiteIdIsNotNull { get; set; }

		public bool? MineralIsNotNullWithValue { get; set; }

		public bool? CoalesceIfDuplicateAddress { get; set; }

		public string AddressType { get; set; }
	}
}
