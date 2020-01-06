using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using TAGov.Common.ResourceLocatorClient;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Search.Test
{
	[TestFixture]
	public class LegalPartySearchProxyTests
	{
		[Test]
		public void CanHandleIsBasedOnFeatureToggle()
		{
			var featureToggleMock = new Mock<IFeatureToggle>();

			var legalPartySearchProxy = new LegalPartySearchProxy(null, featureToggleMock.Object, null);
			legalPartySearchProxy.CanHandle(Features.LegalPartySearch);

			featureToggleMock.Verify(x => x.IsEnabled(Features.LegalPartySearch), Times.Once);
		}

		[Test]
		public void GetLegalPartySearchResultsConvertedToDtoList()
		{
			var current = System.Reflection.Assembly.GetExecutingAssembly();
			string json;
			// ReSharper disable once AssignNullToNotNullAttribute
			using (var streamReader = new StreamReader(current.GetManifestResourceStream("TAGov.Search.Test.testData.json")))
			{
				json = streamReader.ReadToEnd();
			}

			var httpClientProxyMock = new Mock<IHttpClientProxy>();

			httpClientProxyMock.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<string>(), It.Is<SearchLegalPartyQueryDto>(y =>
				y.IsActive == true &&
				y.EffectiveDate.Value.Year == 2017 &&
				y.EffectiveDate.Value.Month == 1 &&
				y.EffectiveDate.Value.Day == 1 &&
				y.SearchText == "foo"))).Returns(() => json);

			var urlServicesMock = new Mock<IUrlServices>();
			urlServicesMock.Setup(x => x.GetServiceUri(Constants.ServiceLegalPartySearch)).Returns(new Uri("http://foo"));

			var legalPartySearchProxy = new LegalPartySearchProxy(httpClientProxyMock.Object, null, urlServicesMock.Object);

			var legalParties = legalPartySearchProxy.Search(new SearchLegalPartyQueryDto { SearchText = "foo", EffectiveDate = new DateTime(2017, 1, 1), MaxRows = 700, IsActive = true }).ToList();

			Assert.That(legalParties.Count, Is.EqualTo(1));
			Assert.That(legalParties[0].DisplayName, Is.EqualTo("foobar"));
			Assert.That(legalParties[0].Address, Is.EqualTo("123 main"));
			Assert.That(legalParties[0].StreetType, Is.EqualTo("RD"));
		}

		[Test]
		public void GetLegalPartySearchNullReturnsNull()
		{
			var httpClientProxyMock = new Mock<IHttpClientProxy>();

			httpClientProxyMock.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<string>(), It.Is<SearchLegalPartyQueryDto>(y =>
				y.IsActive == true &&
				y.EffectiveDate.Value.Year == 2017 &&
				y.EffectiveDate.Value.Month == 1 &&
				y.EffectiveDate.Value.Day == 1 &&
				y.SearchText == "foo"))).Returns(() => null);

			var urlServicesMock = new Mock<IUrlServices>();
			urlServicesMock.Setup(x => x.GetServiceUri(Constants.ServiceLegalPartySearch)).Returns(new Uri("http://foo"));

			var legalPartySearchProxy = new LegalPartySearchProxy(httpClientProxyMock.Object, null, urlServicesMock.Object);

			var legalParties = legalPartySearchProxy.Search(new SearchLegalPartyQueryDto { SearchText = "foo", EffectiveDate = new DateTime(2017, 1, 1), MaxRows = 700, IsActive = true });

			Assert.That(legalParties, Is.Null);
		}
	}
}
