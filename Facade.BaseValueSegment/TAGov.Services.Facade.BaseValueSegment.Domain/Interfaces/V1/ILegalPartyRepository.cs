using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface ILegalPartyRepository
  {
    Task<IEnumerable<LegalPartyDocumentDto>> SearchAsync( LegalPartySearchDto legalPartySearchDto );

    Task<IEnumerable<LegalPartyRoleDto>> GetLegalPartyRoles( int[] legalPartyRoleIds );
  }
}