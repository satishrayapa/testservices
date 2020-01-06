using System;
using Moq;
using Shouldly;
using System.Collections.Generic;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.AssessmentEvent.Domain.Implementation;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Tests
{
  public class AssessmentEventDomainTests
  {
    /***********************  Get ********************/

    [Fact]
    public void Get_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      const int maxTranId = 2;
      const int minTranId = 1;
      const int maxPrimaryBaseYear = 2018;
      const string primaryBaseYearMultiple = "M";

      var assesmentEvent = new Repository.Models.V1.AssessmentEvent
                           {
                             Id = 999,
                             AssessmentEventTransactions = new List<AssessmentEventTransaction>
                                                           {
                                                             new AssessmentEventTransaction
                                                             {
                                                               Id = minTranId
                                                             },
                                                             new AssessmentEventTransaction
                                                             {
                                                               Id = maxTranId
                                                             }
                                                           }
                           };
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAsync( 999 ) ).ReturnsAsync( assesmentEvent );
      moqRepository.Setup( x => x.GetAssessmentEventValueByAssessmentEventTransactionIdAsync( maxTranId ) )
                   .ReturnsAsync( new AssessmentEventValue
                                  {
                                    Attribute1 = maxPrimaryBaseYear,
                                    Attribute2Description = primaryBaseYearMultiple
                                  } );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      var returnAssesmentEvent = assesmentEventDomain.GetAsync( 999 ).Result;

      returnAssesmentEvent.Id.ShouldBe( assesmentEvent.Id );
      returnAssesmentEvent.PrimaryBaseYear.ShouldBe( maxPrimaryBaseYear );
      returnAssesmentEvent.PrimaryBaseYearMultipleOrSingleDescription.ShouldBe( primaryBaseYearMultiple );
    }

    public void Get_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned_NullAssessmentEventTransactionsList()
    {
      var assesmentEvent = new Repository.Models.V1.AssessmentEvent
                           {
                             Id = 999,
                             AssessmentEventTransactions = null
                           };
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAsync( 999 ) ).ReturnsAsync( assesmentEvent );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      var returnAssesmentEvent = assesmentEventDomain.GetAsync( 999 ).Result;

      returnAssesmentEvent.Id.ShouldBe( assesmentEvent.Id );
      returnAssesmentEvent.PrimaryBaseYear.ShouldBeNull();
      returnAssesmentEvent.PrimaryBaseYearMultipleOrSingleDescription.ShouldBeNull();
    }

    public void Get_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned_EmptyAssessmentEventTransactionsList()
    {
      var assesmentEvent = new Repository.Models.V1.AssessmentEvent
                           {
                             Id = 999,
                             AssessmentEventTransactions = new List<AssessmentEventTransaction>()
                           };
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAsync( 999 ) ).ReturnsAsync( assesmentEvent );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      var returnAssesmentEvent = assesmentEventDomain.GetAsync( 999 ).Result;

      returnAssesmentEvent.Id.ShouldBe( assesmentEvent.Id );
      returnAssesmentEvent.PrimaryBaseYear.ShouldBeNull();
      returnAssesmentEvent.PrimaryBaseYearMultipleOrSingleDescription.ShouldBeNull();
    }

    [Fact]
    public void Get_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
    {
      var assesmentEvent = new Repository.Models.V1.AssessmentEvent();
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAsync( 999 ) ).ReturnsAsync( assesmentEvent );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      Should.ThrowAsync<RecordNotFoundException>( () => assesmentEventDomain.GetAsync( 1 ) );
    }

    [Fact]
    public void Get_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
    {
      var assesmentEventDomain = new AssessmentEventDomain( null );
      Should.ThrowAsync<BadRequestException>( () => assesmentEventDomain.GetAsync( -1 ) );
    }

    /************ GetAssessmentRevisionByAssessmentRevisionEventId **************/


    [Fact]
    public void GetAssessmentRevisionByAssessmentRevisionEventId_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      var assessmentRevision = new AssessmentRevision { Id = 999, ReferenceNumber = "None" };
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAssessmentRevisionByAssessmentRevisionEventId( 999, It.IsAny<DateTime>() ) ).Returns( assessmentRevision );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      var returnAssesmentRevision = assesmentEventDomain.GetAssessmentRevisionByAssessmentRevisionEventId( 999, new DateTime( 2016, 1, 1 ) );

      returnAssesmentRevision.Id.ShouldBe( assessmentRevision.Id );
      returnAssesmentRevision.ReferenceNumber.ShouldBe( assessmentRevision.ReferenceNumber );
    }

    [Fact]
    public void GetAssessmentRevisionByAssessmentRevisionEventId_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
    {
      var assessmentRevision = new AssessmentRevision();
      var moqRepository = new Mock<IAssessmentEventRepository>();
      moqRepository.Setup( x => x.GetAssessmentRevisionByAssessmentRevisionEventId( 999, It.IsAny<DateTime>() ) ).Returns( assessmentRevision );

      var assesmentEventDomain = new AssessmentEventDomain( moqRepository.Object );
      Should.Throw<RecordNotFoundException>( () => assesmentEventDomain.GetAssessmentRevisionByAssessmentRevisionEventId( 1, new DateTime( 2016, 1, 1 ) ) );
    }

    [Fact]
    public void GetAssessmentRevisionByAssessmentRevisionEventId_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
    {
      var assesmentEventDomain = new AssessmentEventDomain( null );
      Should.Throw<BadRequestException>( () => assesmentEventDomain.GetAssessmentRevisionByAssessmentRevisionEventId( -1, new DateTime( 2016, 1, 1 ) ) );
    }
  }
}
