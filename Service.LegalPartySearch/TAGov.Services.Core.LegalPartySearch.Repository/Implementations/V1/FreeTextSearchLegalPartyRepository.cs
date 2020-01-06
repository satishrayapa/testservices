//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using TAGov.Common.Paging;
//using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
//using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

//namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
//{
//	public class FreeTextSearchLegalPartyRepository : ISearchLegalPartyRepository
//	{
//		private readonly SearchLegalPartyContext _context;
//		private readonly ILogger _logger;
//		private readonly PagingInfo _pagingInfo;

//		public FreeTextSearchLegalPartyRepository(
//			SearchLegalPartyContext context,
//			ILogger logger,
//			IPagingInfo pagingInfo)
//		{
//			_context = context;
//			_logger = logger;
//			_pagingInfo = (PagingInfo)pagingInfo;
//		}

//		public string ProviderName => "FreeTextSearch";

//		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions)
//		{
//			return await SearchAsync(searchText, searchLegalPartyQueryExclusions, null, null);
//		}

//		private string AddExclusions(SearchLegalPartyQuery searchLegalPartyQueryExclusions)
//		{
//			var builder = new StringBuilder();
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeAddress, "Addr", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeAin, "AIN", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeDisplayName, "DisplayName", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeGeoCode, "GeoCode", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludePin, "PIN", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeSearchAll, "SearchAll", builder);
//			AppendIfFalse(searchLegalPartyQueryExclusions.ExcludeTag, "Tag", builder);

//			var contains = builder.ToString();
//			var exclusions = contains.Substring(0, contains.Length - 1);
//			return !string.IsNullOrEmpty(exclusions) ? "(" + exclusions + ")" : "";
//		}

//		private const string FreeTextTable = "FREETEXTTABLE";
//		private const string ContainsTable = "CONTAINSTABLE";


//		private const string SearchSql =
//			@"
//			DECLARE @maxRows int = {0}
//			--DECLARE @count int

//			--IF @maxRows < 100000
//			--BEGIN

//			--	SET @count = (SELECT COUNT (1)
//			--	FROM [search].[LegalPartySearch] AS FT_TBL  
//			--	INNER JOIN {1}([search].[LegalPartySearch], {2}, @searchText) AS KEY_TBL ON FT_TBL.Id = KEY_TBL.[KEY] {3}); 
			
//			--	IF @count > @maxRows SET @maxRows = @count

//			--END

//			SELECT TOP (@maxRows) FT_TBL.*
//			FROM [search].[LegalPartySearch] AS FT_TBL  
//			INNER JOIN {1}([search].[LegalPartySearch], {2}, @searchText) AS KEY_TBL ON FT_TBL.Id = KEY_TBL.[KEY] {3}
//			ORDER BY KEY_TBL.RANK DESC; 
//		";


//		//private const string SearchSql =
//		//@"
//		//	SELECT TOP {0} FT_TBL.*
//		//	FROM [search].[LegalPartySearch] AS FT_TBL  
//		//	INNER JOIN {1}([search].[LegalPartySearch], {2},  @searchText ) AS KEY_TBL ON FT_TBL.Id = KEY_TBL.[KEY] {3}
//		//	ORDER BY KEY_TBL.RANK DESC; 
//		//";

//		private string AddFilters(
//			List<SqlParameter> parameters,
//			SearchLegalPartyQuery searchLegalPartyQueryExclusions,
//			bool? isActive, DateTime? effectiveDate)
//		{
//			var builder = new StringBuilder();
//			if (isActive.HasValue)
//			{
//				builder.Append(" AND EffStatus=@isActive");
//				parameters.Add(new SqlParameter("@isActive", SqlDbType.VarChar, 1) { Value = isActive.Value ? 'A' : 'I' });
//				_logger.LogDebug($"@isActive={isActive}");
//			}

//			if (effectiveDate.HasValue)
//			{
//				builder.Append(" AND BegEffDate<=@effectiveDate");
//				parameters.Add(new SqlParameter("@effectiveDate", SqlDbType.DateTime) { Value = effectiveDate.Value });
//				_logger.LogDebug($"@effectiveDate={effectiveDate}");
//			}

