using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Tests
{
  public class BaseValueSegmentFlagRepositoryTests
  {
    private readonly BaseValueSegmentFlagRepository _baseValueSegmentFlagRepository;

    public BaseValueSegmentFlagRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder<AumentumContext>();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      var context = new AumentumContext( optionsBuilder.Options );

      context.BuildBaseValueSegmentFlags();

      _baseValueSegmentFlagRepository = new BaseValueSegmentFlagRepository( context );
    }

    [Fact]
    public void NoBaseValueSegmentFlagFoundDueToMissingId()
    {
      var result = _baseValueSegmentFlagRepository.ListAsync( 12 ).Result.ToList();
      result.Count.ShouldBe( 0 );
    }

    [Fact]
    public void NoBaseValueSegmentFlagFoundDueToInactiveEffectiveStatus()
    {
      var result = _baseValueSegmentFlagRepository.ListAsync( 13 ).Result.ToList();
      result.Count.ShouldBe( 0 );
    }

    [Fact]
    public void SingleBaseValueSegmentFlagFound()
    {
      var result = _baseValueSegmentFlagRepository.ListAsync( 15 ).Result.ToList();
      result.Count.ShouldBe( 1 );

      var item = result.Single();
      item.Description.ShouldBe( "foobar" );
      item.RevenueObjectId.ShouldBe( 15 );
    }

    [Fact]
    public void TwoBaseValueSegmentFlagsFound()
    {
      var result = _baseValueSegmentFlagRepository.ListAsync( 18 ).Result.ToList();
      result.Count.ShouldBe( 2 );

      var item1 = result.Single( x => x.Id == 228 );
      item1.Description.ShouldBe( "foo" );
      item1.RevenueObjectId.ShouldBe( 18 );

      var item2 = result.Single( x => x.Id == 229 );
      item2.Description.ShouldBe( "bar" );
      item2.RevenueObjectId.ShouldBe( 18 );
    }
  }
}
