using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FastMember;
using Microsoft.Extensions.Logging;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Domain.Mappings;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;
using TAGov.Common.Paging;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;


namespace TAGov.Services.Core.LegalPartySearch.Domain.Implementation
{
	public class SearchLegalPartyDomain : ISearchLegalPartyDomain
	{
		private readonly ISearchProviderSelector _searchProviderSelector;
		private readonly ILogger _logger;
		private readonly PagingInfo _pagingInfo;
		private readonly ISearchResultsConfiguration _searchResultsConfiguration;

		public SearchLegalPartyDomain(ISearchProviderSelector searchProviderSelector, ILogger logger, 
			IPagingInfo pagingInfo, ISearchResultsConfiguration searchResultsConfiguration)
		{
			_searchProviderSelector = searchProviderSelector;
			_logger = logger;
			_pagingInfo = (PagingInfo)pagingInfo;
			_searchResultsConfiguration = searchResultsConfiguration;
		}

		private void CacheMaxRowIfMaxRowIsValid(int maxRow)
		{
			if (maxRow > 0)
				_pagingInfo.OverrideMaxRowsValue(maxRow);
		}

		public async Task<IEnumerable<SearchLegalPartyDto>> SearchAsync(SearchLegalPartyQueryDto legalPartySearchDto)
		{
			if (string.IsNullOrEmpty(legalPartySearchDto.SearchText))
				throw new BadRequestException("SearchText cannot be null.");

			if (legalPartySearchDto.MaxRows > _searchResultsConfiguration.MaxRows)
				legalPartySearchDto.MaxRows = _searchResultsConfiguration.MaxRows;

			legalPartySearchDto.SearchText = legalPartySearchDto.SearchText.Trim();

			ThrowBadRequestIfAllAreExcluded(legalPartySearchDto);

			var searchProvider = _searchProviderSelector.Get(legalPartySearchDto.SearchText);

			CacheMaxRowIfMaxRowIsValid(legalPartySearchDto.MaxRows);

			IEnumerable<SearchLegalParty> results = null;

			var isPin = IsPin(legalPartySearchDto.SearchText, new List<SearchLegalParty>());

			if (isPin)
			{
				var searchLegalPartyQueryExclusions = legalPartySearchDto.ToExclusions();

				searchLegalPartyQueryExclusions.ExcludeAddress = true;
				searchLegalPartyQueryExclusions.ExcludeDisplayName = true;
				searchLegalPartyQueryExclusions.ExcludeAin = false;
				searchLegalPartyQueryExclusions.ExcludePin = false;
				searchLegalPartyQueryExclusions.ExcludeSearchAll = true;
				searchLegalPartyQueryExclusions.ExcludeTag = false;
				bool isActive = legalPartySearchDto.IsActive ?? true;

				results = await searchProvider.Provider
					.SearchPinAsync(searchProvider.ParsedSearchText, searchLegalPartyQueryExclusions, isActive, legalPartySearchDto.EffectiveDate);
			}
			else
			{
				if (legalPartySearchDto.EffectiveDate.HasValue || legalPartySearchDto.IsActive.HasValue)
				{
					results = await searchProvider.Provider.SearchAsync(
										searchProvider.ParsedSearchText,
										legalPartySearchDto.ToExclusions(),
										legalPartySearchDto.IsActive,
										legalPartySearchDto.EffectiveDate);
				}
				else
				{
					results = await searchProvider.Provider.SearchAsync(
										searchProvider.ParsedSearchText,
										legalPartySearchDto.ToExclusions());
				}
			}

			results = SearchLegalParties(searchProvider.ParsedSearchText, legalPartySearchDto.ToExclusions(), legalPartySearchDto.EffectiveDate, results.ToList());

			var records = results.Select(x => x.ToDomain()).ToList();
			_logger.LogDebug($"Found {records.Count} records.");
			return records;
		}

