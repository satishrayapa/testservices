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

namespace Domain.Tests
{
  public class LegalPartyOfficialDocumentDomainTests
  {
    private readonly Mock<ILegalPartyOfficialDocumentRepository> _legalPartyOfficialDocumentRepository;
    private readonly Mock<IGrmEventRightTransferRepository> _grmEventRightTransferRepository;
    private readonly Mock<IOfficialDocumentShortDescriptionRepository> _officialDocumentShortDescriptionRepository;
    private readonly LegalPartyOfficialDocumentDomain _legalPartyOfficialDocumentDomain;

    public LegalPartyOfficialDocumentDomainTests()
    {
      _legalPartyOfficialDocumentRepository = new Mock<ILegalPartyOfficialDocumentRepository>();
      _grmEventRightTransferRepository = new Mock<IGrmEventRightTransferRepository>();
      _officialDocumentShortDescriptionRepository = new Mock<IOfficialDocumentShortDescriptionRepository>();

      _legalPartyOfficialDocumentDomain = new LegalPartyOfficialDocumentDomain(
        _legalPartyOfficialDocumentRepository.Object,
        _grmEventRightTransferRepository.Object,
        _officialDocumentShortDescriptionRepository.Object );
    }

    [Fact]
    public void NoLegalPartyRoleIdInList_BadRequestException()
    {
      Should.ThrowAsync<BadRequestException>( async () => { await _legalPartyOfficialDocumentDomain.ListAsync( new List<int>(), DateTime.Now ); } );
    }

    [Fact]
    public void NegativeLegalPartyRoleIdInList_BadRequestException()
    {
      Should.ThrowAsync<BadRequestException>( async () => { await _legalPartyOfficialDocumentDomain.ListAsync( new List<int> { -5 }, DateTime.Now ); } );
    }

    [Fact]
    public void NoOfficialDocumentsReturned_RecordsNotFoundException()
    {
      var date = new DateTime( 2017, 1, 1 );
      _legalPartyOfficialDocumentRepository.Setup( x => x.ListAsync( It.IsAny<List<int>>(), date ) ).ReturnsAsync( new List<LegalPartyOfficalDocument>() );

      Should.ThrowAsync<RecordNotFoundException>( async () => { await _legalPartyOfficialDocumentDomain.ListAsync( new List<int> { 5, 7 }, date ); } );
    }

