using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Common.ResourceLocator.Repository.Implementation;
using TAGov.Common.ResourceLocator.Repository.Models.V1;
using Xunit;

namespace TAGov.Common.ResourceLocator.Repository.Tests
{
	public class RepositoryTests
	{
		private readonly ResourceContext _resourceContext;

		public RepositoryTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

			_resourceContext = new ResourceContext(optionsBuilder);

			TestDataBuilder.Build(_resourceContext);
		}

		[Fact]
		public void Get_WithValidKeyAndPartitionWithOneResourceInDb_GetOneResource()
		{
			var repository = new ResourceRepository(_resourceContext);
			var resource = repository.Get("key1", "part1");

			resource.ShouldNotBe(null);
			resource.Key.ShouldBe("key1");
			resource.Partition.ShouldBe("part1");
		}

		[Fact]
		public void Get_WithValidKeyAndPartitionWithTwoResourcesInDb_GetOneResource()
		{
			var repository = new ResourceRepository(_resourceContext);
			var resource = repository.Get("key3", "part2");

			resource.ShouldNotBe(null);
			resource.Key.ShouldBe("key3");
			resource.Partition.ShouldBe("part2");
		}

		[Fact]
		public void List_WithValidPartitionWithTwoResourcesInDb_GetTwoResources()
		{
			var repository = new ResourceRepository(_resourceContext);
			var resources = repository.List("part2").ToList();

			resources.Count.ShouldBe(2);

			resources[0].Key.ShouldBe("key2");
			resources[1].Key.ShouldBe("key3");
		}

		[Fact]
		public void List_WithValidPartitionWithOneResourceInDb_GetSingleResource()
		{
			var repository = new ResourceRepository(_resourceContext);
			var resources = repository.List("part1").ToList();

			resources.Count.ShouldBe(1);

			resources[0].Key.ShouldBe("key1");
		}

		[Fact]
		public void List_WithInvalidPartition_GetNoResources()
		{
			var repository = new ResourceRepository(_resourceContext);
			var resources = repository.List("part0").ToList();

			resources.Count.ShouldBe(0);
		}

		[Fact]
		public void AddTwoValuesOfSamePartitionGetTwoResourcesBack()
		{
			var resourceRepository = new ResourceRepository(_resourceContext);
			resourceRepository.Create(new Resource { Key = "foo1", Partition = "foo", Value = "true" });
			resourceRepository.Create(new Resource { Key = "foo2", Partition = "foo", Value = "false" });

			var list = resourceRepository.List("foo").ToList();
			list.Count.ShouldBe(2);

			list.Single(x => x.Key == "foo1").Value.ShouldBe("true");
			list.Single(x => x.Key == "foo2").Value.ShouldBe("false");
		}

		[Fact]
		public void UpdateTwoValuesOfSamePartitionGetTwoResourcesBack()
		{
			var resourceRepository = new ResourceRepository(_resourceContext);
			var resource1 = resourceRepository.Get("key1", "part1");
			resource1.Value = "value4";
			resourceRepository.Update(resource1);

			var resource2 = resourceRepository.Get("key2", "part2");
			resource2.Value = "value5";
			resourceRepository.Update(resource2);

			var item1 = resourceRepository.Get("key1", "part1");
			item1.ShouldNotBeNull();
			item1.Value.ShouldBe("value4");

			var item2 = resourceRepository.Get("key2", "part2");
			item2.ShouldNotBeNull();
			item2.Value.ShouldBe("value5");
		}

		[Fact]
		public void DeleteResource_GetNull()
		{
			var resourceRepository = new ResourceRepository(_resourceContext);
			var resource1 = resourceRepository.Get("key1", "part1");
			resourceRepository.Delete(resource1);

			resourceRepository.Get("key1", "part1").ShouldBeNull();
		}
	}
}

