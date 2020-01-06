using System.Linq;
using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using TAGov.Common.Security.Domain.Models.V1;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace TAGov.Common.Security.Domain
{
    public static class Mappings
    {
        private static IMapper _mapper;
        static Mappings()
        {
            var config = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<Client, ClientInfoDto>()
                    .ForMember(x => x.Username, m => m.MapFrom(src => src.ClientId))
                    .ForMember(x => x.AllowedScopes, m => m.MapFrom(src => src.AllowedScopes))
                    .AfterMap((src, dest) =>
                    {
                        if (src.AllowedGrantTypes.SingleOrDefault(x => x.GrantType == GrantType.ClientCredentials) != null)
                        {
                            dest.ClientType = ClientTypes.ServiceToService;
                        }

                        if (src.AllowedGrantTypes.SingleOrDefault(x => x.GrantType == GrantType.ResourceOwnerPassword) != null)
                        {
                            dest.ClientType = ClientTypes.ApplicationService;
                        }
                    });



                configuration.CreateMap<ClientScope, ClientScopeDto>()
                    .ForMember(x => x.Name, m => m.MapFrom(src => src.Scope));

                configuration.CreateMap<ApiResourceDto, ApiResource>();
            });

            _mapper = config.CreateMapper();
        }

        public static ClientInfoDto ToInfoDto(this Client client)
        {
            return _mapper.Map(client, new ClientInfoDto());
        }

        public static ApiResource ToEntity(this ApiResourceDto apiResourceDto)
        {
            return _mapper.Map(apiResourceDto, new ApiResource());
        }
    }
}
