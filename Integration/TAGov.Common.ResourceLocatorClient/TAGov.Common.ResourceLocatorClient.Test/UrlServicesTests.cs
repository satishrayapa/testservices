using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace TAGov.Common.ResourceLocatorClient.Test
{
	[TestFixture]
	public class UrlServicesTests
	{
		[Test]
		public void ReturnUriWhenResourceDtoIsFound()
		{
			var list = new List<ResourceDto>
			{
				new ResourceDto { Key = "foo", Value = "http://foo" },
				new ResourceDto { Key = "bar", Value = "http://bar" }
			};

			var restClientMock = new Mock<IRestClient>();
			restClientMock.Setup(x => x.GetResources(It.IsAny<string>())).Returns(list);

			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(It.IsAny<string>())).Returns("bar");

			var urlServices = new UrlServices(restClientMock.Object, configurationMock.Object);

			var uri = urlServices.GetServiceUri("foo");

			Assert.That(uri.ToString(), Is.EqualTo("http://foo/"));
		}

		[Test]
		public void ReturnNullWhenNoResourceDtoIsFound()
		{
			var restClientMock = new Mock<IRestClient>();
			restClientMock.Setup(x => x.GetResources(It.IsAny<string>())).Returns(() => null);

			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(It.IsAny<string>())).Returns("bar");

			var urlServices = new UrlServices(restClientMock.Object, configurationMock.Object);

			Assert.That(urlServices.GetServiceUri("foo"), Is.Null);
		}

		[Test]
		public void ReturnNullWhenPartitionConfigurationIsNull()
		{
			var configurationMock = new Mock<IConfiguration>();

			var urlServices = new UrlServices(null, configurationMock.Object);

			Assert.That(urlServices.GetServiceUri("foo"), Is.Null);
		}
	}
}
