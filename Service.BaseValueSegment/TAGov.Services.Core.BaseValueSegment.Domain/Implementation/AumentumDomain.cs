using System;
using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.BaseValueSegment.Domain.Interfaces;
using TAGov.Services.Core.BaseValueSegment.Domain.Mapping;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;


namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public class AumentumDomain : IAumentumDomain
  {
    private readonly ICaliforniaConsumerPriceIndexRepository _repository;
    private readonly ISysTypeRepository _sysTypeRepository;
    private const int FbyvStartYear = 1977;

    public AumentumDomain( ICaliforniaConsumerPriceIndexRepository repository, ISysTypeRepository sysTypeRepository )
    {
      _repository = repository;
      _sysTypeRepository = sysTypeRepository;
    }

    public IEnumerable<CaliforniaConsumerPriceIndexDto> GetAllCaConsumerPriceIndexes()
    {
      var caConsumerPriceIndexes = _repository.List().ToList();

      if ( !caConsumerPriceIndexes.Any() )
      {
        throw new RecordNotFoundException( "N/A",
                                           typeof( CaliforniaConsumerPriceIndexDto ),
                                           "No California Consumer Price Indexes found in database" );
      }


      return caConsumerPriceIndexes.ToDomain();
    }

    public FactorBaseYearValueDetailDto GetFactoredBasedYearValue( DateTime assessmentDate, int baseYear, decimal baseValue, int assessmentEventType )
    {
      if ( baseYear < FbyvStartYear )
      {
        throw new BadRequestException( string.Format( "BaseYear {0} is invalid.", baseYear ) );
      }

      int finalAssessmentYear = assessmentDate.Year;

      if ( assessmentEventType != _sysTypeRepository.GetSysTypeId( "AsmtEventType", "Annual" ) )
        finalAssessmentYear += 1;

      int year = baseYear + 1;
      decimal fbyv = decimal.Truncate( baseValue );

      while ( year <= finalAssessmentYear )
      {
        var caConsumerPriceIndex = _repository.GetByYear( year );
        if ( caConsumerPriceIndex == null )
        {
          throw new RecordNotFoundException( year.ToString(),
                                             typeof( CaliforniaConsumerPriceIndexDto ),
                                             string.Format( "California Consumer Price Index for year {0} is missing.", year ) );
        }
        fbyv = fbyv * caConsumerPriceIndex.InflationFactor;
        year += 1;
      }

      fbyv = decimal.Truncate( fbyv );

      return new FactorBaseYearValueDetailDto { Fbyv = fbyv, AssessmentYear = finalAssessmentYear };
    }

    public FactorBaseYearValueRequestDto[] GetFactoredBasedYearValue( FactorBaseYearValueRequestDto[] factorBaseYearValueRequestDtos )
    {
      foreach ( var factorBaseYearValueRequestDto in factorBaseYearValueRequestDtos )
      {
        var result = this.GetFactoredBasedYearValue( factorBaseYearValueRequestDto.EventDate,
                                                     factorBaseYearValueRequestDto.AssessmentYear, factorBaseYearValueRequestDto.BaseValue, factorBaseYearValueRequestDto.AssessmentEventType );

        factorBaseYearValueRequestDto.Fbyv = result.Fbyv;
        factorBaseYearValueRequestDto.FbyvYear = result.AssessmentYear;

      }

      return factorBaseYearValueRequestDtos;

    }
  }
}
