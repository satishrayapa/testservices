using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Implementation.V1
{

  public class BaseValueSegmentRepository : RepositoryBase, IBaseValueSegmentRepository
  {
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public BaseValueSegmentRepository(
      IApplicationSettingsHelper applicationSettingsHelper,
      IHttpClientWrapper httpClientWrapper ) : base( "v1.1", applicationSettingsHelper, httpClientWrapper )
    {
      _applicationSettingsHelper = applicationSettingsHelper;
      _httpClientWrapper = httpClientWrapper;
    }

    private string Url => _applicationSettingsHelper.BaseValueSegmentServiceApiUrl;

    public async Task<BaseValueSegmentDto> GetAsync( int baseValueSegmentId )
    {
      return await _httpClientWrapper.Get<BaseValueSegmentDto>( Url, $"{Version}/BaseValueSegments/{baseValueSegmentId}" );
    }

    public async Task<IEnumerable<BaseValueSegmentEventDto>> GetEventsAsync( int revenueObjectId )
    {
      var baseValueSegmentEventDtosEnumerable = await _httpClientWrapper.Get<IEnumerable<BaseValueSegmentEventDto>>( Url,
                                                                                                                     $"{Version}/BaseValueSegmentEvents/RevenueObjectId/{revenueObjectId}" );

      if ( baseValueSegmentEventDtosEnumerable == null )
        throw new RecordNotFoundException( revenueObjectId.ToString(), typeof( BaseValueSegmentEventDto ),
                                           string.Format( "No BaseValueSegmentEvents are associated with RevenueObjectId: {0}.", revenueObjectId ) );

      var baseValueSegmentEventDtos = baseValueSegmentEventDtosEnumerable.ToList();

      if ( baseValueSegmentEventDtos.Count == 0 )
        throw new RecordNotFoundException( revenueObjectId.ToString(), typeof( BaseValueSegmentEventDto ),
                                           string.Format( "No BaseValueSegmentEvents are associated with RevenueObjectId: {0}.", revenueObjectId ) );

      return baseValueSegmentEventDtos;
    }

    public async Task<FactorBaseYearValueDetailDto> GetFactorBaseYearValueDetail( DateTime asOf, int baseYear, decimal amount, int assessmentEventType )
    {
      return await _httpClientWrapper.Get<FactorBaseYearValueDetailDto>( Url,
                                                                         $"{Version}/CAConsumerPriceIndexes/AssessmentDate/{asOf:yyyy-MM-dd}/BaseYear/{baseYear}/BaseValue/{amount}/AssessmentEventType/{assessmentEventType}" );
    }

    public async Task<IEnumerable<SubComponentDetailDto>> GetSubComponentDetails( int revenueObjectId, DateTime asOf )
    {
      return await _httpClientWrapper.Get<IList<SubComponentDetailDto>>( Url,
                                                                         $"{Version}/SubComponents/RevenueObjectId/{revenueObjectId}/AsOfDate/{asOf:yyyy-MM-dd}" );
    }

    public async Task<IEnumerable<BeneficialInterestEventDto>> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOf )
    {
      return await _httpClientWrapper.Get<IList<BeneficialInterestEventDto>>( Url,
                                                                              $"{Version}/Owners/RevenueObjectId/{revenueObjectId}/AsOfDate/{asOf:yyyy-MM-dd}" );
    }

    public async Task<BaseValueSegmentInfoDto> GetAsync( int revenueObjectId, DateTime asOf, int sequenceNumber )
    {
      return await _httpClientWrapper.Get<BaseValueSegmentInfoDto>( Url,
                                                                    $"{Version}/BaseValueSegments/RevenueObjectId/{revenueObjectId}/AsOf/{asOf:yyyy-MM-dd}/SequenceNumber/{sequenceNumber}" );
    }

    public async Task<BaseValueSegmentDto> CreateAsync( BaseValueSegmentDto baseValueSegmentDto )
    {
      return await _httpClientWrapper.Post<BaseValueSegmentDto>( Url, $"{Version}/BaseValueSegments", baseValueSegmentDto );
    }

    public async Task CreateTransactionAsync( BaseValueSegmentTransactionDto baseValueSegmentTransactionDto )
    {
      await _httpClientWrapper.Post<BaseValueSegmentTransactionDto>( Url, $"{Version}/BaseValueSegments/Transactions", baseValueSegmentTransactionDto );
    }

    public async Task<IEnumerable<BaseValueSegmentInfoDto>> GetListAsync( int revenueObjectId, DateTime asOf )
    {
      return await _httpClientWrapper.Get<List<BaseValueSegmentInfoDto>>( Url,
                                                                          $"{Version}/BaseValueSegments/RevenueObjectId/{revenueObjectId}/AsOf/{asOf:yyyy-MM-dd}" );
    }

    public async Task<IEnumerable<BaseValueSegmentInfoDto>> GetListAsync( int revenueObjectId )
    {
      return await _httpClientWrapper.Get<List<BaseValueSegmentInfoDto>>( Url,
                                                                          $"{Version}/BaseValueSegments/RevenueObjectId/{revenueObjectId}" );
    }

    public async Task<IEnumerable<BaseValueSegmentConclusionDto>> GetConclusionsData( int revenueObjectId, DateTime effectiveDate )
    {
      try
      {
        return await _httpClientWrapper.Get<List<BaseValueSegmentConclusionDto>>( Url,
                                                                                  $"{Version}/BaseValueSegmentConclusions/RevenueObjectId/{revenueObjectId}/EffectiveDate/{effectiveDate:yyyy-MM-dd}" );
      }
      catch ( NotFoundException )
      {
        return Enumerable.Empty<BaseValueSegmentConclusionDto>();
      }
    }

    public async Task<IEnumerable<BaseValueSegmentHistoryDto>> GetBaseValueSegmentHistory( int revenueObjectId, DateTime fromDate, DateTime toDate )
    {
      return await _httpClientWrapper.Get<List<BaseValueSegmentHistoryDto>>( Url,
                                                                             $"{Version}/BaseValueSegmentHistory/RevenueObjectId/{revenueObjectId}/FromDate/{fromDate:yyyy-MM-dd}/ToDate/{toDate:yyyy-MM-dd}" );
    }
  }
}