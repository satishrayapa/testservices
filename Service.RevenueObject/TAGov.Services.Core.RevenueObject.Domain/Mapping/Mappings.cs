using System.Collections.Generic;
using AutoMapper;

namespace TAGov.Services.Core.RevenueObject.Domain.Mapping
{
  public static class Mappings
  {
    static Mappings()
    {
      Mapper.Initialize( configuration =>
                         {
                           configuration.CreateMap<Repository.Models.V1.RevenueObject, Models.V1.RevenueObjectDto>();
                           configuration.CreateMap<Repository.Models.V1.TAG, Models.V1.TAGDto>();
                           configuration.CreateMap<Repository.Models.V1.MarketAndRestrictedValue, Models.V1.MarketAndRestrictedValueDto>();
                           configuration.CreateMap<Repository.Models.V1.SitusAddress, Models.V1.SitusAddressDto>();
                         } );
    }

    /// <summary>
    /// This is used in Startup so the static constructor can be invoked.
    /// </summary>
    public static void Init()
    {
      // Do nothing.
    }

    public static Models.V1.RevenueObjectDto ToDomain( this Repository.Models.V1.RevenueObject revenueObject )
    {
      return Mapper.Map<Models.V1.RevenueObjectDto>( revenueObject );
    }

    public static Models.V1.TAGDto ToDomain( this Repository.Models.V1.TAG tag )
    {
      return Mapper.Map<Models.V1.TAGDto>( tag );
    }

    public static IList<Models.V1.MarketAndRestrictedValueDto> ToDomain( this IList<Repository.Models.V1.MarketAndRestrictedValue> marketAndRestrictedValue )
    {
      return Mapper.Map<IList<Models.V1.MarketAndRestrictedValueDto>>( marketAndRestrictedValue );
    }

    public static Models.V1.SitusAddressDto ToDomain( this Repository.Models.V1.SitusAddress situsAddress )
    {
      return Mapper.Map<Models.V1.SitusAddressDto>( situsAddress );
    }
  }
}