    [Fact]
    public void WithOfficialDocumentsAndRightTransfersAndShortDescriptionsFoundAndNoGrantors_GetMappedLegalPartyDocumentDtos()
    {
      var date = new DateTime( 2017, 1, 1 );

      _legalPartyOfficialDocumentRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 12 ) && y.Contains( 15 ) ), date ) ).ReturnsAsync(
        new List<LegalPartyOfficalDocument>
        {
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 12,
            GrantorGrantee = 1,
            RightTransferId = 101,
            DocumentType = 324,
            DocumentDate = new DateTime( 2011, 5, 4 ),
            DocumentNumber = "doc foo",
            LegalPartyDisplayName = "foo one",
            PercentageBeneficialInterest = 60
          },
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 15,
            GrantorGrantee = 1,
            RightTransferId = 201,
            DocumentType = 441,
            DocumentDate = new DateTime( 2012, 3, 4 ),
            DocumentNumber = "doc bar",
            LegalPartyDisplayName = "bar one",
            PercentageBeneficialInterest = 40
          }
        } );

      _grmEventRightTransferRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 101 ) && y.Contains( 201 ) ) ) )
                                      .ReturnsAsync( new List<GrmEventRightTransfer>
                                                     {
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 10123,
                                                         RightTransferId = 101
                                                       },
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 201123,
                                                         RightTransferId = 201
                                                       }
                                                     } );

      _officialDocumentShortDescriptionRepository
        .Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 324 ) && y.Contains( 441 ) ) ) )
        .ReturnsAsync( new List<OfficialDocumentShortDescription>
                       {
                         new OfficialDocumentShortDescription
                         {
                           DocumentTypeId = 324,
                           ShortDescription = "foo"
                         },
                         new OfficialDocumentShortDescription
                         {
                           DocumentTypeId = 441,
                           ShortDescription = "bar"
                         }
                       } );

      var result = _legalPartyOfficialDocumentDomain.ListAsync( new List<int> { 12, 15 }, date ).Result.ToList();

      result.Count.ShouldBe( 2 );

      var foo = result.Single( x => x.LegalPartyRoleId == 12 );
      foo.DocDate.ShouldBe( new DateTime( 2011, 5, 4 ) );
      foo.DocNumber.ShouldBe( "doc foo" );
      foo.DocType.ShouldBe( "foo" );
      foo.GrmEventId.ShouldBe( 10123 );
      foo.LegalPartyDisplayName.ShouldBe( "foo one" );
      foo.LegalPartyRoleId.ShouldBe( 12 );
      foo.RightTransferId.ShouldBe( 101 );
      foo.PctGain.ShouldBe( 60 );

      var bar = result.Single( x => x.LegalPartyRoleId == 15 );
      bar.DocDate.ShouldBe( new DateTime( 2012, 3, 4 ) );
      bar.DocNumber.ShouldBe( "doc bar" );
      bar.DocType.ShouldBe( "bar" );
      bar.GrmEventId.ShouldBe( 201123 );
      bar.LegalPartyDisplayName.ShouldBe( "bar one" );
      bar.LegalPartyRoleId.ShouldBe( 15 );
      bar.RightTransferId.ShouldBe( 201 );
      bar.PctGain.ShouldBe( 40 );
    }

    [Fact]
    public void WithOfficialDocumentsWithNoDocumentType_GetMappedLegalPartyDocumentDtosWithShortDescriptionAsNoDocument()
    {
      var date = new DateTime( 2017, 1, 1 );

      _legalPartyOfficialDocumentRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 12 ) && y.Contains( 15 ) ), date ) ).ReturnsAsync(
        new List<LegalPartyOfficalDocument>
        {
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 12,
            GrantorGrantee = 1,
            RightTransferId = 101,

            DocumentDate = new DateTime( 2011, 5, 4 ),
            DocumentNumber = "doc foo",
            LegalPartyDisplayName = "foo one",
            PercentageBeneficialInterest = 60
          },
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 15,
            GrantorGrantee = 1,
            RightTransferId = 201,

            DocumentDate = new DateTime( 2012, 3, 4 ),
            DocumentNumber = "doc bar",
            LegalPartyDisplayName = "bar one",
            PercentageBeneficialInterest = 40
          }
        } );

      _grmEventRightTransferRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 101 ) && y.Contains( 201 ) ) ) )
                                      .ReturnsAsync( new List<GrmEventRightTransfer>
                                                     {
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 10123,
                                                         RightTransferId = 101
                                                       },
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 201123,
                                                         RightTransferId = 201
                                                       }
                                                     } );


      var result = _legalPartyOfficialDocumentDomain.ListAsync( new List<int> { 12, 15 }, date ).Result.ToList();

      result.Count.ShouldBe( 2 );

      var foo = result.Single( x => x.LegalPartyRoleId == 12 );
      foo.DocDate.ShouldBe( new DateTime( 2011, 5, 4 ) );
      foo.DocNumber.ShouldBe( "doc foo" );
      foo.DocType.ShouldBe( "No Document" );
      foo.GrmEventId.ShouldBe( 10123 );
      foo.LegalPartyDisplayName.ShouldBe( "foo one" );
      foo.LegalPartyRoleId.ShouldBe( 12 );
      foo.RightTransferId.ShouldBe( 101 );
      foo.PctGain.ShouldBe( 60 );

      var bar = result.Single( x => x.LegalPartyRoleId == 15 );
      bar.DocDate.ShouldBe( new DateTime( 2012, 3, 4 ) );
      bar.DocNumber.ShouldBe( "doc bar" );
      bar.DocType.ShouldBe( "No Document" );
      bar.GrmEventId.ShouldBe( 201123 );
      bar.LegalPartyDisplayName.ShouldBe( "bar one" );
      bar.LegalPartyRoleId.ShouldBe( 15 );
      bar.RightTransferId.ShouldBe( 201 );
      bar.PctGain.ShouldBe( 40 );
    }

    [Fact]
    public void WithOfficialDocumentsAndRightTransfersAndShortDescriptionsFoundAndGrantors_GetMappedLegalPartyDocumentDtos()
    {
      var date = new DateTime( 2017, 1, 1 );

      _legalPartyOfficialDocumentRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 12 ) && y.Contains( 15 ) ), date ) ).ReturnsAsync(
        new List<LegalPartyOfficalDocument>
        {
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 12,
            GrantorGrantee = 1,
            RightTransferId = 101,
            DocumentDate = new DateTime( 2011, 5, 4 ),
            DocumentNumber = "doc foo",
            LegalPartyDisplayName = "foo one",
            PercentageBeneficialInterest = 60
          },
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 12,
            GrantorGrantee = 0,
            RightTransferId = 101,
            PercentageBeneficialInterest = 50
          },
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 15,
            GrantorGrantee = 1,
            RightTransferId = 201,
            DocumentDate = new DateTime( 2012, 3, 4 ),
            DocumentNumber = "doc bar",
            LegalPartyDisplayName = "bar one",
            PercentageBeneficialInterest = 40
          },
          new LegalPartyOfficalDocument
          {
            LegalPartyRoleId = 15,
            GrantorGrantee = 0,
            RightTransferId = 201,
            PercentageBeneficialInterest = 10
          }
        } );

      _grmEventRightTransferRepository.Setup( x => x.ListAsync( It.Is<List<int>>( y => y.Contains( 101 ) && y.Contains( 201 ) ) ) )
                                      .ReturnsAsync( new List<GrmEventRightTransfer>
                                                     {
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 10123,
                                                         RightTransferId = 101
                                                       },
                                                       new GrmEventRightTransfer
                                                       {
                                                         GrmEventId = 201123,
                                                         RightTransferId = 201
                                                       }
                                                     } );


      var result = _legalPartyOfficialDocumentDomain.ListAsync( new List<int> { 12, 15 }, date ).Result.ToList();

      result.Count.ShouldBe( 2 );

      var foo = result.Single( x => x.LegalPartyRoleId == 12 );
      foo.DocDate.ShouldBe( new DateTime( 2011, 5, 4 ) );
      foo.DocNumber.ShouldBe( "doc foo" );
      foo.DocType.ShouldBe( "No Document" );
      foo.GrmEventId.ShouldBe( 10123 );
      foo.LegalPartyDisplayName.ShouldBe( "foo one" );
      foo.LegalPartyRoleId.ShouldBe( 12 );
      foo.RightTransferId.ShouldBe( 101 );
      foo.PctGain.ShouldBe( 10 );

      var bar = result.Single( x => x.LegalPartyRoleId == 15 );
      bar.DocDate.ShouldBe( new DateTime( 2012, 3, 4 ) );
      bar.DocNumber.ShouldBe( "doc bar" );
      bar.DocType.ShouldBe( "No Document" );
      bar.GrmEventId.ShouldBe( 201123 );
      bar.LegalPartyDisplayName.ShouldBe( "bar one" );
      bar.LegalPartyRoleId.ShouldBe( 15 );
      bar.RightTransferId.ShouldBe( 201 );
      bar.PctGain.ShouldBe( 30 );
    }
  }
}
