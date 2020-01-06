using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAGov.Common.Paging;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class ContainsSearchLegalPartyRepository : ISearchLegalPartyRepository
	{
		private readonly PagingInfo _pagingInfo;
		private readonly ILogger _logger;
		private readonly SearchLegalPartyContext _context;

		public ContainsSearchLegalPartyRepository(SearchLegalPartyContext context, IPagingInfo pagingInfo, ILogger logger,
												  ICommandTimeoutConfiguration commandTimeoutConfiguration)
		{
			_pagingInfo = (PagingInfo)pagingInfo;
			_logger = logger;
			_context = context;
			_context.Database.SetCommandTimeout(commandTimeoutConfiguration.CommandTimeout);
		}

		public string ProviderName => "ContainsSearch";

		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions)
		{
			return await SearchAsync(searchText, searchLegalPartyQueryExclusions, null, null, false);
		}

		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate)
		{
			return await SearchAsync(searchText, searchLegalPartyQueryExclusions, isActive, effectiveDate, false);
		}

		private string AddExclusions(SearchLegalPartyQuery searchLegalPartyQueryExclusions)
		{
			var builder = new StringBuilder();
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeAddress, "Addr", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeAin, "AIN", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeDisplayName, "DisplayName", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeGeoCode, "GeoCode", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludePin, "PIN", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludePin, "UnformattedPIN", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeSearchAll, "SearchAll", builder);
			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeTag, "Tag", builder);

			var contains = builder.ToString();
			var exclusions = contains.Substring(0, contains.Length - 1);
			return !string.IsNullOrEmpty(exclusions) ? "(" + exclusions + ")" : "";
		}

		private string AddLikeClauses(SearchLegalPartyQuery searchLegalPartyQueryExclusions, string searchText)
		{
			string likeClause = " LIKE '%' + @searchText OR ";
			string exclusionlist = AddExclusions(searchLegalPartyQueryExclusions);
			var likeClauses = string.IsNullOrEmpty(exclusionlist) ? string.Empty : exclusionlist.Replace(",", likeClause).Replace(")", " LIKE '%' + @searchText )");
			return likeClauses;
		}

		private const string SearchSql =
			@"
			DECLARE @maxRows int = {0}

			IF OBJECT_ID('tempdb..#tempResults') IS NOT NULL
			drop table #tempResults

			SELECT FT_TBL.* INTO #tempResults
			FROM [search].[LegalPartySearch] AS FT_TBL  
			INNER JOIN CONTAINSTABLE([search].[LegalPartySearch], {1},  @searchText ) AS KEY_TBL ON FT_TBL.Id = KEY_TBL.[KEY] {2}
			ORDER BY KEY_TBL.RANK DESC;
		";

		private const string PinSearchSql = @"
			SELECT TOP (@maxRows) tr.* FROM #tempResults tr 
				WHERE tr.legalpartyid in (SELECT distinct legalpartyid FROM #tempResults WHERE BegEffDate = (SELECT MAX(BegEffDate) from #tempResults {0} )) {1}
		";

		private const string NonePinSearchSql = @"
			IF OBJECT_ID('tempdb..#tempLegalPartiesWithMaxBegEffDate') IS NOT NULL
			drop table #tempLegalPartiesWithMaxBegEffDate

			SELECT LegalPartyRoleId, MAX(BegEffDate) As MaxBeginEffDate
			INTO #tempLegalPartiesWithMaxBegEffDate
			FROM #tempResults
			GROUP BY LegalPartyRoleId 
						
			SELECT TOP (@maxRows) tr.* FROM #tempResults tr 
				LEFT JOIN #tempLegalPartiesWithMaxBegEffDate lp ON lp.LegalPartyRoleId = tr.LegalPartyRoleId AND tr.BegEffDate = lp.MaxBeginEffDate
		";

		private void AppendIfFalse(bool? value, string fieldName, StringBuilder builder)
		{
			if (value != true)
			{
				builder.Append($"{fieldName},");
			}
		}

		private string AddFilters(
			List<SqlParameter> parameters,
			SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool isPinWithSitusAsAddressType,
			bool? isActive, DateTime? effectiveDate)
		{
			var builder = new StringBuilder();
			if (isActive.HasValue)
			{
				builder.Append(" AND EffStatus=@isActive");
				parameters.Add(new SqlParameter("@isActive", SqlDbType.VarChar, 1) { Value = isActive.Value ? 'A' : 'I' });
				_logger.LogDebug($"@isActive={isActive}");
			}

			if (effectiveDate.HasValue)
			{
				builder.Append(" AND (BegEffDate<=@effectiveDate OR BegEffDate IS NULL)");
				parameters.Add(new SqlParameter("@effectiveDate", SqlDbType.DateTime) { Value = effectiveDate.Value });
				_logger.LogDebug($"@effectiveDate={effectiveDate}");
			}

			if (searchLegalPartyQueryExclusions.RevenueObjectIdIsNotNull == true)
			{
				builder.Append(" AND RevObjId IS NOT NULL");
			}

			if (searchLegalPartyQueryExclusions.AppraisalSiteIdIsNotNull == true)
			{
				builder.Append(" AND AppraisalSiteId IS NOT NULL");
			}

			if (!string.IsNullOrEmpty(searchLegalPartyQueryExclusions.AddressType))
			{
				builder.Append(" AND AddrType = @AddressType");
				parameters.Add(new SqlParameter("@AddressType", SqlDbType.VarChar, 10) { Value = searchLegalPartyQueryExclusions.AddressType });
				_logger.LogDebug($"@AddressType={searchLegalPartyQueryExclusions.AddressType}");

				if (isPinWithSitusAsAddressType)
				{
					builder.Append(" AND (PrimeAddress = @isPrimeAddress OR PrimeAddress IS NULL)");
					parameters.Add(new SqlParameter("@isPrimeAddress", SqlDbType.Bit) { Value = 1 });

					builder.Append(" AND PrimeOwner = @isPrimeOwner");
					parameters.Add(new SqlParameter("@isPrimeOwner", SqlDbType.Bit) { Value = 1 });

					builder.Append(" AND (AddrRoleEffStatus = @addrRoleEffStatus OR AddrRoleEffStatus IS NULL)");
					parameters.Add(new SqlParameter("@addrRoleEffStatus", SqlDbType.Char) { Value = "A" });
				}
			}

			if (searchLegalPartyQueryExclusions.MineralIsNotNullWithValue.HasValue)
			{
				builder.Append(" AND Mineral = @isMineral");
				parameters.Add(new SqlParameter("@isMineral", SqlDbType.Bit) { Value = (bool)searchLegalPartyQueryExclusions.MineralIsNotNullWithValue });
				_logger.LogDebug($"@isMineral={searchLegalPartyQueryExclusions.MineralIsNotNullWithValue.Value}");
			}

			return builder.ToString();
		}

		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate, bool isPin)
		{
			var isPinWithSitusAsAddressType = isPin && searchLegalPartyQueryExclusions.AddressType == "situs";
			var likeClauses = this.AddLikeClauses(searchLegalPartyQueryExclusions, searchText);
			var updatedPinSearchSql = string.Format(PinSearchSql, string.IsNullOrEmpty(likeClauses) ? likeClauses : $" WHERE {likeClauses}",
														string.IsNullOrEmpty(likeClauses) ? likeClauses : $" AND {likeClauses}");
			var appendSearchSql = isPin ? updatedPinSearchSql : NonePinSearchSql;
			var parameters = new List<SqlParameter>();
			var sql = string.Format($"{SearchSql}\r\n\r\n{appendSearchSql}", _pagingInfo.MaxRows, AddExclusions(searchLegalPartyQueryExclusions),
				AddFilters(parameters, searchLegalPartyQueryExclusions, isPinWithSitusAsAddressType, isActive, effectiveDate));

			searchText = isPin ? searchText : new ContainSearchTextSqlTransformer(
				searchText, null).GetSqlFriendly();

			parameters.Add(new SqlParameter("@searchText", SqlDbType.VarChar, 200) { Value = searchText });

			_logger.LogDebug($"@searchText={searchText}");
			_logger.LogDebug($"@maxRows={_pagingInfo.MaxRows}");

			var results = (await SqlSearchAsync(sql, parameters)).ToList();

			return results;
		}


		public Task<IEnumerable<SearchLegalParty>> SearchPinAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive = true, DateTime? effectiveDate = null)
		{
			return SearchAsync(searchText, searchLegalPartyQueryExclusions, isActive, effectiveDate, true);
		}

		private async Task<IEnumerable<SearchLegalParty>> SqlSearchAsync(string sql, List<SqlParameter> sqlParameters)
		{
			// ReSharper disable once CoVariantArrayConversion
			return await _context.LegalParties.FromSql(sql, sqlParameters.ToArray()).ToListAsync();
		}
	}
}

