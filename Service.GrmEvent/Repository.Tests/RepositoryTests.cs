using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TAGov.Services.Core.GrmEvent.Repository.Implementation;
using TAGov.Services.Core.GrmEvent.Repository.Interfaces;
using Xunit;

namespace TAGov.Services.Core.GrmEvent.Repository.Tests
{

  public class RepositoryTests
  {
    private GrmEventContext _grmEventContext;

    public RepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );
      optionsBuilder.ConfigureWarnings( w => w.Ignore( InMemoryEventId.TransactionIgnoredWarning ) );


      _grmEventContext = new GrmEventContext( optionsBuilder );

      TestDataBuilder.Build( _grmEventContext );
    }

    /**************** grm event get *******************/

    [Fact]
    public void GetGrmEvent_InsertsAndRetrievesTheRecord_MatchesId()
    {
      IGrmEventRepository grmEventRepository = new GrmEventRepository( _grmEventContext );
      var grmEvent = grmEventRepository.Get( TestDataBuilder.GrmEventId );

      grmEvent.ShouldNotBeNull();
      grmEvent.Id.ShouldBe( TestDataBuilder.GrmEventId );
    }

    [Fact]
    public void GetGrmEvent_InsertsAndRetrievesTheRecord_DoesNotMatchId()
    {
      IGrmEventRepository grmEventRepository = new GrmEventRepository( _grmEventContext );
      var grmEvent = grmEventRepository.Get( 0 );

      grmEvent.ShouldBeNull();
    }

    [Fact]
    public void GetGrmEvenByRevenueObjectIdAndEffectiveDateAndEventType__MatchFound()
    {
      IGrmEventRepository grmEventRepository = new GrmEventRepository( _grmEventContext );
      var grmEvent = grmEventRepository.Get( TestDataBuilder.RevObjId,
                                             new DateTime( 2016, 1, 1 ), TestDataBuilder.SystemTypeId );

      grmEvent.ShouldNotBeNull();
      grmEvent.Id.ShouldBe( TestDataBuilder.GrmEventId + 3 );
    }

    [Fact]
    public void GetGrmEvenByRevenueObjectIdAndEffectiveDateAndEventType__MatchNotFound()
    {
      IGrmEventRepository grmEventRepository = new GrmEventRepository( _grmEventContext );
      var grmEvent = grmEventRepository.Get( TestDataBuilder.GrmEventId + 1000,
                                             new DateTime( 2017, 1, 1 ), TestDataBuilder.SystemTypeId + 1 );

      grmEvent.ShouldBeNull();
    }
  }
}