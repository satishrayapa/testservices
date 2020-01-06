using System;
using System.Collections.Generic;
using System.Linq;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public static class SysTypeConstants
	{
		public const int SysTypeRevObj = 100002;
		public const int SysTypeLegalPartyRole = 100030;
		public const int SysTypeOwner = 100701;
		public const int SysTypeLegalParty = 100001;
	}

	public class AumentumRepository : IAumentumRepository
	{
		private readonly AumentumContext _aumentumContext;

		public AumentumRepository(AumentumContext aumentumContext)
		{
			_aumentumContext = aumentumContext;
		}

		public IEnumerable<int> GetLegalPartyIdByCommId(int commId)
		{
			return _aumentumContext.CommRoles.Where(x =>
				x.ObjectType == SysTypeConstants.SysTypeLegalParty &&
				x.BeginEffectiveDate <= DateTime.MaxValue &&
				x.EffectiveStatus == "A" &&
				x.CommId == commId)
				.Select(x => x.ObjectId)
				.Distinct()
				.ToList();
		}

		public IEnumerable<int> GetLegalPartyIdByRevenueObjectId(int revenueObjectId)
		{
			return _aumentumContext.LegalPartyRoles.Where(x =>
				x.ObjectType == SysTypeConstants.SysTypeRevObj &&
				x.BeginEffectiveDate <= DateTime.MaxValue &&
				x.EffectiveStatus == "A" &&
				x.ObjectId == revenueObjectId)
				.Select(x => x.LegalPartyId)
				.Distinct()
				.ToList();
		}

		public IEnumerable<int> GetLegalPartyIdBySitusAddressId(int situsAddressId)
		{
			return (from sr in _aumentumContext.SitusAddressRoles
					join lpr in _aumentumContext.LegalPartyRoles on sr.ObjectId equals lpr.ObjectId
					where sr.SitusAddressId == situsAddressId &&
						  sr.BeginEffectiveDate <= DateTime.MaxValue &&
						  sr.EffectiveStatus == "A" &&
						  sr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.EffectiveStatus == "A" &&
						  lpr.BeginEffectiveDate <= DateTime.MaxValue
					select lpr.LegalPartyId)
					.Distinct()
					.ToList();
		}

		public IEnumerable<int> GetLegalPartyIdByTaxAuthorityGroupId(int taxAuthorityGroupId)
		{
			return (from sr in _aumentumContext.TaxAuthorityGroupRoles
					join lpr in _aumentumContext.LegalPartyRoles on sr.ObjectId equals lpr.ObjectId
					where sr.TaxAuthorityGroupId == taxAuthorityGroupId &&
						  sr.BeginEffectiveDate <= DateTime.MaxValue &&
						  sr.EffectiveStatus == "A" &&
						  sr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.EffectiveStatus == "A" &&
						  lpr.BeginEffectiveDate <= DateTime.MaxValue
					select lpr.LegalPartyId)
				.Distinct()
				.ToList();
		}

		public IEnumerable<int> GetLegalPartyIdByAppraisalSiteId(int appraisalSiteId)
		{
			return (from sr in _aumentumContext.AppraisalSiteRoles
					join lpr in _aumentumContext.LegalPartyRoles on sr.ObjectId equals lpr.ObjectId
					where sr.AppraisalSiteId == appraisalSiteId &&
						  sr.BeginEffectiveDate <= DateTime.MaxValue &&
						  sr.EffectiveStatus == "A" &&
						  sr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.ObjectType == SysTypeConstants.SysTypeRevObj &&
						  lpr.EffectiveStatus == "A" &&
						  lpr.BeginEffectiveDate <= DateTime.MaxValue
					select lpr.LegalPartyId)
				.Distinct()
				.ToList();
		}
	}
}
