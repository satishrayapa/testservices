using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Interfaces
{
	public interface ISearchLegalPartyDomain
	{
		Task<IEnumerable<SearchLegalPartyDto>> SearchAsync(SearchLegalPartyQueryDto searchLegalPartyQueryDto);
	}
}
