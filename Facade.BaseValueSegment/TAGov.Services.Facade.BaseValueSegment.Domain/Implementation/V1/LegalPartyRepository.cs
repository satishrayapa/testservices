using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class LegalPartyRepository : RepositoryBase, ILegalPartyRepository
  {
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public LegalPartyRepository(
      IApplicationSettingsHelper applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper ) : base( "v1.1", applicationSettingsHelper, httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    public async Task<IEnumerable<LegalPartyDocumentDto>> SearchAsync( LegalPartySearchDto legalPartySearchDto )
    {
      var legalPartyRoleDocumentDtosEnumerable = await _httpClientWrapper.Post<IEnumerable<LegalPartyDocumentDto>>(
                                                   _applicationSettingsHelper.LegalPartyServiceApiUrl, $"{Version}/LegalParties/Documents", legalPartySearchDto );

      if ( legalPartyRoleDocumentDtosEnumerable == null )
        throw new RecordNotFoundException(
          string.Join( ",", legalPartySearchDto.LegalPartyRoleIdList.Select( x => x.ToString() ).ToArray() ),
          typeof( LegalPartyDocumentDto ),
          "The Legal Party Roles Documents could not be found for Legal Party HeaderValueId List" );

      var legalPartyRoleDocumentDtos = legalPartyRoleDocumentDtosEnumerable.ToList();

      if ( legalPartyRoleDocumentDtos.Count == 0 )
        throw new RecordNotFoundException(
          string.Join( ",", legalPartySearchDto.LegalPartyRoleIdList.Select( x => x.ToString() ).ToArray() ),
          typeof( LegalPartyDocumentDto ),
          "The Legal Party Roles Documents could not be found for Legal Party HeaderValueId List" );

      return legalPartyRoleDocumentDtos;
    }

    public async Task<IEnumerable<LegalPartyRoleDto>> GetLegalPartyRoles( int[] legalPartyRoleIds )
    {
      var legalPartyRoles = ( await _httpClientWrapper.Post<IEnumerable<LegalPartyRoleDto>>(
                                _applicationSettingsHelper.LegalPartyServiceApiUrl, $"{Version}/LegalParties/Roles", legalPartyRoleIds ) ).ToList();

      if ( legalPartyRoles == null || legalPartyRoles.Count == 0 )
      {
        throw new RecordNotFoundException(
          string.Join( ",", legalPartyRoles.Select( x => x.ToString() ).ToArray() ), typeof( LegalPartyRoleDto ),
          "The Legal Party Roles not be found for Legal Party Roles Id List" );

      }

      return legalPartyRoles;
    }
  }
}