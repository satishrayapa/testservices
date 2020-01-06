using System.Linq;
using Moq;
using NUnit.Framework;

namespace TAGov.Common.ResourceLocatorClient.Test
{
	[TestFixture]
	public class RestClientTests
	{
		[Test]
		public void WhenRestClientReturnsDtoStringItIsConvertedToDto()
		{
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorPartition)).Returns("part");
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorUri)).Returns("http://foo");

			var httpClientProxyMock = new Mock<IHttpClientProxy>();
			httpClientProxyMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns("{ \"key\":\"foo\", \"partition\":\"part\", \"value\":\"bar\" }");
			var restClient = new RestClient(configurationMock.Object, httpClientProxyMock.Object);

			var dto = restClient.GetResource("foo");

			Assert.That(dto.Key, Is.EqualTo("foo"));
			Assert.That(dto.Partition, Is.EqualTo("part"));
			Assert.That(dto.Value, Is.EqualTo("bar"));
		}

		[Test]
		public void WhenConfigurationPartIsNullReturnsNullDto()
		{
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorUri)).Returns("http://foo");

			var restClient = new RestClient(configurationMock.Object, null);

			var dto = restClient.GetResource("foo");

			Assert.That(dto, Is.Null);
		}

		[Test]
		public void WhenConfigurationUriIsNullReturnsNullDto()
		{
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorPartition)).Returns("part");

			var restClient = new RestClient(configurationMock.Object, null);

			var dto = restClient.GetResource("foo");

			Assert.That(dto, Is.Null);
		}

		[Test]
		public void WhenRestClientReturnsDtoListStringItIsConvertedToDtoList()
		{
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorPartition)).Returns("part");
			configurationMock.Setup(x => x.Get(Constants.CommonResourceLocatorUri)).Returns("http://foo");

			var httpClientProxyMock = new Mock<IHttpClientProxy>();
			httpClientProxyMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns("[{ \"key\":\"foo1\", \"partition\":\"part1\", \"value\":\"bar1\" },{ \"key\":\"foo2\", \"partition\":\"part2\", \"value\":\"bar2\" }]");
			var restClient = new RestClient(configurationMock.Object, httpClientProxyMock.Object);

			var dtos = restClient.GetResources("foo").ToList();

			Assert.That(dtos[0].Key, Is.EqualTo("foo1"));
			Assert.That(dtos[0].Partition, Is.EqualTo("part1"));
			Assert.That(dtos[0].Value, Is.EqualTo("bar1"));

			Assert.That(dtos[1].Key, Is.EqualTo("foo2"));
			Assert.That(dtos[1].Partition, Is.EqualTo("part2"));
			Assert.That(dtos[1].Value, Is.EqualTo("bar2"));
		}
	}
}
