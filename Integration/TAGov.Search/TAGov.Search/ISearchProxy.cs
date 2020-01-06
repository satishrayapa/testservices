using System.Collections.Generic;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Search
{
	public interface ISearchProxy
	{
		bool CanHandle(Features feature);
		IEnumerable<SearchLegalPartyDto> Search(SearchLegalPartyQueryDto searchLegalPartyQueryDto);
	}
}