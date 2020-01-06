using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;

namespace TAGov.Services.Core.LegalParty.Domain.Interfaces
{
  public interface ILegalPartyOfficialDocumentDomain
  {
    Task<IEnumerable<LegalPartyDocumentDto>> ListAsync( IEnumerable<int> legalPartyRoleIdList, DateTime effectiveDate );
  }
}
