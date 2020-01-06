using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Common.Security.Repository.Implementation;
using TAGov.Common.Security.Repository.Interfaces;
using Xunit;

namespace TAGov.Common.Security.Repository.Tests
{
	public class ClientRepositoryTests
	{
		private readonly ProxyConfigurationDbContext _dbContext;
		private readonly IClientRepository _clientRepository;
		public ClientRepositoryTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

			_dbContext = new ProxyConfigurationDbContext(optionsBuilder);
			_clientRepository = new ClientRepository(_dbContext);
		}

		[Fact]
		public void AddClient_ShouldBeAbleToGetClientBack()
		{
			_clientRepository.Add(new Client
			{
				ClientId = "foo",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar" } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "res1" } }
			});

			var dbClient = _clientRepository.Get("foo").Result;

			dbClient.ShouldNotBeNull();
			dbClient.ClientId.ShouldBe("foo");
			dbClient.ClientSecrets.Single().Value.ShouldBe("bar");
			dbClient.AllowedScopes.Single().Scope.ShouldBe("res1");
		}

		[Fact]
		public void AddTwoClients_ListShouldGetTwoClientsBack()
		{
			_clientRepository.Add(new Client
			{
				ClientId = "foo1",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar1" } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "res1" } }
			});

			_clientRepository.Add(new Client
			{
				ClientId = "foo2",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar2" } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "res2" } }
			});

			var dbClients = _clientRepository.List().Result.ToList();

			dbClients.Count.ShouldBe(2);

			dbClients[0].ClientId.ShouldBe("foo1");
			dbClients[0].ClientSecrets.Single().Value.ShouldBe("bar1");
			dbClients[0].AllowedScopes.Single().Scope.ShouldBe("res1");

			dbClients[1].ClientId.ShouldBe("foo2");
			dbClients[1].ClientSecrets.Single().Value.ShouldBe("bar2");
			dbClients[1].AllowedScopes.Single().Scope.ShouldBe("res2");
		}

		[Fact]
		public void UpdateClient_ShouldBeAbleToGetClientBack()
		{
			var client1 = new Client
			{
				ClientId = "foo1",
				ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "bar1" } },
				AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "res1" } }
			};

			_dbContext.Clients.Add(client1);

			_dbContext.SaveChanges();

			client1.ClientSecrets[0].Value = "bar2";

			_clientRepository.Update(client1);

			var dbClient = _clientRepository.Get("foo1").Result;
			dbClient.ClientSecrets.Single().Value.ShouldBe("bar2");
		}
	}
}
