using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Common;
using TAGov.Common.Http;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{
  public class AssessmentEventRepository : RepositoryBase, IAssessmentEventRepository
  {
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public AssessmentEventRepository(
      IApplicationSettingsHelper applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper ) : base( "v1.1", applicationSettingsHelper, httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    public async Task<IEnumerable<RevenueObjectBasedAssessmentEventDto>> ListAsync( int assessmentEventId )
    {
      assessmentEventId.ThrowBadRequestExceptionIfInvalid( "AssessmentEventId" );

      return await _httpClientWrapper.Get<List<RevenueObjectBasedAssessmentEventDto>>(
               _applicationSettingsHelper.AssessmentEventServiceApiUrl,
               $"{Version}/AssessmentEvents/{assessmentEventId}/RevenueObjectBasedAssessmentEvents" );
    }

    public async Task<AssessmentEventDto> Get( int assessmentEventId )
    {
      assessmentEventId.ThrowBadRequestExceptionIfInvalid( "AssessmentEventId" );

      return await _httpClientWrapper.Get<AssessmentEventDto>( _applicationSettingsHelper.AssessmentEventServiceApiUrl, $"{Version}/AssessmentEvents/{assessmentEventId}" );
    }

    public async Task<IEnumerable<AssessmentEventDto>> ListAsync( int revenueObjectId, DateTime eventDate )
    {
      revenueObjectId.ThrowBadRequestExceptionIfInvalid( "RevenueObjectId" );

      return await _httpClientWrapper.Get<List<AssessmentEventDto>>( _applicationSettingsHelper.AssessmentEventServiceApiUrl,
                                                                     $"{Version}/AssessmentEvents/RevenueObjectId/{revenueObjectId}/EventDate/{eventDate:yyyy-MM-dd}" );
    }
  }
}
