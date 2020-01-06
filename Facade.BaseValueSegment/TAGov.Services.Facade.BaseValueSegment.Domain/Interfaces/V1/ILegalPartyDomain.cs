using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface ILegalPartyDomain
  {
    Task<IEnumerable<LegalPartyDocumentDto>> GetLegalPartyRoleDocuments( BaseValueSegmentDto baseValueSegmentDto );

    Task<LegalPartyRoleDto> GetLegalPartyRole( int legalPartyRoleId, DateTime effectiveDateTime );
  }
}