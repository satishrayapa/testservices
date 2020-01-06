using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Constants;

namespace TAGov.Services.Core.LegalParty.Repository.Implementation
{
  public class GrmEventRightTransferRepository : IGrmEventRightTransferRepository
  {
    private readonly LegalPartyContext _legalPartyContext;

    public GrmEventRightTransferRepository( LegalPartyContext legalPartyContext )
    {
      _legalPartyContext = legalPartyContext;
    }

    public async Task<IEnumerable<GrmEventRightTransfer>> ListAsync( IEnumerable<int> rightTransferIdList )
    {
      return await ( from grmEventArtifacts in _legalPartyContext.GrmEventArtifacts
                     join grmEvents in _legalPartyContext.GrmEvents on grmEventArtifacts.GrmEventId equals grmEvents.Id
                     where grmEvents.EventType == SysType.TransferId
                           && grmEventArtifacts.ObjectType == SysType.RightTransferId
                           && rightTransferIdList.Contains( grmEventArtifacts.ObjectId )
                     select new GrmEventRightTransfer
                            {
                              GrmEventId = grmEventArtifacts.GrmEventId,
                              RightTransferId = grmEventArtifacts.ObjectId
                            } ).GroupBy( x => x.RightTransferId ).Select( x => new GrmEventRightTransfer
                                                                               {
                                                                                 RightTransferId = x.Key,
                                                                                 GrmEventId = x.Max( y => y.GrmEventId )
                                                                               } ).ToListAsync();
    }
  }
}
