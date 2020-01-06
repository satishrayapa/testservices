using System;
using Moq;
using NUnit.Framework;

namespace TAGov.Common.Security.SecurityClient.Test
{
	[TestFixture]
	public class SecurityTokenServiceProxyTests
	{
		[Test]
		public void GetExceptionWhenSecurityConfigurationIsNotSet()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.Catch<ApplicationException>(() => securityTokenServiceProxy.GetAccessToken());
		}

		[Test]
		public void GetExceptionWhenBothSecurityConfigurationAreBetweenServiceAndNoneService()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			configuration.Setup(x => x.ClientId).Returns("abc");
			configuration.Setup(x => x.ServiceClientId).Returns("abc");

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.Catch<ApplicationException>(() => securityTokenServiceProxy.GetAccessToken());
		}

		[Test]
		public void GetExceptionWhenPartServiceSecurityConfigurationSet()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			configuration.Setup(x => x.ServiceClientId).Returns("abc");

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.Catch<ApplicationException>(() => securityTokenServiceProxy.GetAccessToken());
		}

		[Test]
		public void GetExceptionWhenPartNoneServiceSecurityConfigurationSet()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			configuration.Setup(x => x.ClientId).Returns("abc");

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.Catch<ApplicationException>(() => securityTokenServiceProxy.GetAccessToken());
		}

		[Test]
		public void GetAccessTokenWhenAllServiceSecurityConfigurationSet()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			configuration.Setup(x => x.ServiceClientId).Returns("abc");
			configuration.Setup(x => x.ServiceClientPassword).Returns("abc");
			configuration.Setup(x => x.ServiceClientScope).Returns("abc");

			jwtClient.Setup(x => x.ProcessByServiceCredentials()).Returns(new JwtTokenRequestResult
			{
				AccessToken = "foo"
			});

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.That(securityTokenServiceProxy.GetAccessToken(), Is.EqualTo("foo"));
		}

		[Test]
		public void GetAccessTokenWhenAllNoneServiceSecurityConfigurationSet()
		{
			var jwtClient = new Mock<IJwtTokenRequestClient>();
			var configuration = new Mock<ISecurityConfiguration>();
			configuration.Setup(x => x.ClientId).Returns("abc");
			configuration.Setup(x => x.ClientPassword).Returns("abc");
			configuration.Setup(x => x.ClientScope).Returns("abc");

			jwtClient.Setup(x => x.ProcessByUserProfileId()).Returns(new JwtTokenRequestResult
			{
				AccessToken = "bar"
			});

			var securityTokenServiceProxy = new SecurityTokenServiceProxy(jwtClient.Object, configuration.Object);

			Assert.That(securityTokenServiceProxy.GetAccessToken(), Is.EqualTo("bar"));
		}
	}
}