		private void ThrowBadRequestIfAllAreExcluded(SearchLegalPartyQueryDto legalPartySearchDto)
		{
			var accessor = TypeAccessor.Create(typeof(SearchLegalPartyQueryDto));
			var members = accessor.GetMembers().Where(x => x.Type == typeof(bool?) && x.Name.StartsWith("Exclude")).ToList();

			var totalExclusions = members.Count(x => (bool?)accessor[legalPartySearchDto, x.Name] == true);

			if (totalExclusions == members.Count)
			{
				throw new BadRequestException("All search parameters are excluded. At least one search parameter must be enabled.");
			}
		}

		private bool IsPin(string searchText, List<SearchLegalParty> results)
		{
			var regex = new Regex(@"^\w[0-9\-]{2,}$");
			return (regex.IsMatch(searchText) || (results.Count(x => (string.Equals(x.UnformattedPin, searchText, StringComparison.OrdinalIgnoreCase)) || (string.Equals(x.Pin, searchText, StringComparison.OrdinalIgnoreCase))) > 0));
		}

		// refactored from the repository ///////////////////////////////////////////


		private IEnumerable<SearchLegalParty> SearchLegalParties(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions,
			DateTime? effectiveDate, List<SearchLegalParty> results)
		{
			// isPin() function return value could change based on the search results
			bool isPin = IsPin(searchText, results);
			string effStatusActive = "A";
			var isPinWithSitusAsAddressType = isPin && searchLegalPartyQueryExclusions.AddressType == "situs";
			searchText = isPin ? searchText : new ContainSearchTextSqlTransformer(searchText, null).GetSqlFriendly();

			var searchCompText = searchText.Replace("\"", "").Trim();

			_logger.LogDebug($"Records returned from database before filtering: {results.Count}");

			if (isPin)
			{
				// if it is a pin refilter results for only active legal party roles
				results = results.Where(r => r.EffectiveStatus == effStatusActive).ToList();

				if (searchText.StartsWith("0"))
				{
					// filter for leading zero pin results
					results = results.Where(r => r.Pin.ToLower().EndsWith(searchText.ToLower())).ToList();
				}
			}

			if (!isPinWithSitusAsAddressType)
			{
				if (isPin &&
					(searchLegalPartyQueryExclusions.AddressType == "situs"))
				{
					isPinWithSitusAsAddressType = true;
				}
			}

			if (isPinWithSitusAsAddressType)
			{
				var list = new List<SearchLegalParty>();
				results = results.Where(x => x.PrimeOwner == true).ToList();//returning only primeowner
				results.GroupBy(x => new { x.RevenueObjectId, x.LegalPartyRole }).ToList().ForEach(g =>
				{
					var maxEffectiveDate = g.Max(x => x.AddressRoleEffectiveDate);

					if (maxEffectiveDate.HasValue)
					{
						var legalPartyAddressesOfSameDate = g.Where(x =>
								x.AddressRoleEffectiveDate.HasValue &&
								x.AddressRoleEffectiveDate.Value == maxEffectiveDate.Value)
							.ToList();
						var maxId = legalPartyAddressesOfSameDate.Max(x => x.AddressRoleId);
						list.AddRange(legalPartyAddressesOfSameDate.Where(x => x.AddressRoleId == maxId));
					}
					else
					{
						// This is for Legal Party with RevObj with no situs addr.
						// Maybe this legal party does not yet have a situs addr assigned yet.
						list.AddRange(g);
					}
				});

				results = list;
			}

			results = DuplicateRevenueObjectCleanup(results, effectiveDate);

			if (searchLegalPartyQueryExclusions.RevenueObjectIsActive.HasValue)
			{
				var flag = searchLegalPartyQueryExclusions.RevenueObjectIsActive == true ? "A" : "I";

				results = results.Where(x => x.RevenueObjectEffectiveStatus == flag).ToList();
			}

			if (searchLegalPartyQueryExclusions.CoalesceIfDuplicateAddress.HasValue &&
				searchLegalPartyQueryExclusions.CoalesceIfDuplicateAddress == true)
			{
				results = DuplicateLegalPartyCleanup(results);
			}
			
			if (isPinWithSitusAsAddressType && (results.Count > 0)) //Possibly AIN, GEOCODE,TAG multiple result can be returned.Hence picking only the search result that is a pin.
			{
				if (results.Any(x => string.Equals(x.Pin, searchCompText, StringComparison.OrdinalIgnoreCase)))
				{
					var desiredResult = results.Where(x => string.Equals(x.Pin, searchCompText, StringComparison.OrdinalIgnoreCase)).ToList();
					foreach (var rec in desiredResult)
					{
						var latestPin = rec.PinLatest?.Trim();
						if (latestPin != searchCompText && latestPin != rec.Pin?.Trim())
							rec.Pin = $"{latestPin}({rec.Pin?.Trim()})";
					}
					
				}
			}

			if (!isPin)
			{
				//if search is not pin based, not showing the inactive owner
				foreach (var rec in results)
				{
					if (rec.EffectiveStatus != effStatusActive)
					{
						rec.Pin = string.Empty;
						rec.GeoCode = string.Empty;
						rec.Ain = string.Empty;
					}

				}
			}

			return results;
		}



