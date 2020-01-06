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
	public class ApiResourceRepositoryTests
	{
		private readonly ProxyConfigurationDbContext _dbContext;
		private readonly IApiResourceRepository _repository;
		public ApiResourceRepositoryTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

			_dbContext = new ProxyConfigurationDbContext(optionsBuilder);
			_repository = new ApiResourceRepository(_dbContext);
		}

		[Fact]
		public void AddApiResource_ShouldBeAbleToGetApiResourceBack()
		{
			_repository.Add(new ApiResource
			{
				Name = "foo",
				Description = "bar",
				Scopes = new List<ApiScope> { new ApiScope { Name = "res1" } }
			});

			var apiRes = _repository.Get("foo").Result;
			apiRes.ShouldNotBeNull();
			apiRes.Name.ShouldBe("foo");
			apiRes.Description.ShouldBe("bar");
			apiRes.Scopes.Single().Name.ShouldBe("res1");
		}
	}
}
