using System;
using System.Collections.Generic;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;
using Xunit;

namespace Domain.Tests
{
  public class MockBvsDto : BvsDto
  {
  }

  public class ExtensionsTests
  {
    [Fact]
    public void WhenPopulateEventIsNullUseUnknowns()
    {
      var dto = new MockBvsDto();
      var list = new List<GrmEventInformationDto>();

      list.PopulateEvent( dto );

      dto.EventDate.ShouldBeNull();
      dto.EventName.ShouldBe( Constants.EventUnknownName );
      dto.EventType.ShouldBe( Constants.EventUnknownName );
    }

    [Fact]
    public void WhenPopulateEventIsNotNullUseFirstFromList()
    {
      var dto = new MockBvsDto();
      var date = new DateTime( 2017, 2, 1 );
      var list = new List<GrmEventInformationDto>
                 {
                   new GrmEventInformationDto { Description = "foo1", EventType = "bar1", EventDate = date },
                   new GrmEventInformationDto { Description = "foo2", EventType = "bar2", EventDate = date.AddDays( 1 ) }
                 };

      list.PopulateEvent( dto );

      dto.EventDate.ShouldNotBeNull();
      dto.EventDate.ShouldBe( date );
      dto.EventName.ShouldBe( "foo1" );
      dto.EventType.ShouldBe( "bar1" );
    }

    [Fact]
    public void ShouldGetFirstTransactionOrderedByIdFromBaseValueSegmentWhereIdIsAtLast()
    {
      var dto = new BaseValueSegmentDto
                {
                  BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                 {
                                                   new BaseValueSegmentTransactionDto { Id = 101 },
                                                   new BaseValueSegmentTransactionDto { Id = 102 },
                                                   new BaseValueSegmentTransactionDto { Id = 300 }
                                                 }
                };
      var firstTransaction = dto.FirstTransaction();
      firstTransaction.Id.ShouldBe( 300 );
    }

    [Fact]
    public void ShouldGetFirstTransactionOrderedByIdFromBaseValueSegmentWhereIdIsAtFirst()
    {
      var dto = new BaseValueSegmentDto
                {
                  BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                 {
                                                   new BaseValueSegmentTransactionDto { Id = 401 },
                                                   new BaseValueSegmentTransactionDto { Id = 101 },
                                                   new BaseValueSegmentTransactionDto { Id = 102 },
                                                   new BaseValueSegmentTransactionDto { Id = 300 }
                                                 }
                };
      var firstTransaction = dto.FirstTransaction();
      firstTransaction.Id.ShouldBe( 401 );
    }

    [Fact]
    public void ShouldGetFirstTransactionOrderedByIdFromBaseValueSegmentWhereIdIsAtMid()
    {
      var dto = new BaseValueSegmentDto
                {
                  BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                 {
                                                   new BaseValueSegmentTransactionDto { Id = 401 },
                                                   new BaseValueSegmentTransactionDto { Id = 521 },
                                                   new BaseValueSegmentTransactionDto { Id = 102 },
                                                   new BaseValueSegmentTransactionDto { Id = 300 }
                                                 }
                };
      var firstTransaction = dto.FirstTransaction();
      firstTransaction.Id.ShouldBe( 521 );
    }

    [Fact]
    public void WhenPopulateOwnerDtoEventIsNotNullUseFirstMatchFromList()
    {
      var date = new DateTime( 2013, 2, 11 );
      var list = new List<GrmEventInformationDto>
                 {
                   new GrmEventInformationDto { GrmEventId = 5, Description = "foo1", EventType = "bar1", EffectiveDate = date },
                   new GrmEventInformationDto { GrmEventId = 8, Description = "foo2", EventType = "bar2", EffectiveDate = date.AddDays( 1 ) }
                 };

      var ownerDto = new OwnerDto();
      list.PopulateEvent( ownerDto, 8 );

      ownerDto.EventDate.ShouldNotBeNull();
      ownerDto.EventDate.ShouldBe( date.AddDays( 1 ) );
      ownerDto.EventName.ShouldBe( "foo2" );
      ownerDto.EventType.ShouldBe( "bar2" );
    }

