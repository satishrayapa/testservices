using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Implementation;
using Xunit;

namespace Repository.Tests
{
  public class LegalPartyOfficialDocumentRepositoryTests
  {
    private readonly LegalPartyOfficialDocumentRepository _legalPartyDocumentRepository;

    public LegalPartyOfficialDocumentRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      var legalPartyContext = new LegalPartyContext( optionsBuilder );

      legalPartyContext.BuildLegalPartyDocument();

      _legalPartyDocumentRepository = new LegalPartyOfficialDocumentRepository( legalPartyContext );
    }

    [Fact]
    public void SingleLegalPartyWithOfficialDocument()
    {
      var date = new DateTime( 2016, 4, 3 );
      var result = _legalPartyDocumentRepository.ListAsync( new[] { 1 }, date ).Result.ToList();

      result.Count.ShouldBe( 1 );

      result.Single().LegalPartyId.ShouldBe( 1 );
      result.Single().LegalPartyRoleId.ShouldBe( 1 );
      result.Single().LegalPartyDisplayName.ShouldBe( "foo" );
      result.Single().RightTransferId.ShouldBe( 23 );
      Convert.ToInt32( result.Single().GrantorGrantee ).ShouldBe( 1 );
      result.Single().PercentageBeneficialInterest.ShouldBe( 40 );
      result.Single().DocumentDate.ShouldBe( date );
      result.Single().DocumentNumber.ShouldBe( "doc 433555" );
      result.Single().DocumentType.ShouldBe( 3 );
    }

    [Fact]
    public void TwoLegalPartyWithOfficialDocuments()
    {
      var date = new DateTime( 2016, 4, 3 );
      var result = _legalPartyDocumentRepository.ListAsync( new[] { 1, 2 }, new DateTime( 2016, 4, 3 ) ).Result.ToList();

      result.Count.ShouldBe( 2 );

      result.Single( x => x.LegalPartyId == 1 ).LegalPartyId.ShouldBe( 1 );
      result.Single( x => x.LegalPartyId == 1 ).LegalPartyRoleId.ShouldBe( 1 );
      result.Single( x => x.LegalPartyId == 1 ).LegalPartyDisplayName.ShouldBe( "foo" );
      result.Single( x => x.LegalPartyId == 1 ).RightTransferId.ShouldBe( 23 );
      Convert.ToInt32( result.Single( x => x.LegalPartyId == 1 ).GrantorGrantee ).ShouldBe( 1 );
      result.Single( x => x.LegalPartyId == 1 ).PercentageBeneficialInterest.ShouldBe( 40 );
      result.Single( x => x.LegalPartyId == 1 ).DocumentDate.ShouldBe( date );
      result.Single( x => x.LegalPartyId == 1 ).DocumentNumber.ShouldBe( "doc 433555" );
      result.Single( x => x.LegalPartyId == 1 ).DocumentType.ShouldBe( 3 );

      result.Single( x => x.LegalPartyId == 2 ).LegalPartyId.ShouldBe( 2 );
      result.Single( x => x.LegalPartyId == 2 ).LegalPartyRoleId.ShouldBe( 2 );
      result.Single( x => x.LegalPartyId == 2 ).LegalPartyDisplayName.ShouldBe( "bar" );
      result.Single( x => x.LegalPartyId == 2 ).PercentageBeneficialInterest.ShouldBe( 60 );

    }
  }
}
