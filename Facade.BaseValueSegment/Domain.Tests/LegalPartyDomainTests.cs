using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;
using Xunit;

namespace Domain.Tests
{
  public class LegalPartyDomainTests
  {
    private readonly Mock<ILegalPartyRepository> _legalPartyRepositoryMock;
    private readonly LegalPartyDomain _legalPartyDomain;

    public LegalPartyDomainTests()
    {
      _legalPartyRepositoryMock = new Mock<ILegalPartyRepository>();
      _legalPartyDomain = new LegalPartyDomain( _legalPartyRepositoryMock.Object );
    }

    [Fact]
    public void VerifySearchDtoIsPopulated()
    {
      const int legalPartyRoleId = 100;
      const string documentNumber = "foo";

      _legalPartyRepositoryMock.Setup( x => x.SearchAsync( It.Is<LegalPartySearchDto>( y => y.LegalPartyRoleIdList.Count == 1 && y.LegalPartyRoleIdList[ 0 ] == legalPartyRoleId ) ) )
                               .Returns( Task.FromResult( new List<LegalPartyDocumentDto>
                                                          {
                                                            new LegalPartyDocumentDto { DocNumber = documentNumber }
                                                          }.AsEnumerable() ) );


      var result = _legalPartyDomain.GetLegalPartyRoleDocuments( new BaseValueSegmentDto
                                                                 {
                                                                   BaseValueSegmentTransactions = new List<BaseValueSegmentTransactionDto>
                                                                                                  {
                                                                                                    new BaseValueSegmentTransactionDto
                                                                                                    {
                                                                                                      BaseValueSegmentOwners = new List<BaseValueSegmentOwnerDto>
                                                                                                                               {
                                                                                                                                 new BaseValueSegmentOwnerDto
                                                                                                                                 {
                                                                                                                                   LegalPartyRoleId = legalPartyRoleId
                                                                                                                                 }
                                                                                                                               }
                                                                                                    }
                                                                                                  }
                                                                 } ).Result.ToList();

      result.Count.ShouldBe( 1 );
      result[ 0 ].DocNumber.ShouldBe( documentNumber );
    }
  }
}