using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class LegalPartyRepositoryTests
  {
    private readonly Mock<IHttpClientWrapper> _httpClientWrapperMock;
    private readonly LegalPartyRepository _legalPartyRepository;

    public LegalPartyRepositoryTests()
    {
      _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
      var applicationSettingsHelperMock = new Mock<IApplicationSettingsHelper>();
      _legalPartyRepository = new LegalPartyRepository( applicationSettingsHelperMock.Object, _httpClientWrapperMock.Object );
    }

    [Fact]
    public void ShouldThrowRecordNotFoundExceptionWhenNullLegalPartyDocumentDtosAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Get<IEnumerable<LegalPartyDocumentDto>>( It.IsAny<string>(), It.IsAny<string>() ) )
                            .Returns( Task.FromResult( ( IEnumerable<LegalPartyDocumentDto> ) null ) );
      Should.Throw<RecordNotFoundException>( () => _legalPartyRepository.SearchAsync( new LegalPartySearchDto() ) );
    }

    [Fact]
    public void ShouldThrowRecordNotFoundExceptionWhenNoLegalPartyDocumentDtosAreReturned()
    {
      _httpClientWrapperMock.Setup( x => x.Get<IEnumerable<LegalPartyDocumentDto>>( It.IsAny<string>(), It.IsAny<string>() ) )
                            .Returns( Task.FromResult( Enumerable.Empty<LegalPartyDocumentDto>() ) );
      Should.Throw<RecordNotFoundException>( () => _legalPartyRepository.SearchAsync( new LegalPartySearchDto() ) );
    }
  }
}