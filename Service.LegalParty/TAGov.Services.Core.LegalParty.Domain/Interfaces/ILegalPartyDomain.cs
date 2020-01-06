using System;
using System.Collections.Generic;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;

namespace TAGov.Services.Core.LegalParty.Domain.Interfaces
{
  public interface ILegalPartyDomain
  {
    IEnumerable<LegalPartyRoleDto> GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate );

    IEnumerable<LegalPartyRoleDto> GetLegalPartyRolesById( int[] legalPartyRoleIdList );
  }
}
