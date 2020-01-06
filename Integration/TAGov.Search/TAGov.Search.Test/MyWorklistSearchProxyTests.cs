using NUnit.Framework;

namespace TAGov.Search.Test
{
  [TestFixture]
  public class MyWorklistSearchProxyTests
  {
    [Test]
    public void MyWorklistSearchIsCalled()
    {
      string searchResult = new MyWorklistSearchProxy().Search("foo");
      Assert.That(searchResult, Is.EqualTo("MyWorklistSearch was called with criteria foo"));
    }
  }
}