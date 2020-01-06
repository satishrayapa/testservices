using System;

using TAGov.Common.Exceptions;
using TAGov.Services.Core.RevenueObject.Domain.Interfaces;
using TAGov.Services.Core.RevenueObject.Domain.Mapping;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;

namespace TAGov.Services.Core.RevenueObject.Domain.Implementation
{
  public class RevenueObjectDomain : IRevenueObjectDomain
  {
    private readonly IRevenueObjectRepository _revenueObjectRepository;
    private readonly IMarketAndRestrictedValueRepository _marketAndRestrictedRespository;

    public RevenueObjectDomain( IRevenueObjectRepository revenueObjectRepository, IMarketAndRestrictedValueRepository marketAndRestrictedRepository )
    {
      _revenueObjectRepository = revenueObjectRepository;
      _marketAndRestrictedRespository = marketAndRestrictedRepository;
    }

    public RevenueObjectDto Get( int id, DateTime effectiveDate )
    {
      if ( id < 1 ) throw new BadRequestException( string.Format( "Id {0} is invalid.", id ) );

      var revenueObjectRepoModel = _revenueObjectRepository.Get( id, effectiveDate );

      if ( revenueObjectRepoModel == null ) throw new RecordNotFoundException( id.ToString(), typeof( Repository.Models.V1.RevenueObject ), string.Format( "Id {0} is missing.", id ) );

      var domainModel = revenueObjectRepoModel.ToDomain();

      var marketAndRestrictedValues = _marketAndRestrictedRespository.Get( id, effectiveDate );

      var marketAndRestrictedValueModel = marketAndRestrictedValues.ToDomain();

      domainModel.MarketAndRestrictedValues = marketAndRestrictedValueModel;

      return domainModel;
    }

    public TAGDto GetTAGByRevenueObjectId( int id, DateTime effectiveDate )
    {
      if ( id < 1 ) throw new BadRequestException( string.Format( "Id {0} is invalid.", id ) );

      var tagRepoModel = _revenueObjectRepository.GetTAGByRevenueObjectId( id, effectiveDate );

      if ( tagRepoModel == null ) throw new RecordNotFoundException( id.ToString(), typeof( Repository.Models.V1.TAG ), string.Format( "Id {0} is missing.", id ) );

      return tagRepoModel.ToDomain();
    }

    public RevenueObjectDto GetRevenueObjectByPin( string pin )
    {
      if ( string.IsNullOrEmpty( pin ) ) throw new BadRequestException( string.Format( "Pin {0} is invalid.", pin ) );

      var revenueObjectRepoModel = _revenueObjectRepository.GetRevenueObjectByPin( pin );

      if ( revenueObjectRepoModel == null ) throw new RecordNotFoundException( pin, typeof( Repository.Models.V1.RevenueObject ), string.Format( "Pin {0} is invalid.", pin ) );

      return revenueObjectRepoModel.ToDomain();
    }

    public RevenueObjectDto GetRevenueObjectSitusAddressByPin( string pin )
    {
      if ( string.IsNullOrEmpty( pin ) ) throw new BadRequestException( string.Format( "Pin {0} is invalid.", pin ) );

      var revenueObjectRepoModel = _revenueObjectRepository.GetRevenueObjectSitusAddressByPin( pin );

      if ( revenueObjectRepoModel == null ) throw new RecordNotFoundException( pin, typeof( Repository.Models.V1.RevenueObject ), string.Format( "Pin {0} is invalid.", pin ) );

      return revenueObjectRepoModel.ToDomain();
    }
  }
}
