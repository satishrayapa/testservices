using Moq;
using NUnit.Framework;
using TAGov.Common.Security.SecurityClient;

namespace TAGov.Common.ResourceLocatorClient.IntegrationTest
{
	[TestFixture]
	public class UrlServicesTests
	{
		[Test]
		public void GetLegalPartySearchUri()
		{
			var configuration = new Dev1Configuration();

			var securityTokenServiceProxy = new Mock<ISecurityTokenServiceProxy>();

			securityTokenServiceProxy.Setup(x => x.GetAccessToken()).Returns("HELLO");

			var httpClientProxy = new HttpClientProxy(securityTokenServiceProxy.Object);

			var urlService = new UrlServices(new RestClient(configuration, httpClientProxy), configuration);

			var uri = urlService.GetServiceUri("service.legalpartysearch");
			Assert.That(uri.ToString(), Is.EqualTo("http://c592xfglumwb1.ecomqc.tlrg.com/service.legalpartysearch"));
		}
	}
}
