using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Common.Security.Domain.Implementation;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Domain.Models.V1;
using TAGov.Common.Security.Repository.Interfaces;
using Xunit;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;

namespace Domain.Tests
{
	public class ClientDomainTests
	{
		private readonly IClientDomain _clientDomain;
		private readonly Mock<IApiResourceRepository> _apiResourceRepositoryMock;
		private readonly Mock<IClientRepository> _clientRepositoryMock;

		public ClientDomainTests()
		{
			_apiResourceRepositoryMock = new Mock<IApiResourceRepository>();
			_clientRepositoryMock = new Mock<IClientRepository>();

			_clientDomain = new ClientDomain(
				_apiResourceRepositoryMock.Object,
				_clientRepositoryMock.Object);
		}

		[Fact]
		public void GetClientNotFound_ShouldGetNotFoundException()
		{
			Should.ThrowAsync<NotFoundException>(() => _clientDomain.Get("foobar"));
		}

		[Fact]
		public void GetClientFound_ShouldGetClientDto()
		{
			_clientRepositoryMock.Setup(x => x.Get("foobar")).ReturnsAsync(new Client
			{
				ClientId = "foobar",
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "a" } },
				AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ClientCredentials } }
			});

			var dto = _clientDomain.Get("foobar").Result;

			dto.ShouldNotBeNull();
			dto.Username.ShouldBe("foobar");
		}

		[Fact]
		public void ListClientsFound_ShouldGetClientDtos()
		{
			_clientRepositoryMock.Setup(x => x.List()).ReturnsAsync(
				new List<Client>
				{
					new Client { ClientId = "foobar1",
						AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "a" } },
						AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ClientCredentials } } },
					new Client { ClientId = "foobar2",
						AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "b" } },
						AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ClientCredentials } } },
					new Client { ClientId = "foobar3",
						AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "c" } },
						AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ResourceOwnerPassword } } },
					new Client { ClientId = "foobar4",
						AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "d" } },
						AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ResourceOwnerPassword } } }
				});

			var dtos = _clientDomain.List().Result.ToList();

			dtos[0].Username.ShouldBe("foobar1");
			dtos[0].ClientType.ShouldBe(ClientTypes.ServiceToService);
			dtos[0].AllowedScopes.Single().Name.ShouldBe("a");
			dtos[1].Username.ShouldBe("foobar2");
			dtos[1].ClientType.ShouldBe(ClientTypes.ServiceToService);
			dtos[1].AllowedScopes.Single().Name.ShouldBe("b");
			dtos[2].Username.ShouldBe("foobar3");
			dtos[2].ClientType.ShouldBe(ClientTypes.ApplicationService);
			dtos[2].AllowedScopes.Single().Name.ShouldBe("c");
			dtos[3].Username.ShouldBe("foobar4");
			dtos[3].ClientType.ShouldBe(ClientTypes.ApplicationService);
			dtos[3].AllowedScopes.Single().Name.ShouldBe("d");
		}

		[Fact]
		public void AddClientWithoutUsername_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
			_clientDomain.Create(new ClientDto
			{
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } }
			}));
		}

		[Fact]
		public void AddClientWithoutPassword_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Create(new ClientDto
				{
					Username = "foobar",
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } }
				}));
		}

		[Fact]
		public void AddClientWithoutAllowedScopes_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Create(new ClientDto
				{
					Password = "foobar",
					Username = "foobar",
					ClientType = ClientTypes.ServiceToService
				}));
		}

		[Fact]
		public void UpdateClientWithoutUsername_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Password = "foobar",
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
					ClientType = ClientTypes.ServiceToService
				}));
		}

		[Fact]
		public void UpdateClientWithoutPassword_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Username = "foobar",
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
					ClientType = ClientTypes.ServiceToService
				}));
		}

		[Fact]
		public void UpdateClientWithoutAllowedScopes_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Password = "foobar",
					Username = "foobar",
					ClientType = ClientTypes.ServiceToService
				}));
		}

		[Fact]
		public void UpdateClientWithoutEmptyStringScopes_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Password = "foobar",
					Username = "foobar",
					ClientType = ClientTypes.ServiceToService,
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Description = "", Name = "" } }
				}));
		}

		[Fact]
		public void UpdateClientWithoutEmptyDescriptionScope_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Password = "foobar",
					Username = "foobar",
					ClientType = ClientTypes.ServiceToService,
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Description = "", Name = "aaa" } }
				}));
		}

		[Fact]
		public void UpdateClientWithoutEmptyNameScope_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(new ClientDto
				{
					Password = "foobar",
					Username = "foobar",
					ClientType = ClientTypes.ServiceToService,
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Description = "aaaa", Name = "" } }
				}));
		}

		[Fact]
		public void UpdateNullClient_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Update(null));
		}

		[Fact]
		public void AddClientWithAllExistingResourceApis_NoResourceApiAdded()
		{
			_apiResourceRepositoryMock.Setup(x => x.Get("f")).ReturnsAsync(new ApiResource { Name = "f" });

			_clientDomain.Create(new ClientDto
			{
				Username = "foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } }
			}).Wait();

			_apiResourceRepositoryMock.Verify(x => x.Add(It.IsAny<ApiResource>()), Times.Never);
		}

		[Fact]
		public void AddClientWithNotExistingResourceApis_ResourceApiAdded()
		{
			_clientDomain.Create(new ClientDto
			{
				Username = "service.foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
				ClientType = ClientTypes.ServiceToService
			}).Wait();

			_apiResourceRepositoryMock.Verify(x => x.Add(It.IsAny<ApiResource>()), Times.Once);
		}

		[Fact]
		public void AddClientWithClientTypeAsServiceToServiceButUsernameDoesNotStartWithService_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
			_clientDomain.Create(new ClientDto
			{
				Username = "foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
				ClientType = ClientTypes.ServiceToService
			}));
		}

		[Fact]
		public void AddClientWithClientTypeAsApplicationServiceButUsernameStartWithService_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<BadRequestException>(() =>
				_clientDomain.Create(new ClientDto
				{
					Username = "service.foobar",
					Password = "foobar",
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
					ClientType = ClientTypes.ApplicationService
				}));
		}

		private void SetupFooBarClientInRepository()
		{
			_clientRepositoryMock.Setup(x => x.Get("foobar")).ReturnsAsync(new Client
			{
				ClientId = "foobar",
				AllowedScopes = new List<ClientScope> { new ClientScope() },
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "f" } },
				AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ResourceOwnerPassword } }
			});
		}

		[Fact]
		public void UpdateClientWithNotExistingResourceApis_ResourceApiAdded()
		{
			SetupFooBarClientInRepository();

			_clientDomain.Update(new ClientDto
			{
				Username = "foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
				ClientType = ClientTypes.ApplicationService
			}).Wait();

			_apiResourceRepositoryMock.Verify(x => x.Add(It.IsAny<ApiResource>()), Times.Once);
		}

		[Fact]
		public void UpdateClientWithAllExistingResourceApis_NoResourceApiAdded()
		{
			SetupFooBarClientInRepository();

			_apiResourceRepositoryMock.Setup(x => x.Get("f")).ReturnsAsync(new ApiResource { Name = "f" });

			_clientDomain.Update(new ClientDto
			{
				Username = "foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
				ClientType = ClientTypes.ApplicationService
			}).Wait();

			_apiResourceRepositoryMock.Verify(x => x.Add(It.IsAny<ApiResource>()), Times.Never);
			_clientRepositoryMock.Verify(x => x.Update(It.Is<Client>(c => c.ClientId == "foobar")));
		}

		[Fact]
		public void UpdateClientPassword_PasswordIsUpdated()
		{
			_clientRepositoryMock.Setup(x => x.Get("foobar")).ReturnsAsync(new Client
			{
				ClientId = "foobar",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar1".Sha256() } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "a" } },
				AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ResourceOwnerPassword } }
			});

			_clientDomain.Update(new ClientDto
			{
				Username = "foobar",
				Password = "bar2",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
				ClientType = ClientTypes.ApplicationService

			}).Wait();

			_clientRepositoryMock.Verify(x => x.Update(It.Is<Client>(c => c.ClientId == "foobar" && c.ClientSecrets.Single().Value == "bar2".Sha256())));
		}

		[Fact]
		public void UpdateClientNotInSystem_ShouldGetBadRequestException()
		{
			Should.ThrowAsync<RecordNotFoundException>(() => _clientDomain.Update(new ClientDto
			{
				Username = "foobar",
				Password = "foobar",
				AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } }
			}));
		}

		[Fact]
		public void UpdateClientGrantType_ShouldGetBadRequestException()
		{
			_clientRepositoryMock.Setup(x => x.Get("foobar")).ReturnsAsync(new Client
			{
				ClientId = "foobar",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar1".Sha256() } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "a" } },
				AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.ResourceOwnerPassword } }
			});

			Should.ThrowAsync<RecordNotFoundException>(async () =>
			{
				await _clientDomain.Update(new ClientDto
				{
					Username = "foobar",
					Password = "bar2",
					AllowedScopes = new List<ApiResourceDto> { new ApiResourceDto { Name = "f", Description = "d" } },
					ClientType = ClientTypes.ServiceToService
				});
			});
		}
	}
}
