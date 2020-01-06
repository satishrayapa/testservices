using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository.Implementation
{
  public class OfficialDocumentShortDescriptionRepository : IOfficialDocumentShortDescriptionRepository
  {
    private readonly LegalPartyContext _legalPartyContext;

    public OfficialDocumentShortDescriptionRepository( LegalPartyContext legalPartyContext )
    {
      _legalPartyContext = legalPartyContext;
    }

    public async Task<IEnumerable<OfficialDocumentShortDescription>> ListAsync( IEnumerable<int> documentTypeIdList )
    {
      return await ( from sys in _legalPartyContext.SystemType
                     where documentTypeIdList.Contains( sys.Id ) &&
                           sys.BegEffDate == ( from maxSys in _legalPartyContext.SystemType
                                               where maxSys.Id == sys.Id
                                               select maxSys ).Max( new Func<SystemType, DateTime?>( sysType => sysType.BegEffDate ) )
                     select new OfficialDocumentShortDescription
                            {
                              DocumentTypeId = sys.Id,
                              ShortDescription = sys.ShortDescr.TrimEnd()
                            } ).ToListAsync();
    }
  }
}
