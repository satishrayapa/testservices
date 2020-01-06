using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class SearchOperations : ISearchOperations
	{
		private readonly AumentumContext _aumentumContext;

		public SearchOperations(AumentumContext aumentumContext)
		{
			_aumentumContext = aumentumContext;
		}

		public async Task EnableStopwords()
		{
			await _aumentumContext.Database.ExecuteSqlCommandAsync("ALTER FULLTEXT INDEX ON [search].[LegalPartySearch] SET STOPLIST = SYSTEM");
		}

		public async Task DisableStopwords()
		{
			await _aumentumContext.Database.ExecuteSqlCommandAsync("ALTER FULLTEXT INDEX ON [search].[LegalPartySearch] SET STOPLIST = OFF");
		}
	}
}
