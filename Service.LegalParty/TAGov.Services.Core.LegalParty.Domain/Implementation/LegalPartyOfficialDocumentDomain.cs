using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalParty.Domain.Interfaces;
using TAGov.Services.Core.LegalParty.Domain.Mapping;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Domain.Implementation
{
  public class LegalPartyOfficialDocumentDomain : ILegalPartyOfficialDocumentDomain
  {

    private readonly ILegalPartyOfficialDocumentRepository _legalPartyOfficialDocumentRepository;
    private readonly IGrmEventRightTransferRepository _grmEventRightTransferRepository;
    private readonly IOfficialDocumentShortDescriptionRepository _officialDocumentShortDescriptionRepository;

    public LegalPartyOfficialDocumentDomain(
      ILegalPartyOfficialDocumentRepository legalPartyOfficialDocumentRepository,
      IGrmEventRightTransferRepository grmEventRightTransferRepository,
      IOfficialDocumentShortDescriptionRepository officialDocumentShortDescriptionRepository )
    {
      _legalPartyOfficialDocumentRepository = legalPartyOfficialDocumentRepository;
      _grmEventRightTransferRepository = grmEventRightTransferRepository;
      _officialDocumentShortDescriptionRepository = officialDocumentShortDescriptionRepository;
    }

    public async Task<IEnumerable<LegalPartyDocumentDto>> ListAsync( IEnumerable<int> legalPartyRoleIdList, DateTime effectiveDate )
    {
      var list = legalPartyRoleIdList.ToList();
      if ( list.Count == 0 )
        throw new BadRequestException( "Please supply at least one legal party role Id in list." );

      if ( list.Any( id => id < 1 ) )
        throw new BadRequestException( $"legalPartyRoleIdList {string.Join( ",", list )} are invalid." );

      var legalPartyOfficalDocuments = ( await _legalPartyOfficialDocumentRepository.ListAsync( list, effectiveDate ) ).ToList();

      if ( legalPartyOfficalDocuments.Count == 0 )
      {
        throw new RecordNotFoundException( "", typeof( LegalPartyDocument ),
                                           $"The legalPartyRoleIdList {string.Join( ",", list )} does not contain any valid Ids." );
      }

      var rightTransferIdList = legalPartyOfficalDocuments.Where( x => x.GrantorGrantee == 1 && x.RightTransferId.HasValue ).Select( x => x.RightTransferId.Value ).Distinct().ToList();

      var grmEventRightTransfers = ( await _grmEventRightTransferRepository.ListAsync( rightTransferIdList ) ).ToList();

      var documentTypeIdList = legalPartyOfficalDocuments.Where( x => x.DocumentType.HasValue ).Select( x => x.DocumentType.Value ).ToList();
      var officialDocumentShortDescriptions = new List<OfficialDocumentShortDescription>();
      if ( documentTypeIdList.Count > 0 )
      {
        officialDocumentShortDescriptions = ( await _officialDocumentShortDescriptionRepository.ListAsync( documentTypeIdList ) ).ToList();
      }

      return legalPartyOfficalDocuments
             .Where( x => x.GrantorGrantee == 1 )
             .Select( x =>
                      {
                        var grm = grmEventRightTransfers.SingleOrDefault( y => y.RightTransferId == x.RightTransferId );
                        var grantor = legalPartyOfficalDocuments.SingleOrDefault( y =>
                                                                                    y.RightTransferId == x.RightTransferId &&
                                                                                    y.LegalPartyRoleId == x.LegalPartyRoleId &&
                                                                                    y.GrantorGrantee == 0 );

                        string shortDescription = "No Document";

                        if ( x.DocumentType.HasValue )
                        {
                          var found = officialDocumentShortDescriptions.SingleOrDefault( y => y.DocumentTypeId == x.DocumentType.Value );
                          if ( found != null )
                          {
                            shortDescription = found.ShortDescription;
                          }
                        }

                        return new LegalPartyDocument
                               {
                                 DocDate = x.DocumentDate,
                                 DocNumber = x.DocumentNumber,
                                 DocType = shortDescription,
                                 GrmEventId = grm?.GrmEventId,
                                 LegalPartyDisplayName = x.LegalPartyDisplayName,
                                 LegalPartyRoleId = x.LegalPartyRoleId,
                                 RightTransferId = x.RightTransferId,
                                 PctGain = x.PercentageBeneficialInterest - ( grantor?.PercentageBeneficialInterest ?? 0 )
                               };
                      } ).ToDomain();
    }
  }
}
