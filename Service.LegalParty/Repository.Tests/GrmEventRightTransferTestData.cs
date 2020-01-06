using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Models;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Constants;

namespace Repository.Tests
{
  public static class GrmEventRightTransferTestData
  {
    public static void BuildGrmEventRightTransfer( this LegalPartyContext legalPartyContext )
    {
      legalPartyContext.GrmEventArtifacts.Add( new GrmEventArtifact
                                               {
                                                 Id = 1,
                                                 GrmEventId = 1,
                                                 ObjectId = 1,
                                                 ObjectType = SysType.RightTransferId
                                               } );

      legalPartyContext.GrmEvents.Add( new GrmEvent
                                       {
                                         Id = 1,
                                         EventType = SysType.TransferId
                                       } );

      legalPartyContext.GrmEventArtifacts.Add( new GrmEventArtifact
                                               {
                                                 Id = 2,
                                                 GrmEventId = 2,
                                                 ObjectId = 2,
                                                 ObjectType = SysType.RightTransferId
                                               } );

      legalPartyContext.GrmEvents.Add( new GrmEvent
                                       {
                                         Id = 2,
                                         EventType = SysType.TransferId
                                       } );

      // This record should not show up because the SysType.RightTransferId and 
      // SysType.TransferId don't match with our expected query.

      legalPartyContext.GrmEventArtifacts.Add( new GrmEventArtifact
                                               {
                                                 Id = 3,
                                                 GrmEventId = 3,
                                                 ObjectId = 3,
                                                 ObjectType = 64
                                               } );

      legalPartyContext.GrmEvents.Add( new GrmEvent
                                       {
                                         Id = 3,
                                         EventType = 88
                                       } );



      legalPartyContext.SaveChanges();
    }
  }
}
