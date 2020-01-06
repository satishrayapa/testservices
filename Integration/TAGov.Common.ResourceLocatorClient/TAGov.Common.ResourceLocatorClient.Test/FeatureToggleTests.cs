using Moq;
using NUnit.Framework;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Common.ResourceLocatorClient.Test
{
	[TestFixture]
	public class FeatureToggleTests
	{
		[Test]
		public void WhenClientReturnsValueAsTrueIsEnabledIsTrue()
		{
			var clientMock = new Mock<IRestClient>();

			clientMock.Setup(x => x.GetResource(Features.BaseValueSegment.GetResourceKey())).Returns(new ResourceDto { Key = Features.BaseValueSegment.GetResourceKey(), Partition = "part", Value = "true" });

			var featureToggle = new FeatureToggle(clientMock.Object);
			var value = featureToggle.IsEnabled(Features.BaseValueSegment);

			Assert.That(value, Is.True);
		}

		[Test]
		public void WhenClientReturnsValueAsFalseIsEnabledIsFalse()
		{
			var clientMock = new Mock<IRestClient>();
			clientMock.Setup(x => x.GetResource(Features.BaseValueSegment.GetResourceKey())).Returns(new ResourceDto { Key = Features.BaseValueSegment.GetResourceKey(), Partition = "part", Value = "false" });

			var featureToggle = new FeatureToggle(clientMock.Object);
			var value = featureToggle.IsEnabled(Features.BaseValueSegment);

			Assert.That(value, Is.False);
		}

		[Test]
		public void WhenClientReturnsValueAsNullIsEnabledIsFalse()
		{
			var clientMock = new Mock<IRestClient>();
			clientMock.Setup(x => x.GetResource(Features.BaseValueSegment.GetResourceKey())).Returns(new ResourceDto { Key = Features.BaseValueSegment.GetResourceKey(), Partition = "part" });

			var featureToggle = new FeatureToggle(clientMock.Object);
			var value = featureToggle.IsEnabled(Features.BaseValueSegment);

			Assert.That(value, Is.False);

			clientMock.Verify(x => x.GetResource(Features.BaseValueSegment.GetResourceKey()), Times.Once);
		}
	}
}
