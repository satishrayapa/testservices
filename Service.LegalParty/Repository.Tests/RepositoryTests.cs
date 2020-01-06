using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Implementation;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using Xunit;

namespace Repository.Tests
{

  public class RepositoryTests
  {
    private readonly LegalPartyContext _legalPartyContext;

    public RepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      _legalPartyContext = new LegalPartyContext( optionsBuilder );

      TestDataBuilder.Build( _legalPartyContext );
    }

    /**********************GetByEffectiveDate********************************/

    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdMatch_EffectiveDateExistsLess_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( TestDataBuilder.LegalPartyRoleId1, new DateTime( 2010, 01, 01 ) );

      legalParties.ShouldNotBeNull();
      legalParties.Id.ShouldBe( TestDataBuilder.LegalPartyId1 );
    }

    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdMatch_EffectiveDateDoesNotExistsLess_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( TestDataBuilder.LegalPartyRoleId1, new DateTime( 2009, 01, 01 ) );

      legalParties.ShouldBeNull();
    }


    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdMatch_EffectiveDateMaxValue_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( TestDataBuilder.LegalPartyRoleId1, new DateTime( 9999, 12, 31 ) );

      legalParties.ShouldNotBeNull();
      legalParties.Id.ShouldBe( TestDataBuilder.LegalPartyId1 );
    }

    /******************************************************/

    /*****************************no match id************************************/

    /******************************************************/

    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdDoesNotMatch_EffectiveDateExistsLess_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( 0, new DateTime( 2010, 01, 01 ) );

      legalParties.ShouldBeNull();
    }

    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdDoesNotMatch_EffectiveDateDoesNotExistsLess_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( 0, new DateTime( 2009, 01, 01 ) );

      legalParties.ShouldBeNull();
    }


    [Fact]
    public void GetByEffectiveDate_LegalPartyRoleIdDoesNotMatch_EffectiveDateMaxValue_EffectiveStatusNull()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalParties = legalPartyRepository.GetByEffectiveDate( 0, new DateTime( 9999, 12, 31 ) );

      legalParties.ShouldBeNull();
    }

    /**********************Get Legal Party Rolese********************************/
    [Fact]
    public void GetLegalPartyRolesById_MatchesId()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalPartyRoles = legalPartyRepository.GetLegalPartyRolesById( new[] { TestDataBuilder.LegalPartyRoleId1 } ).ToList();

      legalPartyRoles.Count.ShouldBeGreaterThan( 0 );
      legalPartyRoles[ 0 ].Id.ShouldBe( TestDataBuilder.LegalPartyRoleId1 );
    }

    [Fact]
    public void GetLegalPartyRolesById_DoesNotMatchId()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalPartyRoles = legalPartyRepository.GetLegalPartyRolesById( new[] { 100 } ).ToList();

      legalPartyRoles.Count.ShouldBe( 0 );
    }

    [Fact( Skip = "Enable when we change to use linq" )]
    public void GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate_EffectiveDateExistsLess_LegalPartyRoleExistsSecondHalfOfConcat_EffectiveStatusA()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalPartyRoles =
        legalPartyRepository.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( TestDataBuilder.RevenueObjectId1,
                                                                                  TestDataBuilder.NewestEffectiveDate );
      legalPartyRoles.ShouldContain( lpr => ( lpr.Id == TestDataBuilder.LegalPartyRoleId3 ) && ( lpr.LegalParty != null ) );
    }

    // Enable when we change to use linq
    public void GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate_EffectiveDateExistsLess_LegalPartyRoleExistsFirstHalfOfConcat_EffectiveStatusA()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalPartyRoles =
        legalPartyRepository.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( TestDataBuilder.RevenueObjectId2,
                                                                                  TestDataBuilder.NewestEffectiveDate );
      legalPartyRoles.ShouldContain( lpr => ( lpr.Id == TestDataBuilder.LegalPartyRoleId4 ) && ( lpr.LegalParty != null ) );
    }

    [Fact( Skip = "Enable when we change to use linq" )]
    public void GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate_TwoLegalPartiesTheSameDate()
    {
      ILegalPartyRepository legalPartyRepository = new LegalPartyRepository( _legalPartyContext );
      var legalPartyRoles =
        legalPartyRepository.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( TestDataBuilder.RevenueObjectId3,
                                                                                  TestDataBuilder.OldestEffectiveDate );
      legalPartyRoles.ShouldHaveSingleItem();
      legalPartyRoles.ShouldContain( lpr => ( lpr.Id == TestDataBuilder.LegalPartyRoleId7 ) && ( lpr.LegalParty != null ) );
    }
  }
}
