using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalParty.Domain.Implementation;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;
using Xunit;
using EffectiveStatuses = TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums.EffectiveStatuses;

namespace Domain.Tests
{
  public class LegalPartyDomainTests
  {
    /***************GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate*************/
    [Fact]
    public void GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate_EntitiesReturned_GetDtos()
    {
      const int revenueObjectId = 1;
      var effectiveDate = new DateTime( 2011, 1, 1 );

      var moqRepository = new Mock<ILegalPartyRepository>();
      moqRepository.Setup(
                     x => x.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate(
                       revenueObjectId, effectiveDate ) )
                   .Returns(
                     () => new List<LegalPartyRole>
                           {
                             new LegalPartyRole
                             {
                               Id = 12,
                               BegEffDate = new DateTime( 2014, 01, 01 ),
                               EffectiveStatus = EffectiveStatuses.Active,
                               TranId = 1,
                               LegalPartyId = 13,
                               ObjectType = 1,
                               ObjectId = revenueObjectId,
                               AcctId = 1,
                               LegalPartyRoleType = 1,
                               PrimeLegalParty = 1,
                               OwnershipType = 1,
                               PercentInt = 1,
                               Numerator = 1,
                               Denominator = 1,
                               GroupSequence = 1,
                               LegalPartyRoleSubtype = 1,
                               OriginalTransferor = 1,
                               Survivorship = 1,
                               LegalParty = new LegalParty
                                            {
                                              Id = 13,
                                              AliasType = 10,
                                              CombineToId = 11,
                                              CompressedName = "CN",
                                              Confidential = 3,
                                              DateOfBirth = new DateTime( 2010, 1, 1 ),
                                              DisplayName = "abc",
                                              FirstName = "joe",
                                              LastName = "mack",
                                              FunclRole = 3,
                                              LPSubType = 12,
                                              LegalPartyType = 13,
                                              MiddleName = "kate",
                                              NamePrefix = "mr",
                                              NameSfx = "mary",
                                              PrimeLPId = 14,
                                              ResidVerified = 15,
                                              SoundexCd = "as",
                                              TranId = 12333,
                                              DateOfDeath = new DateTime( 2011, 2, 1 )
                                            }
                             }
                           } );

      var legalPartyDomain = new LegalPartyDomain( moqRepository.Object );

      var list = legalPartyDomain.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( revenueObjectId, effectiveDate ).ToList();

      list.Count.ShouldBe( 1 );
      list[ 0 ].Id.ShouldBe( 12 );
      list[ 0 ].BegEffDate.ShouldBe( new DateTime( 2014, 01, 01 ) );
      //list[0].EffectiveStatus.ShouldBe<EffectiveStatuses>(EffectiveStatuses.Active);
      list[ 0 ].TranId.ShouldBe( 1 );
      list[ 0 ].LegalPartyId.ShouldBe( 13 );
      list[ 0 ].ObjectType.ShouldBe( 1 );
      list[ 0 ].ObjectId.ShouldBe( 1 );
      list[ 0 ].AcctId.ShouldBe( 1 );
      list[ 0 ].LegalPartyRoleType.ShouldBe( 1 );
      list[ 0 ].PrimeLegalParty.ShouldBe<short>( 1 );
      list[ 0 ].OwnershipType.ShouldBe( 1 );
      list[ 0 ].PercentInt.ShouldBe( 1 );
      list[ 0 ].Numerator.ShouldBe<short>( 1 );
      list[ 0 ].Denominator.ShouldBe<short>( 1 );
      list[ 0 ].GroupSequence.ShouldBe( 1 );
      list[ 0 ].LegalPartyRoleSubtype.ShouldBe( 1 );
      list[ 0 ].OriginalTransferor.ShouldBe( 1 );
      list[ 0 ].Survivorship.ShouldBe( 1 );

      list[ 0 ].LegalParty.ShouldNotBeNull();
      list[ 0 ].LegalParty.Id.ShouldBe( 13 );
      list[ 0 ].LegalParty.AliasType.ShouldBe( 10 );
      list[ 0 ].LegalParty.CombineToId.ShouldBe( 11 );
      list[ 0 ].LegalParty.CompressedName.ShouldBe( "CN" );
      list[ 0 ].LegalParty.Confidential.ShouldBe<short>( 3 );
      list[ 0 ].LegalParty.DateOfBirth.ShouldBe( new DateTime( 2010, 1, 1 ) );
      list[ 0 ].LegalParty.DisplayName.ShouldBe( "abc" );
      list[ 0 ].LegalParty.FirstName.ShouldBe( "joe" );
      list[ 0 ].LegalParty.LastName.ShouldBe( "mack" );
      list[ 0 ].LegalParty.FunclRole.ShouldBe( 3 );
      list[ 0 ].LegalParty.LPSubType.ShouldBe( 12 );
      list[ 0 ].LegalParty.LegalPartyType.ShouldBe( 13 );
      list[ 0 ].LegalParty.MiddleName.ShouldBe( "kate" );
      list[ 0 ].LegalParty.NamePrefix.ShouldBe( "mr" );
      list[ 0 ].LegalParty.NameSfx.ShouldBe( "mary" );
      list[ 0 ].LegalParty.PrimeLPId.ShouldBe( 14 );
      list[ 0 ].LegalParty.ResidVerified.ShouldBe<short>( 15 );
      list[ 0 ].LegalParty.SoundexCd.ShouldBe( "as" );
      list[ 0 ].LegalParty.TranId.ShouldBe( 12333 );
      list[ 0 ].LegalParty.DateOfDeath.ShouldBe( new DateTime( 2011, 2, 1 ) );
    }

