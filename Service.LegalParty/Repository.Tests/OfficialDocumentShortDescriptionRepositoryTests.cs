using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Implementation;
using Xunit;

namespace Repository.Tests
{
  public class OfficialDocumentShortDescriptionRepositoryTests
  {
    private readonly OfficialDocumentShortDescriptionRepository _officialDocumentShortDescriptionRepository;

    public OfficialDocumentShortDescriptionRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      var legalPartyContext = new LegalPartyContext( optionsBuilder );

      _officialDocumentShortDescriptionRepository = new OfficialDocumentShortDescriptionRepository( legalPartyContext );

      legalPartyContext.BuildOfficialDocumentShortDescription();
    }

    [Fact]
    public void SingleOfficialDocumentShortDescription()
    {
      var result = _officialDocumentShortDescriptionRepository.ListAsync( new List<int> { 455 } ).Result.ToList();

      result.Count.ShouldBe( 1 );

      result.Single().DocumentTypeId.ShouldBe( 455 );
      result.Single().ShortDescription.ShouldBe( "foo d" );
    }

    [Fact]
    public void TwoOfficialDocumentShortDescriptions()
    {
      var result = _officialDocumentShortDescriptionRepository.ListAsync( new List<int> { 455, 46 } ).Result.ToList();

      result.Count.ShouldBe( 2 );

      result.Single( x => x.DocumentTypeId == 455 ).DocumentTypeId.ShouldBe( 455 );
      result.Single( x => x.DocumentTypeId == 455 ).ShortDescription.ShouldBe( "foo d" );

      result.Single( x => x.DocumentTypeId == 46 ).DocumentTypeId.ShouldBe( 46 );
      result.Single( x => x.DocumentTypeId == 46 ).ShortDescription.ShouldBe( "fooa b" );
    }
  }
}
