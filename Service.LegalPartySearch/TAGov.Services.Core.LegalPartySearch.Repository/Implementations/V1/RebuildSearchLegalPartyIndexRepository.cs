using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class RebuildSearchLegalPartyIndexRepository : IRebuildSearchLegalPartyIndexRepository
	{
		private readonly SearchLegalPartyContext _context;
		private readonly ILogger _logger;
		private readonly int? _timeout;

		public RebuildSearchLegalPartyIndexRepository(
			SearchLegalPartyContext context, ILogger logger, ICommandTimeoutConfiguration commandTimeoutConfiguration)
		{
			_context = context;
			_logger = logger;
			_timeout = commandTimeoutConfiguration.CommandTimeout;
		}

		public async Task RebuildSearchLegalPartyIndexByLegalPartyId(IEnumerable<int> legalPartyIdList)
		{
			_context.Database.SetCommandTimeout(_timeout);

			var idList = string.Join(",", legalPartyIdList);

			_logger.LogDebug($"Id List: {idList}");

			var sql = string.Format(RebuildSearchLegalPartyIndex.GetRebuildVersion(), idList);

			//File.WriteAllText(@"d:\rebuildQueryFinal.sql", sql);

			await _context.Database.ExecuteSqlCommandAsync(sql);
		}

		public async Task RebuildSearchLegalPartyIndexAll()
		{
			_context.Database.SetCommandTimeout(_timeout);
			try
			{
				await _context.Database.ExecuteSqlCommandAsync(RebuildSearchLegalPartyIndex.GetRebuildAllVersion());
			}
			catch (SqlException e)
			{
				if (e.Number != int.Parse(RebuildSearchLegalPartyIndex.SqlErrorNumberForLocked)) throw;
				_logger.LogInformation(e.Message);
				throw new LockedException(e.Message);
			}
		}
		public void RebuildAll()
		{
			_context.Database.SetCommandTimeout(_timeout);
			try
			{
				_context.Database.ExecuteSqlCommand(UpdatedSeedQuery.RebuildSeedQuery());

			}
			catch (SqlException e)
			{
				if (e.Number != int.Parse(RebuildSearchLegalPartyIndex.SqlErrorNumberForLocked)) throw;
				_logger.LogInformation(e.Message);
				throw new LockedException(e.Message);
			}
		}


		public CrawlProgress CrawlProgress()
		{
			_context.Database.SetCommandTimeout(_timeout);

			return _context.Crawls.FromSql(UpdatedSeedQuery.GetCrawlProgress()).FirstOrDefault();
		}

		public void EnableChangeTracking()
		{
			_context.Database.SetCommandTimeout(_timeout);
			try
			{
				_context.Database.ExecuteSqlCommand(ChangeTracking.ChangeTrackingTablesEnable());

			}
			catch (SqlException e)
			{
				if (e.Number != int.Parse(RebuildSearchLegalPartyIndex.SqlErrorNumberForLocked)) throw;
				_logger.LogInformation(e.Message);
				throw new LockedException(e.Message);
			}
		}

		public void DisableChangeTracking()
		{
			_context.Database.SetCommandTimeout(_timeout);
			try
			{
				_context.Database.ExecuteSqlCommand(ChangeTracking.ChangeTrackingTablesDisable());

			}
			catch (SqlException e)
			{
				if (e.Number != int.Parse(RebuildSearchLegalPartyIndex.SqlErrorNumberForLocked)) throw;
				_logger.LogInformation(e.Message);
				throw new LockedException(e.Message);
			}
		}


	}
}
