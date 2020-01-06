using Moq;
using NUnit.Framework;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Common.ResourceLocatorClient.IntegrationTest
{
	[TestFixture]
	public class FeatureToggleTests
	{
		[Test]
		public void FeatureIsTurnedOffWhenResourceReturnedIsNull()
		{
			var restClient = new Mock<IRestClient>();
			
			var featureToggle  =new FeatureToggle(restClient.Object);
			Assert.That(featureToggle.IsEnabled(Features.BaseValueSegment), Is.False);
		}

		[Test]
		public void FeatureIsTurnedOffWhenResourceReturnedValueIsFalse()
		{
			var restClient = new Mock<IRestClient>();
			restClient.Setup(x => x.GetResource(It.IsAny<string>())).Returns(new ResourceDto {Value = "false"});
			var featureToggle = new FeatureToggle(restClient.Object);
			
			Assert.That(featureToggle.IsEnabled(Features.BaseValueSegment), Is.False);
		}

		[Test]
		public void FeatureIsOnWhenResourceReturnedValueIsTrue()
		{
			var restClient = new Mock<IRestClient>();
			restClient.Setup(x => x.GetResource(It.IsAny<string>())).Returns(new ResourceDto { Value = "true" });
			var featureToggle = new FeatureToggle(restClient.Object);

			Assert.That(featureToggle.IsEnabled(Features.BaseValueSegment), Is.True);
		}
	}
}
