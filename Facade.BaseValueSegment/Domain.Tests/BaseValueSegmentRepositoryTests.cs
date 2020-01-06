using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class BaseValueSegmentRepositoryTests
  {
    private readonly Mock<IHttpClientWrapper> _httpClientWrapperMock;
    private readonly BaseValueSegmentRepository _baseValueSegmentRepository;

    public BaseValueSegmentRepositoryTests()
    {
      _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      var applicationSettingsHelperMock = new Mock<IApplicationSettingsHelper>();
      _baseValueSegmentRepository = new BaseValueSegmentRepository( applicationSettingsHelperMock.Object, _httpClientWrapperMock.Object );
    }

    [Fact]
    public void GetEventsAsyncShouldThrowRecordNotFoundExceptionWhenNullBaseValueSegmentEventsAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Get<IEnumerable<BaseValueSegmentEventDto>>( It.IsAny<string>(), It.IsAny<string>() ) )
                            .Returns( Task.FromResult( ( IEnumerable<BaseValueSegmentEventDto> ) null ) );
      Should.Throw<RecordNotFoundException>( () => _baseValueSegmentRepository.GetEventsAsync( -1 ) );
    }

    [Fact]
    public void GetEventsAsyncShouldThrowRecordNotFoundExceptionWhenNoBaseValueSegmentEventsAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Get<IEnumerable<BaseValueSegmentEventDto>>( It.IsAny<string>(), It.IsAny<string>() ) )
                            .Returns( Task.FromResult( Enumerable.Empty<BaseValueSegmentEventDto>() ) );
      Should.Throw<RecordNotFoundException>( () => _baseValueSegmentRepository.GetEventsAsync( -1 ) );
    }
  }
}