//			if (searchLegalPartyQueryExclusions.RevenueObjectIdIsNotNull == true)
//			{
//				builder.Append(" AND RevObjId IS NOT NULL");
//			}

//			if (searchLegalPartyQueryExclusions.AppraisalSiteIdIsNotNull == true)
//			{
//				builder.Append(" AND AppraisalSiteId IS NOT NULL");
//			}

//			if (!string.IsNullOrEmpty(searchLegalPartyQueryExclusions.AddressType))
//			{
//				builder.Append(" AND AddrType = @AddressType");
//				parameters.Add(new SqlParameter("@AddressType", SqlDbType.VarChar, 10) { Value = searchLegalPartyQueryExclusions.AddressType });
//				_logger.LogDebug($"@AddressType={searchLegalPartyQueryExclusions.AddressType}");
//			}

//			if (searchLegalPartyQueryExclusions.MineralIsNotNullWithValue.HasValue)
//			{
//				builder.Append(" AND Mineral = @isMineral");
//				parameters.Add(new SqlParameter("@isMineral", SqlDbType.Bit) { Value = (bool)searchLegalPartyQueryExclusions.MineralIsNotNullWithValue });
//				_logger.LogDebug($"@isMineral={searchLegalPartyQueryExclusions.MineralIsNotNullWithValue.Value}");
//			}

//			return builder.ToString();
//		}

//		private void AppendIfFalse(bool? value, string fieldName, StringBuilder builder)
//		{
//			if (value != true)
//			{
//				builder.Append($"{fieldName},");
//			}
//		}

//		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate)
//		{
//			return await SearchAsync(searchText, searchLegalPartyQueryExclusions, isActive, effectiveDate, false);

//		}

//		public async Task<IEnumerable<SearchLegalParty>> SearchAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate, bool isPin)
//		{
//			var parameters = new List<SqlParameter>();

//			string sql = isPin ?
//				string.Format(SearchSql, _pagingInfo.MaxRows, ContainsTable, AddExclusions(searchLegalPartyQueryExclusions), AddFilters(parameters, searchLegalPartyQueryExclusions, isActive, effectiveDate)) :
//				string.Format(SearchSql, _pagingInfo.MaxRows, FreeTextTable, AddExclusions(searchLegalPartyQueryExclusions), AddFilters(parameters, searchLegalPartyQueryExclusions, isActive, effectiveDate));

//			parameters.Add(new SqlParameter("@searchText", SqlDbType.VarChar, 200) { Value = searchText });

//			_logger.LogDebug($"@searchText={searchText}");
//			_logger.LogDebug($"@maxRows={_pagingInfo.MaxRows}");

//			return await SqlSearchAsync(sql, parameters);
//		}

//		public Task<IEnumerable<SearchLegalParty>> SearchPinAsync(string searchText, SearchLegalPartyQuery searchLegalPartyQueryExclusions, bool? isActive, DateTime? effectiveDate)
//		{
//			searchLegalPartyQueryExclusions.ExcludeAddress = true;
//			searchLegalPartyQueryExclusions.ExcludeDisplayName = true;
//			searchLegalPartyQueryExclusions.ExcludeAin = false;
//			searchLegalPartyQueryExclusions.ExcludePin = false;
//			searchLegalPartyQueryExclusions.ExcludeSearchAll = false;
//			searchLegalPartyQueryExclusions.ExcludeTag = false;

//			return SearchAsync(searchText, searchLegalPartyQueryExclusions, isActive, effectiveDate, true);
//		}

//		private async Task<IEnumerable<SearchLegalParty>> SqlSearchAsync(string sql, List<SqlParameter> sqlParameters)
//		{
//			// ReSharper disable once CoVariantArrayConversion
//			return await _context.LegalParties.FromSql(sql, sqlParameters.ToArray()).ToListAsync();
//		}
//	}
//}
