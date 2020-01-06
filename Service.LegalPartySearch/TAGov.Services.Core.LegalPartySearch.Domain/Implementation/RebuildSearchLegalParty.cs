using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Implementation
{
	public class RebuildSearchLegalParty : IRebuildSearchLegalParty
	{
		private readonly IAumentumRepository _aumentumRepository;
		private readonly IRebuildSearchLegalPartyIndexRepository _searchLegalPartyRepository;
		private readonly ILogger _logger;

		public RebuildSearchLegalParty(
			IAumentumRepository aumentumRepository,
			IRebuildSearchLegalPartyIndexRepository searchLegalPartyRepository,
			ILogger logger)
		{
			_aumentumRepository = aumentumRepository;
			_searchLegalPartyRepository = searchLegalPartyRepository;
			_logger = logger;
		}

		public async Task DoAsync(RebuildSearchLegalPartyDto rebuildSearchLegalPartyDto)
		{
			var list = GetLegalPartyIdList(rebuildSearchLegalPartyDto).Distinct().ToList();

			_logger.LogDebug($"Found {list.Count} Legal Parties");

			_logger.LogDebug("Rebuilding LegalPartyId from list.");
			await _searchLegalPartyRepository.RebuildSearchLegalPartyIndexByLegalPartyId(list);
			_logger.LogDebug("LegalPartyId is rebuilt from list.");
		}

		public async Task DoAsync()
		{
			_logger.LogDebug($"Rebuilding all LegalPartyIds.");
			await _searchLegalPartyRepository.RebuildSearchLegalPartyIndexAll();
			_logger.LogDebug("All LegalPartyIds are rebuilt.");
		}

		private List<int> GetLegalPartyIdList(RebuildSearchLegalPartyDto rebuildSearchLegalPartyDto)
		{
			var idList = new List<int>();

			if (rebuildSearchLegalPartyDto.LegalPartyIdList != null)
				idList.AddRange(rebuildSearchLegalPartyDto.LegalPartyIdList);

			rebuildSearchLegalPartyDto.CommIdList?.ForEach(commId =>
			{
				idList.AddRange(_aumentumRepository.GetLegalPartyIdByCommId(commId));
			});

			rebuildSearchLegalPartyDto.RevenueObjectIdList?.ForEach(revenueObjectId =>
			{
				idList.AddRange(_aumentumRepository.GetLegalPartyIdByRevenueObjectId(revenueObjectId));
			});

			rebuildSearchLegalPartyDto.SitusAddressIdList?.ForEach(situsAddressId =>
			{
				idList.AddRange(_aumentumRepository.GetLegalPartyIdBySitusAddressId(situsAddressId));
			});

			rebuildSearchLegalPartyDto.TaxAuthorityGroupIdList?.ForEach(taxAuthorityGroupId =>
			{
				idList.AddRange(_aumentumRepository.GetLegalPartyIdByTaxAuthorityGroupId(taxAuthorityGroupId));
			});

			rebuildSearchLegalPartyDto.AppraisalSiteIdList?.ForEach(appraisalSiteId =>
			{
				idList.AddRange(_aumentumRepository.GetLegalPartyIdByAppraisalSiteId(appraisalSiteId));
			});

			return idList;
		}
	}
}
