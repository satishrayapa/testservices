using System;
using Moq;
using Shouldly;
using TAGov.Common.Caching;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation;
using Xunit;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Tests
{
	public class CaliforniaConsumerPriceIndexTests
	{
		[Fact]
		public void GetFactoredBasedYearValue_WithBaseYearOf2010AndValueOf150000_In2016ValueIs163558()
		{
			var memoryCacheMock = new Mock<ICacheHelper>();

			var californiaConsumerPriceIndex = new CaliforniaConsumerPriceIndex(
				new CaliforniaConsumerPriceIndexRepository(memoryCacheMock.Object));

			var fbyv = californiaConsumerPriceIndex.GetFactoredBasedYearValue(new DateTime(2016, 1, 1), 2010, 150000);

			fbyv.ShouldBe(163558);
		}
	}
}
