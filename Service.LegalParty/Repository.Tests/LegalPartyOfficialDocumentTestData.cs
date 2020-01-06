using System;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace Repository.Tests
{
  public static class LegalPartyOfficialDocumentTestData
  {
    public static void BuildLegalPartyDocument( this LegalPartyContext legalPartyContext )
    {
      var date = new DateTime( 2016, 4, 3 );

      // Legal Party 1.

      legalPartyContext.LegalParty.Add( new LegalParty
                                        {
                                          Id = 1,
                                          DisplayName = "foo"
                                        } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = 1,
                                              LegalPartyId = 1,
                                              BegEffDate = date,
                                              PercentBeneficialInterest = 40
                                            } );

      legalPartyContext.RightHistories.Add( new RightHistory
                                            {
                                              Id = 1,
                                              LegalPartyRoleId = 1,
                                              BeginEffectiveDate = date,
                                              LegalPartyRoleBeginEffectiveDate = date,
                                              RightTransferId = 23,
                                              GrantorGrantee = 1,
                                              EffectiveStatus = "A"
                                            } );

      // Should not be picked up because it is "inactive"
      legalPartyContext.RightHistories.Add( new RightHistory
                                            {
                                              Id = 1,
                                              LegalPartyRoleId = 1,
                                              BeginEffectiveDate = new DateTime( 2016, 4, 4 ),
                                              LegalPartyRoleBeginEffectiveDate = new DateTime( 2016, 4, 4 ),
                                              RightTransferId = 24,
                                              GrantorGrantee = 1,
                                              EffectiveStatus = "I"
                                            } );

      // Should not be picked up because this is NOT the max.
      legalPartyContext.RightHistories.Add( new RightHistory
                                            {
                                              Id = 1,
                                              LegalPartyRoleId = 1,
                                              BeginEffectiveDate = new DateTime( 2016, 4, 1 ),
                                              LegalPartyRoleBeginEffectiveDate = date,
                                              RightTransferId = 28,
                                              GrantorGrantee = 1,
                                              EffectiveStatus = "A"
                                            } );

      legalPartyContext.RightTransfers.Add( new RightTransfer
                                            {
                                              Id = 23,
                                              OfficialDocumentId = 33,
                                              BeginEffectiveDate = date,
                                              EffectiveStatus = "A"
                                            } );

      // Related to the "none max" right history record.
      legalPartyContext.RightTransfers.Add( new RightTransfer
                                            {
                                              Id = 24,
                                              OfficialDocumentId = 3190,
                                              BeginEffectiveDate = new DateTime( 2016, 4, 1 ),
                                              EffectiveStatus = "A"
                                            } );

      // Related to the "inactive" right history record.
      legalPartyContext.RightTransfers.Add( new RightTransfer
                                            {
                                              Id = 28,
                                              OfficialDocumentId = 313,
                                              BeginEffectiveDate = new DateTime( 2016, 4, 4 ),
                                              EffectiveStatus = "I"
                                            } );

      legalPartyContext.OfficialDocuments.Add( new OfficialDocument
                                               {
                                                 Id = 33,
                                                 BeginEffectiveDate = date,
                                                 EffectiveStatus = "A",
                                                 DocumentDate = date,
                                                 DocumentNumber = "doc 433555",
                                                 DocumentType = 3
                                               } );

      // Related to the "none max" Official Document record.
      legalPartyContext.OfficialDocuments.Add( new OfficialDocument
                                               {
                                                 Id = 3190,
                                                 BeginEffectiveDate = new DateTime( 2016, 4, 1 ),
                                                 EffectiveStatus = "A",
                                                 DocumentDate = new DateTime( 2016, 4, 1 ),
                                                 DocumentNumber = "doc dsfd 548935",
                                                 DocumentType = 3
                                               } );

      // Related to the "inactive" right history record.
      legalPartyContext.OfficialDocuments.Add( new OfficialDocument
                                               {
                                                 Id = 313,
                                                 BeginEffectiveDate = new DateTime( 2016, 4, 4 ),
                                                 EffectiveStatus = "I",
                                                 DocumentDate = date,
                                                 DocumentNumber = "doc SJFDD",
                                                 DocumentType = 3
                                               } );

      // Legal Party 2.
      legalPartyContext.LegalParty.Add( new LegalParty
                                        {
                                          Id = 2,
                                          DisplayName = "bar"
                                        } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = 2,
                                              LegalPartyId = 2,
                                              BegEffDate = date,
                                              PercentBeneficialInterest = 60
                                            } );

      legalPartyContext.RightHistories.Add( new RightHistory
                                            {
                                              Id = 2,
                                              LegalPartyRoleId = 2,
                                              BeginEffectiveDate = date,
                                              LegalPartyRoleBeginEffectiveDate = date,
                                              RightTransferId = 25,
                                              GrantorGrantee = 0,
                                              EffectiveStatus = "A"
                                            } );

      legalPartyContext.RightTransfers.Add( new RightTransfer
                                            {
                                              Id = 25,
                                              OfficialDocumentId = 4533,
                                              BeginEffectiveDate = date,
                                              EffectiveStatus = "A"
                                            } );

      legalPartyContext.OfficialDocuments.Add( new OfficialDocument
                                               {
                                                 Id = 4533,
                                                 BeginEffectiveDate = date,
                                                 EffectiveStatus = "A",
                                                 DocumentDate = date,
                                                 DocumentNumber = "doc 35567bxc",
                                                 DocumentType = 6
                                               } );

      legalPartyContext.SaveChanges();
    }
  }
}
