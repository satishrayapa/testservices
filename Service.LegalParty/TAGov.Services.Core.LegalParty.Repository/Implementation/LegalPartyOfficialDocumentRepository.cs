using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace TAGov.Services.Core.LegalParty.Repository.Implementation
{
  public class LegalPartyOfficialDocumentRepository : ILegalPartyOfficialDocumentRepository
  {
    private readonly LegalPartyContext _legalPartyContext;

    public LegalPartyOfficialDocumentRepository( LegalPartyContext legalPartyContext )
    {
      _legalPartyContext = legalPartyContext;
    }

    public async Task<IEnumerable<LegalPartyOfficalDocument>> ListAsync( IEnumerable<int> legalPartyRoleIds, DateTime effectiveDate )
    {
      var list = legalPartyRoleIds.ToList();

      return await ( from legalPartyRole in _legalPartyContext.LegalPartyRole

                     // inner join against legal party
                     join legalParty in _legalPartyContext.LegalParty on
                       legalPartyRole.LegalPartyId equals legalParty.Id

                     // Left JOIN RightHistory
                    join rightHistory in _legalPartyContext.RightHistories.Where(x => x.GrantorGrantee == 1)
                       on legalPartyRole.Id equals rightHistory.LegalPartyRoleId into rightHistoryJoinInfo
                     from rightHistoryDefaultIfEmpty in rightHistoryJoinInfo.DefaultIfEmpty()

                     // Left JOIN RightTransfer
                     join rightTransfer in _legalPartyContext.RightTransfers
                       on rightHistoryDefaultIfEmpty.RightTransferId equals rightTransfer.Id into rightTransferJoinInfo
                     from rightTransferDefaultIfEmpty in rightTransferJoinInfo.DefaultIfEmpty()

                     // Left JOIN OfficialDoc
                     join officialDocument in _legalPartyContext.OfficialDocuments
                       on rightTransferDefaultIfEmpty.OfficialDocumentId equals officialDocument.Id into officialDocumentsJoinInfo
                     from officialDocumentIfEmpty in officialDocumentsJoinInfo.DefaultIfEmpty()

                     // where clause with original table
                     where list.Contains( legalPartyRole.Id )

                           // where clauses against left joins
                           && ( rightHistoryDefaultIfEmpty == null || legalPartyRole.BegEffDate == rightHistoryDefaultIfEmpty.LegalPartyRoleBeginEffectiveDate )

                           && ( rightHistoryDefaultIfEmpty == null || rightHistoryDefaultIfEmpty.BeginEffectiveDate ==
                                ( from maxEffectiveDateRightHistory in _legalPartyContext.RightHistories
                                  where maxEffectiveDateRightHistory.EffectiveStatus == "A" &&
                                        maxEffectiveDateRightHistory.Id == rightHistoryDefaultIfEmpty.Id
                                  select maxEffectiveDateRightHistory ).Max( new Func<RightHistory, DateTime?>( rightHistory => rightHistory.BeginEffectiveDate ) ) )

                           && ( rightTransferDefaultIfEmpty == null || rightTransferDefaultIfEmpty.BeginEffectiveDate ==
                                ( from maxEffectiveDateRightTransfer in _legalPartyContext.RightTransfers
                                  where maxEffectiveDateRightTransfer.EffectiveStatus == "A" &&
                                        maxEffectiveDateRightTransfer.Id == rightTransferDefaultIfEmpty.Id
                                  select maxEffectiveDateRightTransfer ).Max( new Func<RightTransfer, DateTime?>( rightTransfer => rightTransfer.BeginEffectiveDate ) ) )

                           && ( officialDocumentIfEmpty == null || officialDocumentIfEmpty.BeginEffectiveDate ==
                                ( from maxEffectiveDateOfficialDocument in _legalPartyContext.OfficialDocuments
                                  where maxEffectiveDateOfficialDocument.EffectiveStatus == "A" &&
                                        maxEffectiveDateOfficialDocument.Id == officialDocumentIfEmpty.Id
                                  select maxEffectiveDateOfficialDocument ).Max( new Func<OfficialDocument, DateTime?>( officialDocument => officialDocument.BeginEffectiveDate ) ) )

                           && ( officialDocumentIfEmpty == null || officialDocumentIfEmpty.DocumentDate.Date <= effectiveDate.Date )

                     // build return set
                     select new LegalPartyOfficalDocument
                            {
                              // required
                              LegalPartyId = legalParty.Id,
                              LegalPartyRoleId = legalPartyRole.Id,
                              LegalPartyDisplayName = legalParty.DisplayName,
                              PercentageBeneficialInterest = legalPartyRole.PercentBeneficialInterest,

                              // left joins maybe missing
                              RightTransferId = officialDocumentIfEmpty != null ? rightHistoryDefaultIfEmpty.RightTransferId : ( int? ) null,
                              GrantorGrantee = ( short ) ( officialDocumentIfEmpty != null ? rightHistoryDefaultIfEmpty.GrantorGrantee : 1 ),
                              DocumentDate = officialDocumentIfEmpty != null ? officialDocumentIfEmpty.DocumentDate : ( DateTime? ) null,
                              DocumentNumber = officialDocumentIfEmpty != null ? officialDocumentIfEmpty.DocumentNumber : null,
                              DocumentType = officialDocumentIfEmpty != null ? officialDocumentIfEmpty.DocumentType : ( int? ) null

                            } ).ToListAsync();
    }
  }
}
