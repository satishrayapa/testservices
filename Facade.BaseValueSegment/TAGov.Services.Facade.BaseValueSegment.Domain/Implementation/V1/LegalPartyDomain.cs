using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class LegalPartyDomain : ILegalPartyDomain
  {
    private readonly ILegalPartyRepository _legalPartyRepository;

    public LegalPartyDomain( ILegalPartyRepository legalPartyRepository )
    {
      _legalPartyRepository = legalPartyRepository;
    }

    public async Task<IEnumerable<LegalPartyDocumentDto>> GetLegalPartyRoleDocuments( BaseValueSegmentDto baseValueSegmentDto )
    {
      var owners = baseValueSegmentDto.BaseValueSegmentTransactions.SelectMany( t => t.BaseValueSegmentOwners );

      var legalPartyRoleIds = owners.Select( owner => owner.LegalPartyRoleId ).Distinct();

      return await GetLegalPartyRoleDocuments( legalPartyRoleIds, baseValueSegmentDto.AsOf );
    }


    private async Task<IEnumerable<LegalPartyDocumentDto>> GetLegalPartyRoleDocuments( IEnumerable<int> legalPartyRoleIdList, DateTime asOf )
    {
      // build search object for retrieving the legal parties associated to this base value segment
      var legalPartySearchDto = new LegalPartySearchDto();

      legalPartySearchDto.LegalPartyRoleIdList.AddRange( legalPartyRoleIdList );

      // changed to bvs asof date from the assesment date because it doesnt work for previous bvs
      legalPartySearchDto.EffectiveDate = asOf;

      // call service with search object to get legal parties associated to this base value segment
      return await _legalPartyRepository.SearchAsync( legalPartySearchDto );
    }

    public async Task<LegalPartyRoleDto> GetLegalPartyRole( int legalPartyRoleId, DateTime effectiveDate )
    {
      var legalPartyRoles = await _legalPartyRepository.GetLegalPartyRoles( new int[] { legalPartyRoleId } );

      return legalPartyRoles.FirstOrDefault( lpr => lpr.BegEffDate < effectiveDate.AddDays( 1 ) );
    }
  }
}