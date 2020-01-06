using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Implementation;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Tests
{
	public class AumentumDomainTests
	{
		private readonly Mock<ICaliforniaConsumerPriceIndexRepository> _mockRepository;
		private readonly Mock<ISysTypeRepository> _mockSysTypeRepository;
		private readonly AumentumDomain _aumentumDomain;
		public AumentumDomainTests()
		{
			_mockRepository = new Mock<ICaliforniaConsumerPriceIndexRepository>();
			_mockSysTypeRepository = new Mock<ISysTypeRepository>();
			_aumentumDomain = new AumentumDomain( _mockRepository.Object, _mockSysTypeRepository.Object );
		}

		#region Get All CA Consumer Price Index Tests

		[Fact]
		public void GetAllCaConsumerPriceIndexes_ReturnsConsumerPriceIndexes()
		{
			var caConsumerPriceIndexes =
				new List<CaliforniaConsumerPriceIndex> { new CaliforniaConsumerPriceIndex { AssessmentYear = 1980, InflationFactor = (decimal)1.1 } };

			_mockRepository.Setup(x => x.List()).Returns(caConsumerPriceIndexes);

			var returnCaConsumerPriceIndexes = _aumentumDomain.GetAllCaConsumerPriceIndexes().ToList();

			returnCaConsumerPriceIndexes.ShouldNotBeEmpty();
			returnCaConsumerPriceIndexes.Count.ShouldBe(1);
			returnCaConsumerPriceIndexes.Single().AssessmentYear.ShouldBe(1980);
			returnCaConsumerPriceIndexes.Single().InflationFactor.ShouldBe((decimal)1.1);
		}

		[Fact]
		public void GetAllCaConsumerPriceIndexes_NotFoundExceptionIsThrown()
		{
			_mockRepository.Setup(x => x.List()).Returns(new List<CaliforniaConsumerPriceIndex>());

			Should.Throw<RecordNotFoundException>(() => _aumentumDomain.GetAllCaConsumerPriceIndexes());
		}

		#endregion

		private void SeedCaliforniaConsumerPriceIndexFrom1990To2000()
		{
			for (int year = 1990; year < 2001; year++)
			{
				// ReSharper disable once AccessToModifiedClosure
				_mockRepository.Setup(x => x.GetByYear(year))
					.Returns(new CaliforniaConsumerPriceIndex
					{
						InflationFactor = 1.1M
					});
			}
		}

		[Fact]
		public void AnnualAssessmentMonthOfJanBaseValueOf1000WithInflationFactorOfOnePointOnePercentFrom1990To1999ShouldGetFbyvAs2357()
		{
			SeedCaliforniaConsumerPriceIndexFrom1990To2000();
			//GetSysTypeId is used to get the annual systype id so return the same id
			const int annualAssessmentEventType = 1;
			_mockSysTypeRepository.Setup( x => x.GetSysTypeId( It.IsAny<string>(), It.IsAny<string>() ) ).Returns( annualAssessmentEventType );
			var dto = _aumentumDomain.GetFactoredBasedYearValue(new DateTime(1999, 1, 1), 1990, 1000, annualAssessmentEventType);

			dto.AssessmentYear.ShouldBe(1999);
			dto.Fbyv.ShouldBe(2357);
		}

		[Fact]
		public void NonAnnualAssessmentMonthOfJanBaseValueOf1000WithInflationFactorOfOnePointOnePercentFrom1990To1999ShouldGetFbyvAs2593()
		{
			SeedCaliforniaConsumerPriceIndexFrom1990To2000();
			//GetSysTypeId is used to get the annual systype id so return a different id than passed in
			const int annualAssessmentEventType = 1;
			const int nonAnnualAssessmentEventType = 2;
			_mockSysTypeRepository.Setup( x => x.GetSysTypeId( It.IsAny<string>(), It.IsAny<string>() ) ).Returns( annualAssessmentEventType );
			var dto = _aumentumDomain.GetFactoredBasedYearValue(new DateTime(1999, 1, 1), 1990, 1000, nonAnnualAssessmentEventType);

			//the next year
			dto.AssessmentYear.ShouldBe(2000);
			dto.Fbyv.ShouldBe(2593);
		}

		[Fact]
		public void BaseYearOf1976BeforeProposition13StartedIn1977ShouldGetBadRequestException()
		{
			Should.Throw<BadRequestException>(() => _aumentumDomain.GetFactoredBasedYearValue(new DateTime(1999, 1, 1), 1976, 1000, 1));
		}

		[Fact]
		public void AnnualAssessmentMonthOfJulBaseValueOf1000WithInflationFactorOfOnePointOnePercentFrom1990To1999ShouldGetFbyvAs2357AndYear1999()
		{
			SeedCaliforniaConsumerPriceIndexFrom1990To2000();
			//GetSysTypeId is used to get the annual systype id so return the same id
			const int annualAssessmentEventType = 1;
			_mockSysTypeRepository.Setup( x => x.GetSysTypeId( It.IsAny<string>(), It.IsAny<string>() ) ).Returns( annualAssessmentEventType );
			var dto = _aumentumDomain.GetFactoredBasedYearValue(new DateTime(1999, 7, 1), 1990, 1000, annualAssessmentEventType);

			//annual uses the same year
			dto.AssessmentYear.ShouldBe(1999);
			dto.Fbyv.ShouldBe(2357);
		}

		[Fact]
		public void InflationFactorNotFoundShouldGetRecordNotFoundException()
		{
			SeedCaliforniaConsumerPriceIndexFrom1990To2000();

			Should.Throw<RecordNotFoundException>(() => _aumentumDomain.GetFactoredBasedYearValue(new DateTime(1999, 1, 1), 1988, 1000, 1));
		}

		[Fact]
		public void GetFactoredBasedYearValue()
		{
			SeedCaliforniaConsumerPriceIndexFrom1990To2000();
			//GetSysTypeId is used to get the annual systype id so return the same id
			const int annualAssessmentEventType = 1;
			_mockSysTypeRepository.Setup(x => x.GetSysTypeId(It.IsAny<string>(), It.IsAny<string>())).Returns(annualAssessmentEventType);
			var dtos = _aumentumDomain.GetFactoredBasedYearValue(new[]{new FactorBaseYearValueRequestDto
			{
				EventDate = new DateTime(1999,1,1), AssessmentYear=1990, BaseValue=1000, AssessmentEventType = 1
			}});

			dtos.Single().Fbyv.ShouldBe(2357);
			dtos.Single().FbyvYear.ShouldBe( 1999 );
		}
	}
}