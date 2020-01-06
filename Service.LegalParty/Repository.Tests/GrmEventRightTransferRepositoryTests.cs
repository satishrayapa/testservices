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
  public class GrmEventRightTransferRepositoryTests
  {
    private readonly GrmEventRightTransferRepository _grmEventRightTransferRepository;

    public GrmEventRightTransferRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      var legalPartyContext = new LegalPartyContext( optionsBuilder );

      legalPartyContext.BuildLegalPartyDocument();

      _grmEventRightTransferRepository = new GrmEventRightTransferRepository( legalPartyContext );

      legalPartyContext.BuildGrmEventRightTransfer();
    }

    [Fact]
    public void SingleGrmEventRightTransfer()
    {
      var result = _grmEventRightTransferRepository.ListAsync( new List<int> { 1 } ).Result.ToList();

      result.Count.ShouldBe( 1 );

      result.Single().RightTransferId.ShouldBe( 1 );
      result.Single().GrmEventId.ShouldBe( 1 );
    }

    [Fact]
    public void TwoGrmEventRightTransfers()
    {
      var result = _grmEventRightTransferRepository.ListAsync( new List<int> { 1, 2 } ).Result.ToList();

      result.Count.ShouldBe( 2 );

      result.Single( x => x.RightTransferId == 1 ).RightTransferId.ShouldBe( 1 );
      result.Single( x => x.RightTransferId == 1 ).GrmEventId.ShouldBe( 1 );

      result.Single( x => x.RightTransferId == 2 ).RightTransferId.ShouldBe( 2 );
      result.Single( x => x.RightTransferId == 2 ).GrmEventId.ShouldBe( 2 );
    }
  }
}
