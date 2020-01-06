using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Domain.Models.V1;
using TAGov.Common.Security.Repository.Interfaces;
using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace TAGov.Common.Security.Domain.Implementation
{
	public class ClientDomain : IClientDomain
	{
		private readonly IApiResourceRepository _apiResourceRepository;
		private readonly IClientRepository _clientRepository;

		public ClientDomain(
			IApiResourceRepository apiResourceRepository,
			IClientRepository clientRepository)
		{
			_apiResourceRepository = apiResourceRepository;
			_clientRepository = clientRepository;
		}

		public async Task<IEnumerable<ClientInfoDto>> List()
		{
			return (await _clientRepository.List()).Select(x => x.ToInfoDto()).ToList();
		}

		private async Task AddApiResourceIfMissing(List<ApiResourceDto> apiResourceDtos)
		{
			foreach (var apiResourceDto in apiResourceDtos)
			{
				var apiRes = await _apiResourceRepository.Get(apiResourceDto.Name);
				if (apiRes == null)
				{
					await _apiResourceRepository.Add(apiResourceDto.ToEntity());
				}
			}
		}

		public async Task Update(ClientDto clientDto)
		{
			ValidateClientDto(clientDto);

			var existing = await _clientRepository.Get(clientDto.Username);
			if (existing == null)
				throw new RecordNotFoundException(clientDto.Username, typeof(ClientDto), "Client with username is not found.");

			if (existing.AllowedGrantTypes.Single().GrantType != GetGrantType(clientDto))
				throw new BadRequestException("You cannot change the grant-type once a client has been created.");

			await AddApiResourceIfMissing(clientDto.AllowedScopes);

			existing.ClientSecrets.Single().Value = clientDto.Password.Sha256();
			existing.AllowedScopes = clientDto.AllowedScopes.Select(x => new ClientScope { Scope = x.Name }).ToList();

			await _clientRepository.Update(existing);
		}

		// ReSharper disable once UnusedParameter.Local
		private void ValidateClientDto(ClientDto clientDto)
		{
			if (clientDto == null)
			{
				throw new BadRequestException("Unable to parse ClientDto or you have sent in a null.");
			}

			if (clientDto.AllowedScopes == null || clientDto.AllowedScopes.Count == 0 ||
				clientDto.AllowedScopes.Any(x => string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Description)))
			{
				throw new BadRequestException("You must have at least a single scope for access.");
			}

			if (string.IsNullOrEmpty(clientDto.Username))
			{
				throw new BadRequestException("Username is a required field.");
			}

			if (string.IsNullOrEmpty(clientDto.Password))
			{
				throw new BadRequestException("Password is a required field.");
			}

			if (clientDto.ClientType == ClientTypes.ServiceToService && !clientDto.Username.StartsWith("service."))
			{
				throw new BadRequestException("By convention, all ServiceToService clients are required to start with the word 'service.'.");
			}

			if (clientDto.ClientType == ClientTypes.ApplicationService && clientDto.Username.StartsWith("service."))
			{
				throw new BadRequestException("By convention, all ApplicationService clients cannot start with the word 'service.'.");
			}
		}

		public async Task Create(ClientDto clientDto)
		{
			ValidateClientDto(clientDto);

			await AddApiResourceIfMissing(clientDto.AllowedScopes);

			var grantType = GetGrantType(clientDto);

			var client = new Client
			{
				ClientId = clientDto.Username,
				AllowedGrantTypes = new List<ClientGrantType>
				{
					new ClientGrantType {GrantType = grantType}
				},
				ClientSecrets = new List<ClientSecret>
				{
					new ClientSecret {Value = clientDto.Password.Sha256()}
				},
				AllowedScopes = clientDto.AllowedScopes.Select(x => new ClientScope { Scope = x.Name }).ToList()
			};

			await _clientRepository.Add(client);
		}

		private string GetGrantType(ClientDto clientDto)
		{
			if (clientDto.ClientType == ClientTypes.ServiceToService)
				return GrantType.ClientCredentials;

			if (clientDto.ClientType == ClientTypes.ApplicationService)
				return GrantType.ResourceOwnerPassword;

			throw new BadRequestException("Invalid/Unknown grant type.");
		}

		public async Task<ClientInfoDto> Get(string username)
		{
			var client = await _clientRepository.Get(username);

			if (client == null)
				throw new NotFoundException($"Client with username: {username} cannot be found.");

			return client.ToInfoDto();
		}
	}
}
