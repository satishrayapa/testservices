using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.GrmEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class GrmEventRepositoryTests
  {
    private readonly Mock<IHttpClientWrapper> _httpClientWrapperMock;
    private readonly GrmEventRepository _grmEventRepository;

    public GrmEventRepositoryTests()
    {
      _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      var applicationSettingsHelperMock = new Mock<IApplicationSettingsHelper>();
      _grmEventRepository = new GrmEventRepository( applicationSettingsHelperMock.Object, _httpClientWrapperMock.Object );
    }

    [Fact]
    public void SearchShouldThrowRecordNotFoundExceptionWhenNullGrmEventInformationDtosAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Post<IEnumerable<GrmEventInformationDto>>( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GrmEventSearchDto>() ) )
                            .Returns( Task.FromResult( ( IEnumerable<GrmEventInformationDto> ) null ) );
      Should.Throw<RecordNotFoundException>( () => _grmEventRepository.SearchAsync( new GrmEventSearchDto() ) );
    }

    [Fact]
    public void SearchShouldThrowRecordNotFoundExceptionWhenNoGrmEventInformationDtosAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Post<IEnumerable<GrmEventInformationDto>>( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<GrmEventSearchDto>() ) )
                            .Returns( Task.FromResult( Enumerable.Empty<GrmEventInformationDto>() ) );
      Should.Throw<RecordNotFoundException>( () => _grmEventRepository.SearchAsync( new GrmEventSearchDto() ) );
    }
  }
}