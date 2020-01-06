using System;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;

namespace Repository.Tests
{
  public static class OfficialDocumentShortDescriptionTestData
  {
    public static void BuildOfficialDocumentShortDescription( this LegalPartyContext legalPartyContext )
    {
      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = 455,
                                          BegEffDate = new DateTime( 2016, 4, 1 ),
                                          ShortDescr = "foo a   "
                                        } );

      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = 455,
                                          BegEffDate = new DateTime( 2017, 4, 1 ),
                                          ShortDescr = "foo d   "
                                        } );

      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = 46,
                                          BegEffDate = new DateTime( 2015, 4, 1 ),
                                          ShortDescr = "fooa b   "
                                        } );

      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = 8132,
                                          BegEffDate = new DateTime( 2016, 2, 1 ),
                                          ShortDescr = "barr c   "
                                        } );

      legalPartyContext.SaveChanges();
    }
  }
}
