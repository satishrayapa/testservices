using System;
using System.Collections.Generic;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository.Interfaces
{
  public interface ILegalPartyRepository
  {
    Models.V1.LegalParty GetByEffectiveDate( int legalPartyRoleId, DateTime effectDate );

    IEnumerable<LegalPartyRole> GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate );

    IEnumerable<LegalPartyRole> GetLegalPartyRolesById( int[] legalPartyRoleIdList );
  }
}
