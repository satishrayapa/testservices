using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using TAGov.Services.Core.LegalPartySearch.Domain.Implementation;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
using Xunit;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Test
{
	public class RebuildSearchLegalPartyTests
	{
		private readonly IRebuildSearchLegalParty _rebuildSearchLegalParty;
		private readonly Mock<IAumentumRepository> _aumentumRepositoryMock;
		private readonly Mock<IRebuildSearchLegalPartyIndexRepository> _rebuildSearchLegalPartyIndexRepositoryMock;

		public RebuildSearchLegalPartyTests()
		{
			_aumentumRepositoryMock = new Mock<IAumentumRepository>();
			_rebuildSearchLegalPartyIndexRepositoryMock = new Mock<IRebuildSearchLegalPartyIndexRepository>();

			_rebuildSearchLegalParty = new RebuildSearchLegalParty(_aumentumRepositoryMock.Object,
				_rebuildSearchLegalPartyIndexRepositoryMock.Object, new Mock<ILogger>().Object);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyId()
		{
			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				LegalPartyIdList = new List<int> { 43, 55 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 2 &&
						y.Contains(43) &&
						y.Contains(55))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdWithNoDuplicates()
		{
			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				LegalPartyIdList = new List<int> { 43, 55, 43, 55, 78 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(43) &&
						y.Contains(78) &&
						y.Contains(55))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdFromCommIdList()
		{
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByCommId(431)).Returns(new List<int> { 411, 671 });
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByCommId(155)).Returns(new List<int> { 22 });

			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				CommIdList = new List<int> { 431, 155 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(411) &&
						y.Contains(671) &&
						y.Contains(22))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdFromRevenueObjectIdList()
		{
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByRevenueObjectId(431)).Returns(new List<int> { 7411, 1671 });
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByRevenueObjectId(155)).Returns(new List<int> { 122 });

			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				RevenueObjectIdList = new List<int> { 431, 155 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(7411) &&
						y.Contains(1671) &&
						y.Contains(122))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdFromSitusAddressIdList()
		{
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdBySitusAddressId(8431)).Returns(new List<int> { 7411, 1671 });
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdBySitusAddressId(1535)).Returns(new List<int> { 122 });

			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				SitusAddressIdList = new List<int> { 8431, 1535 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(7411) &&
						y.Contains(1671) &&
						y.Contains(122))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdFromTaxAuthorityGroupIdList()
		{
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByTaxAuthorityGroupId(38431)).Returns(new List<int> { 74111, 1671 });
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByTaxAuthorityGroupId(41535)).Returns(new List<int> { 122 });

			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				TaxAuthorityGroupIdList = new List<int> { 38431, 41535 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(74111) &&
						y.Contains(1671) &&
						y.Contains(122))), Times.Once);
		}

		[Fact]
		public void RebuildListContainsAllLegalPartyIdFromAppraisalSiteIdList()
		{
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByAppraisalSiteId(338431)).Returns(new List<int> { 74111, 16731 });
			_aumentumRepositoryMock.Setup(x => x.GetLegalPartyIdByAppraisalSiteId(411535)).Returns(new List<int> { 11122 });

			_rebuildSearchLegalParty.DoAsync(new RebuildSearchLegalPartyDto
			{
				AppraisalSiteIdList = new List<int> { 338431, 411535 }
			});

			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x =>
				x.RebuildSearchLegalPartyIndexByLegalPartyId(
					It.Is<List<int>>(y =>
						y.ToList().Count == 3 &&
						y.Contains(74111) &&
						y.Contains(16731) &&
						y.Contains(11122))), Times.Once);
		}

		[Fact]
		public void DoAsyncRebuildSearchLegalPartyIndexAllIsInvoked()
		{
			_rebuildSearchLegalParty.DoAsync();
			_rebuildSearchLegalPartyIndexRepositoryMock.Verify(x => x.RebuildSearchLegalPartyIndexAll(), Times.Once);
		}
	}
}
