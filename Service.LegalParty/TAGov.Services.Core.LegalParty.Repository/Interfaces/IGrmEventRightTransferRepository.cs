using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository.Interfaces
{
  public interface IGrmEventRightTransferRepository
  {
    Task<IEnumerable<GrmEventRightTransfer>> ListAsync( IEnumerable<int> rightTransferIdList );
  }
}