		private List<SearchLegalParty> DuplicateRevenueObjectCleanup(List<SearchLegalParty> searchLegalParties, DateTime? effectiveDate)
		{
			var results = searchLegalParties.Where(x => !x.RevenueObjectId.HasValue).ToList();

			var listWithRevObjId = searchLegalParties.Where(x => x.RevenueObjectId.HasValue).ToList();

			if (listWithRevObjId.Count > 0)
			{
				var groups = listWithRevObjId.GroupBy(x => new { x.RevenueObjectId, x.LegalPartyId }).ToList();

				if (effectiveDate.HasValue)
				{
					groups.ForEach(g =>
					{
						results.AddRange(FilterByRevenueObjectMaxEffectiveDate(g.ToList()));
					});
				}
				else
				{
					results.AddRange(groups.Select(list => list.OrderByDescending(o => o.EffectiveDate).First()).ToList());
				}
			}

			results = FilterByLegalPartyMaxEffectiveDate(results);

			return results;
		}

		private List<SearchLegalParty> FilterByLegalPartyMaxEffectiveDate(List<SearchLegalParty> legalParties)
		{
			List<SearchLegalParty> results = new List<SearchLegalParty>();

			var lps = legalParties.GroupBy(x => new { x.LegalPartyId }).ToList();

			lps.ForEach(g =>
			{
				var lprevs = g.ToList().GroupBy(x => new { x.RevenueObjectId }).ToList();

				if (lprevs.Count == 1 && lprevs.First().Key.RevenueObjectId == null)
				{
					var lpSearchResult = lprevs.First().ToList()
						.FirstOrDefault(x => x.RevenueObjectId == lprevs[0].Key.RevenueObjectId);
					if (lpSearchResult != null)
						results.Add(lpSearchResult);
				}
			});

			var lprevobjs = legalParties.GroupBy(x => new { x.LegalPartyId, x.RevenueObjectId }).ToList();
			lprevobjs.ForEach(g =>
			{
				var legalParty = g.ToList();
				var max = legalParty.Max(x => x.EffectiveDate);
				var lpSearchResult = legalParty.FirstOrDefault(r =>
					r.EffectiveDate == max && r.RevenueObjectId == g.Key.RevenueObjectId && r.RevenueObjectId != null);
				if (lpSearchResult != null)
					results.Add(lpSearchResult);
			});

			return results;
		}

		private List<SearchLegalParty> DuplicateLegalPartyCleanup(List<SearchLegalParty> searchLegalParties)
		{
			var results = searchLegalParties.Distinct(new DistinctLegalPartyComparer()).ToList();

			return results;
		}

		private List<SearchLegalParty> FilterByRevenueObjectMaxEffectiveDate(List<SearchLegalParty> list)
		{
			// ReSharper disable once PossibleInvalidOperationException
			var max = list.Max(x => x.RevenueObjectEffectiveDate.Value);
			// ReSharper disable once PossibleInvalidOperationException
			return list.Where(r => r.RevenueObjectEffectiveDate.Value == max).ToList();
		}
	}
}
