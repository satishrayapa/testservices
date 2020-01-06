using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository.Interfaces
{
  public interface IOfficialDocumentShortDescriptionRepository
  {
    Task<IEnumerable<OfficialDocumentShortDescription>> ListAsync( IEnumerable<int> documentIdList );
  }
}