    [Fact]
    public void WhenPopulateOwnerDtoEventIsNullUseUnknown()
    {
      var date = new DateTime( 2013, 2, 11 );
      var list = new List<GrmEventInformationDto>
                 {
                   new GrmEventInformationDto { GrmEventId = 5, Description = "foo1", EventType = "bar1", EffectiveDate = date },
                   new GrmEventInformationDto { GrmEventId = 8, Description = "foo2", EventType = "bar2", EffectiveDate = date.AddDays( 1 ) }
                 };

      var ownerDto = new OwnerDto();
      list.PopulateEvent( ownerDto, 18 );

      ownerDto.EventDate.ShouldBeNull();
      ownerDto.EventName.ShouldBe( Constants.EventUnknownName );
      ownerDto.EventType.ShouldBe( Constants.EventUnknownName );
    }

    [Fact]
    public void WhenPopulateOwnerValueDtoEventIsNotNullUseFirstMatchFromList()
    {
      var date = new DateTime( 2013, 2, 11 );
      var list = new List<GrmEventInformationDto>
                 {
                   new GrmEventInformationDto { GrmEventId = 5, Description = "foo1", EventType = "bar1", EffectiveDate = date },
                   new GrmEventInformationDto { GrmEventId = 8, Description = "foo2", EventType = "bar2", EffectiveDate = date.AddDays( 1 ) }
                 };

      var ownerDto = new OwnerValueDto();
      list.PopulateEvent( ownerDto, 8 );

      ownerDto.EventDate.ShouldNotBeNull();
      ownerDto.EventDate.ShouldBe( date.AddDays( 1 ) );
      ownerDto.EventName.ShouldBe( "foo2" );
      ownerDto.EventType.ShouldBe( "bar2" );
    }

    [Fact]
    public void WhenPopulateOwnerValueDtoEventIsNullUseUnknown()
    {
      var date = new DateTime( 2013, 2, 11 );
      var list = new List<GrmEventInformationDto>
                 {
                   new GrmEventInformationDto { GrmEventId = 5, Description = "foo1", EventType = "bar1", EffectiveDate = date },
                   new GrmEventInformationDto { GrmEventId = 8, Description = "foo2", EventType = "bar2", EffectiveDate = date.AddDays( 1 ) }
                 };

      var ownerDto = new OwnerValueDto();
      list.PopulateEvent( ownerDto, 18 );

      ownerDto.EventDate.ShouldBeNull();
      ownerDto.EventName.ShouldBe( Constants.EventUnknownName );
      ownerDto.EventType.ShouldBe( Constants.EventUnknownName );
    }

    [Fact]
    public void PopulateOwnerFromDocument()
    {
      var list = new List<LegalPartyDocumentDto>
                 {
                   new LegalPartyDocumentDto
                   {
                     LegalPartyRoleId = 18,
                     GrmEventId = 1,
                     DocNumber = "foodocnum1",
                     LegalPartyDisplayName = "fooname1",
                     DocType = "footype1",
                     PctGain = 40
                   },
                   new LegalPartyDocumentDto
                   {
                     LegalPartyRoleId = 11,
                     GrmEventId = 1,
                     DocNumber = "foodocnum2",
                     LegalPartyDisplayName = "fooname2",
                     DocType = "footype2",
                     PctGain = 10
                   },
                   new LegalPartyDocumentDto
                   {
                     LegalPartyRoleId = 12,
                     GrmEventId = 1,
                     DocNumber = "foodocnum3",
                     LegalPartyDisplayName = "fooname3",
                     DocType = "footype3",
                     PctGain = 60
                   }
                 };

      var bvsOwner = new BaseValueSegmentOwnerDto()
                     {
                       LegalPartyRoleId = 12,
                       GRMEventId = 1
                     };

      var owner = new OwnerDto();
      list.PopulateOwner( owner, bvsOwner );
      owner.BeneficialInterest.ShouldBe( "fooname3" );
      owner.DocNumber.ShouldBe( "foodocnum3" );
      owner.DocType.ShouldBe( "footype3" );
      owner.PercentageInterestGain.ShouldBe( 60 );
    }

    [Fact]
    public void PopulateToOwner()
    {
      var owner = new BaseValueSegmentOwnerDto
                  {
                    Id = 2,
                    BeneficialInterestPercent = 64
                  };

      var dto = owner.ToOwner();
      dto.OwnerId.ShouldBe( 2 );
      dto.BiPercentage.ShouldBe( 64 );
    }
  }
}