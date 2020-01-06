using System.Threading.Tasks;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Interfaces
{
	public interface IRebuildSearchLegalParty
	{
		Task DoAsync(RebuildSearchLegalPartyDto rebuildSearchLegalPartyDto);
		Task DoAsync();
	}
}
