using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.AssessmentHeader.Domain.Implementation.V1;
using TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1;
using TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1;
using Xunit;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Tests
{
  public class AssessmentHeaderDomainTests
  {
    [Fact]
    public void Get_CallsGetOnAssessmentEventService_Receives_AssessmentEventIdIsInvalid_ThrowsException()
    {
      const int assessmentEventId = 0;

      var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( ( AssessmentEventDto ) null );

      var applicationSettingsHelper = new Mock<IApplicationSettingsHelper>();

      var domain = new AssessmentHeaderDomain( httpClientWrapperMock.Object, applicationSettingsHelper.Object );
      Should.Throw<BadRequestException>( () => domain.Get( assessmentEventId ) );
    }

    [Fact]
    public void Get_CallsGetOnAssessmentEventService_Receives_AssessmentEventIsNull()
    {
      var assessmentEventId = 1;

      var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( ( AssessmentEventDto ) null );

      var applicationSettingsHelper = new Mock<IApplicationSettingsHelper>();
      applicationSettingsHelper.Setup( x => x.AssessmentEventServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.RevenueObjectServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.LegalPartyServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.BaseValueSegmentServiceApiUrl ).Returns( "" );

      var domain = new AssessmentHeaderDomain( httpClientWrapperMock.Object, applicationSettingsHelper.Object );
      var assessmentHeaderModel = domain.Get( assessmentEventId ).Result;

      assessmentHeaderModel.ShouldBeNull();
    }

    [Fact]
    public void Get_CallsGetOnAssessmentEventService_ReceivesAssessmentEventServiceDomainDTO_RevenueObjectIsNull()
    {
      var assessmentEventId = 1;
      var assessmentEventDto = new AssessmentEventDto
                               {
                                 Id = assessmentEventId,
                                 EventDate = DateTime.Now,
                                 AsmtEventType = 999,
                                 AsmtEventTypeDescription = "Annual",
                                 RevObjId = 888,
                                 DynCalcStepTrackingId = 777,
                                 TaxYear = 2017,
                                 TranId = 99999999999
                               };
      var assessmentEventTranDto = new AssessmentEventTransactionDto
                                   {
                                     AsmtEventStateDescription = "Review Required",
                                     AsmtEventId = assessmentEventId,
                                     AsmtEventState = 1,
                                     AsmtRevnEventId = 2,
                                     Id = 4
                                   };

      assessmentEventDto.AssessmentEventTransactions.Add( assessmentEventTranDto );

      var baseValueSegmentDto = new BaseValueSegmentDto()
                                {
                                  AsOf = DateTime.Now,
                                  RevenueObjectId = 999,
                                };

      var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( assessmentEventDto );
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventTransactionDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( assessmentEventTranDto );
      httpClientWrapperMock.Setup( x => x.Get<BaseValueSegmentDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( baseValueSegmentDto );

      var applicationSettingsHelper = new Mock<IApplicationSettingsHelper>();
      applicationSettingsHelper.Setup( x => x.AssessmentEventServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.RevenueObjectServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.LegalPartyServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.BaseValueSegmentServiceApiUrl ).Returns( "" );

      var domain = new AssessmentHeaderDomain( httpClientWrapperMock.Object, applicationSettingsHelper.Object );
      var assessmentHeaderModel = domain.Get( assessmentEventId ).Result;

      assessmentHeaderModel.ShouldNotBeNull();
      assessmentHeaderModel.ShouldBeOfType<Models.V1.AssessmentHeader>();

      assessmentHeaderModel.AssessmentEvent.ShouldBeOfType<AssessmentEvent>();
      assessmentHeaderModel.AssessmentEvent.AssessmentEventId.ShouldBe( assessmentEventDto.Id );
      assessmentHeaderModel.AssessmentEvent.EventDate.ShouldBe( assessmentEventDto.EventDate );
      assessmentHeaderModel.AssessmentEvent.AssessmentEventType.ShouldBe( assessmentEventDto.AsmtEventType );
      assessmentHeaderModel.AssessmentEvent.AssessmentEventTypeDescription.ShouldBe( assessmentEventDto.AsmtEventTypeDescription );
      assessmentHeaderModel.AssessmentEvent.RevenueObjectId.ShouldBe( assessmentEventDto.RevObjId );
      assessmentHeaderModel.AssessmentEvent.TaxYear.ShouldBe( assessmentEventDto.TaxYear );
      assessmentHeaderModel.AssessmentEvent.EventState.ShouldBe( assessmentEventDto.AssessmentEventTransactions[ 0 ].AsmtEventStateDescription );
      assessmentHeaderModel.AssessmentEvent.BVSTranType.ShouldBeNullOrEmpty();

      assessmentHeaderModel.RevenueObject.ShouldBeNull();
    }

    [Fact] // TODO: Need to consider the new services we have added.
    public void Get_CallsGetOnAssessmentEventService_ReceivesAssessmentEventServiceDomainDTOAndRevenueObjectDomainAndLegalPartyRolesDomainDTO()
    {
      var assessmentEventId = 1;
      var assessmentEventDate = DateTime.Now;
      const int asmtRevnEventId = 2;
      const int asmtRevnId = 6;
      const int nonPrimeLegalPartyRoleId = 3;
      const int primeLegalPartyRoleId = 4;
      const string primeLegalPartyDisplayName = "Prime Dude";
      const string nonPrimeLegalPartyDisplayName = "Nonprime Dude";
      const string tagDescription = "some TAG description";
      const string firstName = "UnitTestFirstName";
      const string middleName = "UnitTestMiddleName";
      const string lastName = "UnitTestLastName";
      const string nameSfx = "UnitTestNameSfx";
      const int legalPartyRoleObjectType = 9999;

      var assessmentEventDto = new AssessmentEventDto
                               {
                                 Id = assessmentEventId,
                                 EventDate = assessmentEventDate,
                                 AsmtEventType = 999,
                                 AsmtEventTypeDescription = "Annual",
                                 RevObjId = 888,
                                 DynCalcStepTrackingId = 777,
                                 TaxYear = 2017,
                                 TranId = 99999999999,
                                 PrimaryBaseYear = 2015,
                                 PrimaryBaseYearMultipleOrSingleDescription = "M"
                               };
      var assessmentEventTranDto = new AssessmentEventTransactionDto
                                   {
                                     AsmtEventStateDescription = "Review Required",
                                     AsmtEventId = assessmentEventId,
                                     AsmtEventState = 1,
                                     AsmtRevnEventId = asmtRevnEventId,
                                     Id = 4
                                   };

      assessmentEventDto.AssessmentEventTransactions.Add( assessmentEventTranDto );

      var assessmentRevnDto = new AssessmentRevisionDto
                              {
                                Id = asmtRevnId
                              };

      var revenueObjectDto = new RevenueObjectDto
                             {
                               Id = 333
                             };

      var tagDto = new TAGDto
                   {
                     Description = tagDescription
                   };

      var legalPartyRoleDtos = new List<LegalPartyRoleDto>()
                               {
                                 new LegalPartyRoleDto()
                                 {
                                   Id = nonPrimeLegalPartyRoleId,
                                   PrimeLegalParty = 0,
                                   LegalParty = new LegalPartyDto()
                                                {
                                                  DisplayName = nonPrimeLegalPartyDisplayName,
                                                }
                                 },
                                 new LegalPartyRoleDto()
                                 {
                                   Id = primeLegalPartyRoleId,
                                   PrimeLegalParty = 1,
                                   LegalParty = new LegalPartyDto()
                                                {
                                                  Id = 100,
                                                  DisplayName = primeLegalPartyDisplayName,
                                                  FirstName = firstName,
                                                  MiddleName = middleName,
                                                  LastName = lastName,
                                                  NameSfx = nameSfx,
                                                },
                                   ObjectType = legalPartyRoleObjectType
                                 }
                               };
      var bvsTran1 = new BaseValueSegmentTransactionDto()
                     {
                       Id = 1,
                       BaseValueSegmentTransactionTypeId = 3,
                       BaseValueSegmentTransactionType = new BaseValueSegmentTransactionTypeDto()
                                                         {
                                                           Name = "Conversion",
                                                           Description = "Conversion"
                                                         }
                     };
      var bvsTran2 = new BaseValueSegmentTransactionDto()
                     {
                       Id = 2,
                       BaseValueSegmentTransactionTypeId = 2,
                       BaseValueSegmentTransactionType = new BaseValueSegmentTransactionTypeDto()
                                                         {
                                                           Name = "User",
                                                           Description = "User"
                                                         }
                     };
      var baseValueSegmentDto = new BaseValueSegmentDto()
                                {
                                  AsOf = assessmentEventDate,
                                  RevenueObjectId = 888,
                                  BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>()
                                                                 {
                                                                   bvsTran1,
                                                                   bvsTran2
                                                                 }
                                };
      var statutoryReferenceDto = new StatutoryReferenceDto()
                                  {
                                    Description = "Test Statuatory Reference"
                                  };

      var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( assessmentEventDto );

      httpClientWrapperMock.Setup( x => x.Get<AssessmentRevisionDto>( It.IsAny<string>(), It.IsAny<string>() ) )
                           .ReturnsAsync( assessmentRevnDto );
      httpClientWrapperMock.Setup( x => x.Get<RevenueObjectDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( revenueObjectDto );
      httpClientWrapperMock.Setup( x => x.Get<TAGDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( tagDto );
      httpClientWrapperMock.Setup( x => x.Get<IList<LegalPartyRoleDto>>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( legalPartyRoleDtos );
      httpClientWrapperMock.Setup( x => x.Get<BaseValueSegmentDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( baseValueSegmentDto );
      httpClientWrapperMock.Setup( x => x.Get<StatutoryReferenceDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( statutoryReferenceDto );

      var applicationSettingsHelper = new Mock<IApplicationSettingsHelper>();
      applicationSettingsHelper.Setup( x => x.AssessmentEventServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.RevenueObjectServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.LegalPartyServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.BaseValueSegmentServiceApiUrl ).Returns( "" );

      var domain = new AssessmentHeaderDomain( httpClientWrapperMock.Object, applicationSettingsHelper.Object );
      var assessmentHeaderModel = domain.Get( assessmentEventId ).Result;

      assessmentHeaderModel.ShouldNotBeNull();
      assessmentHeaderModel.ShouldBeOfType<Models.V1.AssessmentHeader>();
      assessmentHeaderModel.AssessmentEvent.AssessmentEventId.ShouldBe( assessmentEventDto.Id );
      assessmentHeaderModel.AssessmentEvent.EventDate.ShouldBe( assessmentEventDto.EventDate );
      assessmentHeaderModel.AssessmentEvent.AssessmentEventType.ShouldBe( assessmentEventDto.AsmtEventType );
      assessmentHeaderModel.AssessmentEvent.AssessmentEventTypeDescription.ShouldBe( assessmentEventDto.AsmtEventTypeDescription );
      assessmentHeaderModel.AssessmentEvent.RevenueObjectId.ShouldBe( assessmentEventDto.RevObjId );
      assessmentHeaderModel.AssessmentEvent.TaxYear.ShouldBe( assessmentEventDto.TaxYear );
      assessmentHeaderModel.AssessmentEvent.EventState.ShouldBe( assessmentEventDto.AssessmentEventTransactions[ 0 ].AsmtEventStateDescription );
      assessmentHeaderModel.AssessmentEvent.RevisionId.ShouldBe( assessmentRevnDto.Id );
      assessmentHeaderModel.AssessmentEvent.BVSTranType.ShouldBe( "User" );
      assessmentHeaderModel.AssessmentEvent.TransactionId.ShouldBe( 4 );
      assessmentHeaderModel.AssessmentEvent.Note.ShouldBeNull();
      assessmentHeaderModel.AssessmentEvent.ReferenceNumber.ShouldBeNull();
      assessmentHeaderModel.AssessmentEvent.ChangeReason.ShouldBeNull();
      assessmentHeaderModel.AssessmentEvent.RevenueAndTaxCode.ShouldBe( "Test Statuatory Reference" );
      assessmentHeaderModel.AssessmentEvent.PrimaryBaseYear.ShouldBe( assessmentEventDto.PrimaryBaseYear );
      assessmentHeaderModel.AssessmentEvent.PrimaryBaseYearMultipleOrSingleDescription.ShouldBe( assessmentEventDto.PrimaryBaseYearMultipleOrSingleDescription );
      assessmentHeaderModel.RevenueObject.TAG.ShouldBe( tagDto.Description );
      assessmentHeaderModel.RevenueObject.Id.ShouldBe( 333 );
      assessmentHeaderModel.RevenueObject.Ain.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.AreaCd.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.BeginEffectiveDate.ShouldBe( DateTime.MinValue );
      assessmentHeaderModel.RevenueObject.CensusBlock.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.CensusTrack.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.ClassCd.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.ClassCodeDescription.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.CountyCd.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.Description.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.EffectiveStatus.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.GeoCd.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.Pin.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.PropertyType.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.RelatedPins.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.RevenueCode.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.RightDescription.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.RightEstate.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.RightType.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.SitusAddress.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.SubType.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.TaxCode.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.Type.ShouldBe( 0 );
      assessmentHeaderModel.RevenueObject.UnformattedPin.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.XCoordinate.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.YCoordinate.ShouldBeNull();
      assessmentHeaderModel.RevenueObject.ZCoordinate.ShouldBeNull();
      assessmentHeaderModel.LegalParty.DisplayName.ShouldBe( primeLegalPartyDisplayName );
      assessmentHeaderModel.LegalParty.LegalPartyId.ShouldBe( 100 );
      assessmentHeaderModel.LegalParty.FirstName.ShouldBe( firstName );
      assessmentHeaderModel.LegalParty.MiddleName.ShouldBe( middleName );
      assessmentHeaderModel.LegalParty.LastName.ShouldBe( lastName );
      assessmentHeaderModel.LegalParty.NameSfx.ShouldBe( nameSfx );
      assessmentHeaderModel.LegalParty.RevenueAcct.ShouldBe( 0 );
      assessmentHeaderModel.LegalParty.LegalPartyRoleObjectType.ShouldBe( legalPartyRoleObjectType );

    }

    [Fact]
    public void Get_CallsGetOnAssessmentEventService_ReceivesAssessmentEventServiceDomainDTO_GetRevenueObjectThrowsNotFoundException()
    {
      var assessmentEventId = 1;
      var assessmentEventDto = new AssessmentEventDto
                               {
                                 Id = assessmentEventId,
                                 EventDate = DateTime.Now,
                                 AsmtEventType = 999,
                                 AsmtEventTypeDescription = "Annual",
                                 RevObjId = 888,
                                 DynCalcStepTrackingId = 777,
                                 TaxYear = 2017,
                                 TranId = 99999999999
                               };
      var assessmentEventTranDto = new AssessmentEventTransactionDto
                                   {
                                     AsmtEventStateDescription = "Review Required",
                                     AsmtEventId = assessmentEventId,
                                     AsmtEventState = 1,
                                     AsmtRevnEventId = 2,
                                     Id = 4
                                   };

      assessmentEventDto.AssessmentEventTransactions.Add( assessmentEventTranDto );

      var baseValueSegmentDto = new BaseValueSegmentDto()
                                {
                                  AsOf = DateTime.Now,
                                  RevenueObjectId = 999,
                                };

      var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( assessmentEventDto );
      httpClientWrapperMock.Setup( x => x.Get<AssessmentEventTransactionDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( assessmentEventTranDto );
      httpClientWrapperMock.Setup( x => x.Get<BaseValueSegmentDto>( It.IsAny<string>(), It.IsAny<string>() ) ).ReturnsAsync( baseValueSegmentDto );

      var applicationSettingsHelper = new Mock<IApplicationSettingsHelper>();
      applicationSettingsHelper.Setup( x => x.AssessmentEventServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.RevenueObjectServiceApiUrl ).Throws( new NotFoundException( "something" ) );
      applicationSettingsHelper.Setup( x => x.LegalPartyServiceApiUrl ).Returns( "" );
      applicationSettingsHelper.Setup( x => x.BaseValueSegmentServiceApiUrl ).Returns( "" );

      var domain = new AssessmentHeaderDomain( httpClientWrapperMock.Object, applicationSettingsHelper.Object );
      Should.Throw<NotFoundException>( () => domain.Get( assessmentEventId ) );
    }
  }
}
