using System.Threading.Tasks;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1
{
   public interface ISearchOperations
   {
	   Task EnableStopwords();

	   Task DisableStopwords();
	}
}
