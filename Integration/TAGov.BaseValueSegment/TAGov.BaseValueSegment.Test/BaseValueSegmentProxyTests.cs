using System;
using System.IO;
using Moq;
using NUnit.Framework;
using TAGov.Common.ResourceLocatorClient;

namespace TAGov.BaseValueSegment.Test
{
	[TestFixture]
    public class BaseValueSegmentProxyTests
    {
	    [Test]
	    public void SaveSucceeds()
	    {
		    var current = System.Reflection.Assembly.GetExecutingAssembly();
		    string saveResponse;
		    // ReSharper disable once AssignNullToNotNullAttribute
		    using (var streamReader = new StreamReader(current.GetManifestResourceStream("TAGov.BaseValueSegment.Test.SaveResponse.json")))
		    {
			    saveResponse = streamReader.ReadToEnd();
		    }

			var httpClientProxyMock = new Mock<IHttpClientProxy>();
		    var urlServicesMock = new Mock<IUrlServices>();
		    urlServicesMock.Setup(x => x.GetServiceUri(Constants.FacadeBaseValueSegment)).Returns(new Uri("http://foo"));

			httpClientProxyMock.Setup( x => x.Post( It.IsAny<string>(), It.IsAny<string>(),
		                                            It.IsAny<BaseValueSegmentDto>() ) ).Returns( () => saveResponse );


		    var proxy = new BaseValueSegmentProxy( httpClientProxyMock.Object, urlServicesMock.Object );

		    var savedBvs = proxy.Save( 1, 1, new BaseValueSegmentDto() );

			Assert.That(savedBvs.Id, Is.EqualTo(9201));
	    }
	}
}
