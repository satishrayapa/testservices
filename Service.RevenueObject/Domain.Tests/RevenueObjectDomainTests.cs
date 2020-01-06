using System;
using System.Collections.Generic;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.RevenueObject.Domain.Implementation;
using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;
using Xunit;

namespace TAGov.Services.Core.RevenueObject.Domain.Tests
{
  public class RevenueObjectDomainTests
  {
    [Fact]
    public void Get_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject { Id = 999 };
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      var returnAssesmentEvent = revenueObjectDomain.Get( 999, DateTime.Today );

      returnAssesmentEvent.Id.ShouldBe( revenueObject.Id );
    }

    [Fact]
    public void Get_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      Should.Throw<RecordNotFoundException>( () => revenueObjectDomain.Get( 1, DateTime.Today ) );
    }

    [Fact]
    public void Get_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      Should.Throw<BadRequestException>( () => revenueObjectDomain.Get( 0, DateTime.Today ) );
    }

    [Fact]
    public void Get_CallsRepository_InValidIdIsPassedToRepository_MarketAndRestrictedValuesReturnsNull()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject();
      IList<Repository.Models.V1.MarketAndRestrictedValue> marketAndRestrictedValue = null;

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      var returnAssessmentEvent = revenueObjectDomain.Get( 999, DateTime.Today );

      returnAssessmentEvent.Id.ShouldBe( revenueObject.Id );
      returnAssessmentEvent.MarketAndRestrictedValues.ShouldBeNull();
    }

    [Fact]
    public void Get_CallsRepository_ValidIdIsPassedToRepository_MarketAndRestrictedValuesAreReturned()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      var returnAssessmentEvent = revenueObjectDomain.Get( 999, DateTime.Today );

      returnAssessmentEvent.Id.ShouldBe( revenueObject.Id );
      returnAssessmentEvent.MarketAndRestrictedValues.Count.ShouldBe( 1 );
      returnAssessmentEvent.MarketAndRestrictedValues[ 0 ].SubComponent.ShouldBe( marketAndRestrictedValue[ 0 ].SubComponent );
      returnAssessmentEvent.MarketAndRestrictedValues[ 0 ].MarketValue.ShouldBe( marketAndRestrictedValue[ 0 ].MarketValue );
      returnAssessmentEvent.MarketAndRestrictedValues[ 0 ].RestrictedValue.ShouldBe( marketAndRestrictedValue[ 0 ].RestrictedValue );
    }

    [Fact]
    public void Get_CallsRepository_InValidIdIsPassedToRepository_MarketAndRestrictedValuesAreNotReturned()
    {
      var revenueObject = new Repository.Models.V1.RevenueObject();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>();

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.Get( 999, DateTime.Today ) ).Returns( revenueObject );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      var returnAssessmentEvent = revenueObjectDomain.Get( 999, DateTime.Today );

      returnAssessmentEvent.Id.ShouldBe( revenueObject.Id );
      returnAssessmentEvent.MarketAndRestrictedValues.Count.ShouldBe( 0 );
    }

    [Fact]
    public void GetTAGByRevenueObjectId_CallsRepository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      var beginEffectiveDate = DateTime.Today;
      var tag = new Repository.Models.V1.TAG { Id = 999 };
      var moqRepository = new Mock<IRevenueObjectRepository>();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      moqRepository.Setup( x => x.GetTAGByRevenueObjectId( 999, beginEffectiveDate ) ).Returns( tag );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      var returnTag = revenueObjectDomain.GetTAGByRevenueObjectId( 999, beginEffectiveDate );

      returnTag.Id.ShouldBe( tag.Id );
    }

    [Fact]
    public void GetTAGByRevenueObjectId_CallsRepository_ValidMissingIdIsPassedToRepository_NotFoundExceptionIsThrown()
    {
      var tag = new Repository.Models.V1.TAG();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.GetTAGByRevenueObjectId( 999, DateTime.Today ) ).Returns( tag );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      Should.Throw<RecordNotFoundException>(
        () => revenueObjectDomain.GetTAGByRevenueObjectId( 1, DateTime.Today ) );
    }

    [Fact]
    public void GetTAGByRevenueObjectId_CallsRepository_InvalidIdIsPassedToRepository_BadRequestExceptionIsThrown()
    {
      var tag = new Repository.Models.V1.TAG();
      var marketAndRestrictedValue = new List<Repository.Models.V1.MarketAndRestrictedValue>
                                     {
                                       new Repository.Models.V1.MarketAndRestrictedValue { SubComponent = 123, MarketValue = 9999, RestrictedValue = 999 }
                                     };

      var moqRepository = new Mock<IRevenueObjectRepository>();
      moqRepository.Setup( x => x.GetTAGByRevenueObjectId( 999, DateTime.Today ) ).Returns( tag );

      var moqMarketAndRestrictedRepository = new Mock<IMarketAndRestrictedValueRepository>();
      moqMarketAndRestrictedRepository.Setup( x => x.Get( It.IsAny<int>(), It.IsAny<DateTime>() ) ).Returns( marketAndRestrictedValue );

      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, moqMarketAndRestrictedRepository.Object );
      Should.Throw<BadRequestException>(
        () => revenueObjectDomain.GetTAGByRevenueObjectId( 0, DateTime.Today ) );
    }

    [Fact]
    public void GetRevenueObjectSitusAddressByBin_CallsRespository_ValidRecordIsReturnedFromRepository_RecordIsReturned()
    {
      var pin = "some pin";
      var moqRepository = new Mock<IRevenueObjectRepository>();
      var revenueObject = new Repository.Models.V1.RevenueObject { Pin = pin };
      moqRepository.Setup( x => x.GetRevenueObjectSitusAddressByPin( It.IsAny<string>() ) ).Returns( revenueObject );
      var revenueObjectDomain = new RevenueObjectDomain( moqRepository.Object, new Mock<IMarketAndRestrictedValueRepository>().Object );
      var returnRevenuObject = revenueObjectDomain.GetRevenueObjectSitusAddressByPin( pin );
      returnRevenuObject.Pin.ShouldBe( pin );
    }
  }
}
