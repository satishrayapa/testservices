using System;
using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalParty.Domain.Interfaces;
using TAGov.Services.Core.LegalParty.Domain.Mapping;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;

namespace TAGov.Services.Core.LegalParty.Domain.Implementation
{
  public class LegalPartyDomain : ILegalPartyDomain
  {
    private readonly ILegalPartyRepository _legalPartyRepository;

    public LegalPartyDomain( ILegalPartyRepository legalPartyRepository )
    {
      _legalPartyRepository = legalPartyRepository;
    }

    public IEnumerable<LegalPartyRoleDto> GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate )
    {
      if ( revenueObjectId < 1 )
        throw new BadRequestException( string.Format( "revenueObjectId {0} is invalid.", revenueObjectId ) );

      var list =
        _legalPartyRepository.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( revenueObjectId,
                                                                                   effectiveDate )
                             .Select( x => x.ToDomain() )
                             .ToList();

      if ( list.Count == 0 )
      {
        throw new RecordNotFoundException( "", typeof( Repository.Models.V1.LegalPartyRole ),
                                           $"No legal party roles found for revenue object id {revenueObjectId}." );
      }

      return list;
    }

    public IEnumerable<LegalPartyRoleDto> GetLegalPartyRolesById( int[] legalPartyRoleIdList )
    {
      if ( legalPartyRoleIdList.ToList().Any( id => id < 1 ) )
        throw new BadRequestException( $"legalPartyRoleIdList {string.Join( ",", legalPartyRoleIdList )} are invalid." );

      var list =
        _legalPartyRepository.GetLegalPartyRolesById( legalPartyRoleIdList )
                             .Select( x => x.ToDomain() )
                             .ToList();

      if ( list.Count == 0 )
      {
        throw new RecordNotFoundException( "", typeof( Repository.Models.V1.LegalParty ),
                                           $"The legalPartyRoleIdList {string.Join( ",", legalPartyRoleIdList )} does not contain any valid Ids." );
      }

      return list;
    }
  }
}
