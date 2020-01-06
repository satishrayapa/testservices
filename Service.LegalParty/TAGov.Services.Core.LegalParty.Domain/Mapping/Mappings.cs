using System.Collections.Generic;
using AutoMapper;

namespace TAGov.Services.Core.LegalParty.Domain.Mapping
{
  public static class Mappings
  {
    static Mappings()
    {
      Mapper.Initialize( configuration =>
                         {
                           configuration.CreateMap<Repository.Models.V1.LegalParty, Models.V1.LegalPartyDto>();
                           configuration.CreateMap<Repository.Models.V1.LegalPartyRole, Models.V1.LegalPartyRoleDto>();
                           configuration.CreateMap<Repository.Models.V1.LegalPartyDocument, Models.V1.LegalPartyDocumentDto>();
                           configuration.CreateMap<Models.V1.Enums.EffectiveStatuses, Repository.Models.V1.Enums.EffectiveStatuses>();
                         } );
    }

    /// <summary>
    /// This is used in Startup so the static constructor can be invoked.
    /// </summary>
    public static void Init()
    {
      // Do nothing.
    }

    public static Models.V1.LegalPartyDto ToDomain( this Repository.Models.V1.LegalParty legalParty )
    {
      return Mapper.Map<Models.V1.LegalPartyDto>( legalParty );
    }

    public static Models.V1.LegalPartyRoleDto ToDomain( this Repository.Models.V1.LegalPartyRole legalPartyRole )
    {
      return Mapper.Map<Models.V1.LegalPartyRoleDto>( legalPartyRole );
    }

    public static Models.V1.LegalPartyDocumentDto ToDomain( this Repository.Models.V1.LegalPartyDocument legalPartyGrmEvent )
    {
      return Mapper.Map<Models.V1.LegalPartyDocumentDto>( legalPartyGrmEvent );
    }

    public static IEnumerable<Models.V1.LegalPartyDocumentDto> ToDomain( this IEnumerable<Repository.Models.V1.LegalPartyDocument> legalPartyGrmEvent )
    {
      return Mapper.Map<IEnumerable<Models.V1.LegalPartyDocumentDto>>( legalPartyGrmEvent );
    }

    public static Repository.Models.V1.Enums.EffectiveStatuses ToRepository( this Models.V1.Enums.EffectiveStatuses value )
    {
      return Mapper.Map<Repository.Models.V1.Enums.EffectiveStatuses>( value );
    }

  }
}