    [Fact]
    public void GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate_RevenuObjectIdIs0_GetBadRequestException()
    {
      var legalPartyDomain = new LegalPartyDomain( null );
      Should.Throw<BadRequestException>(
        () => legalPartyDomain.GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate(
          0, new DateTime( 2014, 01, 01 ) ) );
    }

    [Fact]
    public void GetLegalPartyRolesById_ReturnsLegalPartyRoles()
    {
      var moqRepository = new Mock<ILegalPartyRepository>();
      moqRepository.Setup( x => x.GetLegalPartyRolesById( new[] { 12 } ) ).Returns( () => new List<LegalPartyRole>
                                                                                          {
                                                                                            new LegalPartyRole
                                                                                            {
                                                                                              Id = 12,
                                                                                              LegalParty = new LegalParty
                                                                                                           {
                                                                                                             Id = 13
                                                                                                           }
                                                                                            }
                                                                                          } );

      var legalPartyDomain = new LegalPartyDomain( moqRepository.Object );

      var list = legalPartyDomain.GetLegalPartyRolesById( new[] { 12 } ).ToList();

      list.Count.ShouldBe( 1 );
      list[ 0 ].Id.ShouldBe( 12 );

      list[ 0 ].LegalParty.ShouldNotBeNull();
      list[ 0 ].LegalParty.Id.ShouldBe( 13 );
    }

    [Fact]
    public void GetLegalPartyRolesById_GetBadRequestException()
    {
      var moqRepository = new Mock<ILegalPartyRepository>();
      moqRepository.Setup( x => x.GetLegalPartyRolesById( new[] { 12 } ) ).Returns( () => new List<LegalPartyRole>
                                                                                          {
                                                                                            new LegalPartyRole
                                                                                            {
                                                                                              Id = 12,
                                                                                              LegalParty = new LegalParty
                                                                                                           {
                                                                                                             Id = 13
                                                                                                           }
                                                                                            }
                                                                                          } );

      var legalPartyDomain = new LegalPartyDomain( moqRepository.Object );

      Should.Throw<BadRequestException>( () => legalPartyDomain.GetLegalPartyRolesById( new[] { 12, -1 } ) );

    }

    [Fact]
    public void GetLegalPartyRolesById_NoRecordsFound_GetNotFoundException()
    {
      var moqRepository = new Mock<ILegalPartyRepository>();
      moqRepository.Setup( x => x.GetLegalPartyRolesById( new[] { 12 } ) ).Returns( () => new List<LegalPartyRole>
                                                                                          {
                                                                                            new LegalPartyRole
                                                                                            {
                                                                                              Id = 12,
                                                                                              LegalParty = new LegalParty
                                                                                                           {
                                                                                                             Id = 13
                                                                                                           }
                                                                                            }
                                                                                          } );

      var legalPartyDomain = new LegalPartyDomain( moqRepository.Object );

      Should.Throw<RecordNotFoundException>( () => legalPartyDomain.GetLegalPartyRolesById( new[] { 42 } ) );
    }

  }
}
