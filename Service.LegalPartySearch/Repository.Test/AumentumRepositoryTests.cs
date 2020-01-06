using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Test
{
    public class AumentumRepositoryTests
    {
	    private readonly AumentumContext _context;
	    private const int ComId = 100;
	    private const int CommRoleId = 200;
	    private const int LegalPartyRoleId = 500;
	    private const int LegalPartyId = 600;
	    private const int RevenueObjectId = 700;
	    private const int SitusAddressRoleId = 1000;
	    private const int SitusAddressId = 1300;
	    private const int TagRoleId = 2000;
	    private const int TaxAuthorityGroupId = 2300;
	    private const int AppraisalSiteRoleId = 3000;
	    private const int AppraisalSiteId = 3300;
	    private const string EffectiveStatus = "A";

		public AumentumRepositoryTests()
	    {
			var beginEffectiveDate = new DateTime(2016, 1, 1);
		    var effectiveStatus = "A";

			var optionsBuilder = new DbContextOptionsBuilder<AumentumContext>();
		    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
		    _context = new AumentumContext(optionsBuilder.Options);

		    var commRole = new CommRole
		    {
			    BeginEffectiveDate = beginEffectiveDate,
			    CommId = ComId,
			    EffectiveStatus = EffectiveStatus,
			    Id = CommRoleId,
			    ObjectId = 300,
			    ObjectType = SysTypeConstants.SysTypeLegalParty
			};
		    _context.CommRoles.Add(commRole);

		    var legalPartyRole = new LegalPartyRole
		    {
			    BeginEffectiveDate = beginEffectiveDate,
				EffectiveStatus = effectiveStatus,
			    Id = LegalPartyRoleId,
			    LegalPartyId = LegalPartyId,
			    ObjectId = RevenueObjectId,
			    ObjectType = SysTypeConstants.SysTypeRevObj
			};
		    _context.LegalPartyRoles.Add(legalPartyRole);

		    var situsAddressRole = new SitusAddressRole
		    {
			    BeginEffectiveDate = beginEffectiveDate,
			    EffectiveStatus = effectiveStatus,
			    Id = SitusAddressRoleId,
			    ObjectId = RevenueObjectId,
			    ObjectType = SysTypeConstants.SysTypeRevObj,
				SitusAddressId = SitusAddressId
			};
		    _context.SitusAddressRoles.Add(situsAddressRole);

		    var tagRole = new TagRole
		    {
			    BeginEffectiveDate = beginEffectiveDate,
			    EffectiveStatus = effectiveStatus,
			    Id = TagRoleId,
			    ObjectId = RevenueObjectId,
			    ObjectType = SysTypeConstants.SysTypeRevObj,
			    TaxAuthorityGroupId = TaxAuthorityGroupId
			};
		    _context.TaxAuthorityGroupRoles.Add(tagRole);

		    var appraisalSiteRole = new AppraisalSiteRole
			{
			    BeginEffectiveDate = beginEffectiveDate,
			    EffectiveStatus = effectiveStatus,
			    Id = AppraisalSiteRoleId,
			    ObjectId = RevenueObjectId,
			    ObjectType = SysTypeConstants.SysTypeRevObj,
				AppraisalSiteId = AppraisalSiteId
			};
		    _context.AppraisalSiteRoles.Add(appraisalSiteRole);

		    _context.SaveChanges();
	    }

	    [Fact]
	    public void GetLegalPartyIdByCommIdMatchesId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

			List<int>  legalPartyIdList = aumentumRepository.GetLegalPartyIdByCommId(ComId).ToList();
		    legalPartyIdList.Count.ShouldBeGreaterThan(0);
		}

		[Fact]
	    public void GetLegalPartyIdByCommIdDoesNotMatchId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByCommId(ComId+1).ToList();
		    legalPartyIdList.Count.ShouldBe(0);
		}

	    [Fact]
	    public void GetLegalPartyIdByRevenueObjectIdMatchesId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByRevenueObjectId(RevenueObjectId).ToList();
		    legalPartyIdList.Count.ShouldBeGreaterThan(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdByRevenueObjectIdDoesNotMatchId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByRevenueObjectId(RevenueObjectId + 1).ToList();
		    legalPartyIdList.Count.ShouldBe(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdBySitusAddressIdMatchesId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdBySitusAddressId(SitusAddressId).ToList();
		    legalPartyIdList.Count.ShouldBeGreaterThan(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdBySitusAddressIdDoesNotMatchId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdBySitusAddressId(SitusAddressId + 1).ToList();
		    legalPartyIdList.Count.ShouldBe(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdByTaxAuthorityGroupIdMatchesId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByTaxAuthorityGroupId(TaxAuthorityGroupId).ToList();
		    legalPartyIdList.Count.ShouldBeGreaterThan(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdByTaxAuthorityGroupIdDoesNotMatchId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByTaxAuthorityGroupId(TaxAuthorityGroupId + 1).ToList();
		    legalPartyIdList.Count.ShouldBe(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdByAppraisalSiteIdMatchesId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByAppraisalSiteId(AppraisalSiteId).ToList();
		    legalPartyIdList.Count.ShouldBeGreaterThan(0);
	    }

	    [Fact]
	    public void GetLegalPartyIdByAppraisalSiteIdDoesNotMatchId()
	    {
		    AumentumRepository aumentumRepository = new AumentumRepository(_context);

		    var legalPartyIdList = aumentumRepository.GetLegalPartyIdByAppraisalSiteId(AppraisalSiteId + 1).ToList();
		    legalPartyIdList.Count.ShouldBe(0);
	    }
	}
}
