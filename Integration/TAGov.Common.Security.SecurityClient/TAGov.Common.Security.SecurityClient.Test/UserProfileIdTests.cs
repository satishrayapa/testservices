using System.Web;
using Moq;
using NUnit.Framework;

namespace TAGov.Common.Security.SecurityClient.Test
{
	[TestFixture]
	public class UserProfileIdTests
	{
		[Test]
		public void UserIsNotAuthenticatedInWhenHttpContextIsFalse()
		{
			var loggerMock = new Mock<IInternalLogger>();
			var webContextMock = new Mock<IWebContext>();

			webContextMock.Setup(x => x.IsExist()).Returns(false);

			var userLogin = new UserProfileId(webContextMock.Object, loggerMock.Object);
			int id;
			var authenticated = userLogin.IsAuthenticated(out id);

			Assert.That(authenticated, Is.False);
			Assert.That(id, Is.EqualTo(-1));
		}

		[Test]
		public void UserIsNotAuthenticatedInWhenSessionProfileLoginIsNull()
		{
			var loggerMock = new Mock<IInternalLogger>();
			var webContextMock = new Mock<IWebContext>();

			webContextMock.Setup(x => x.IsExist()).Returns(true);

			var userLogin = new UserProfileId(webContextMock.Object, loggerMock.Object);
			int id;
			var authenticated = userLogin.IsAuthenticated(out id);

			Assert.That(authenticated, Is.False);
			Assert.That(id, Is.EqualTo(-1));
		}

		[Test]
		public void UserIsAuthenticatedInWhenSessionProfileLoginIsSetAndNumber()
		{
			var loggerMock = new Mock<IInternalLogger>();
			var webContextMock = new Mock<IWebContext>();

			webContextMock.Setup(x => x.IsExist()).Returns(true);
			webContextMock.Setup(x => x.GetSessionProfileLoginId()).Returns(1000);

			var userLogin = new UserProfileId(webContextMock.Object, loggerMock.Object);
			int id;
			var authenticated = userLogin.IsAuthenticated(out id);

			Assert.That(authenticated, Is.True);
			Assert.That(id, Is.EqualTo(1000));
		}
	}
}
