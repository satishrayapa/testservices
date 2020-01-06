using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;

namespace TAGov.Services.Core.GrmEvent.Domain.Mapping
{
  public static class Mappers
  {
    static Mappers()
    {
      Mapper.Initialize( configuration =>
                         {
                           configuration.AddConditionalObjectMapper().Where( ( src, dest ) => src.Name == dest.Name + "Dto" );
                           configuration.AddConditionalObjectMapper().Where( ( src, dest ) => src.Name == dest.Name.Replace( "Dto", "" ) );
                         } );
    }

    /// <summary>
    /// This is used in Startup so the static constructor can be invoked.
    /// </summary>
    public static void Init()
    {
      // Do nothing.
    }

    public static Models.V1.GrmEventDto ToDomain( this Repository.Models.V1.GrmEvent grmEvent )
    {
      return Mapper.Map<Models.V1.GrmEventDto>( grmEvent );
    }

    public static Models.V1.SubComponentValueDto ToDomain( this Repository.Models.V1.SubComponentValue subComponentValue )
    {
      return Mapper.Map<Models.V1.SubComponentValueDto>( subComponentValue );
    }

    public static IEnumerable<Models.V1.GrmEventDto> ToDomain( this IEnumerable<Repository.Models.V1.GrmEvent> grmEvent )
    {
      return Mapper.Map<IEnumerable<Models.V1.GrmEventDto>>( grmEvent );
    }

    public static Models.V1.GrmEventInformationDto ToDomain( this Repository.Models.V1.GrmEventInformation grmEventInformation )
    {
      return Mapper.Map<Models.V1.GrmEventInformationDto>( grmEventInformation );
    }

    public static IEnumerable<Models.V1.GrmEventInformationDto> ToDomain( this IEnumerable<Repository.Models.V1.GrmEventInformation> grmEventInformation )
    {
      return Mapper.Map<IEnumerable<Models.V1.GrmEventInformationDto>>( grmEventInformation );
    }

    public static IEnumerable<Models.V1.GrmEventCreateDto> ToDomain( this IEnumerable<Repository.Models.V1.GrmEventCreate> grmEventCreate )
    {
      return Mapper.Map<IEnumerable<Models.V1.GrmEventCreateDto>>( grmEventCreate );
    }

    public static IEnumerable<Repository.Models.V1.GrmEventComponentCreate> ToEntity( this IEnumerable<Models.V1.GrmEventComponentCreateDto> grmEventComponentCreate )
    {
      return Mapper.Map<IEnumerable<Repository.Models.V1.GrmEventComponentCreate>>( grmEventComponentCreate );
    }

    public static IEnumerable<Repository.Models.V1.GrmEventCreate> ToEntity( this IEnumerable<Models.V1.GrmEventCreateDto> grmEventCreate )
    {
      return Mapper.Map<IEnumerable<Repository.Models.V1.GrmEventCreate>>( grmEventCreate );
    }

  }
}
