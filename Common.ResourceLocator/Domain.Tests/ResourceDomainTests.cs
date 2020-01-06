using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Common.ResourceLocator.Domain.Implementation;
using TAGov.Common.ResourceLocator.Domain.Models.V1;
using TAGov.Common.ResourceLocator.Repository.Interfaces;
using TAGov.Common.ResourceLocator.Repository.Models.V1;
using Xunit;

namespace TAGov.Common.ResourceLocator.Domain.Tests
{
	public class GrmEventDomainTests
	{
		[Fact]
		public void ListItems_AreMapped()
		{
			var repo = new Mock<IResourceRepository>();
			repo.Setup(x => x.List("part1"))
				.Returns(new List<Resource> { new Resource { Key = "key1", Partition = "part1", Value = "value1" } });
			var domain = new ResourceDomain(repo.Object);

			var items = domain.List("part1").ToList();

			items.Count.ShouldBe(1);
			items[0].Key = "key1";
			items[0].Partition = "part1";
			items[0].Value = "value1";
		}

		[Fact]
		public void ListItemsNotFound_ThrowsNotFoundException()
		{
			var repo = new Mock<IResourceRepository>();
			repo.Setup(x => x.List("part2")).Returns(new List<Resource>());
			var domain = new ResourceDomain(repo.Object);

			Should.Throw<NotFoundException>(() => domain.List("part2"));
		}

		[Fact]
		public void UpdateWithNullDto_ThrowsBadRequestException()
		{
			var domain = new ResourceDomain(null);
			Should.Throw<BadRequestException>(() => domain.Update(null));
		}

		[Fact]
		public void UpdateWithMissingDto_NotFoundException()
		{
			var repo = new Mock<IResourceRepository>();
			repo.Setup(x => x.Get("foo", "foo")).Returns((Resource)null);
			var domain = new ResourceDomain(repo.Object);
			Should.Throw<NotFoundException>(() => domain.Update(new ResourceDto { Key = "foo", Partition = "foo" }));
		}

		[Fact]
		public void UpdateWithExistingDto_ValueIsUpdated()
		{
			var repo = new Mock<IResourceRepository>();
			repo.Setup(x => x.Get("foo", "foo")).Returns(new Resource { Key = "foo", Partition = "foo", Value = "foo" });
			var domain = new ResourceDomain(repo.Object);
			domain.Update(new ResourceDto { Key = "foo", Partition = "foo", Value = "bar" });

			repo.Verify(x => x.Update(It.Is<Resource>(y => y.Key == "foo" && y.Partition == "foo" && y.Value == "bar")));
		}

		[Fact]
		public void CreateWithNullDto_ThrowsBadRequestException()
		{
			var domain = new ResourceDomain(null);
			Should.Throw<BadRequestException>(() => domain.Create(null));
		}

		[Fact]
		public void CreateWithNewDto_DtoIsMapped()
		{
			var repo = new Mock<IResourceRepository>();

			var domain = new ResourceDomain(repo.Object);
			domain.Create(new ResourceDto { Key = "foo1", Partition = "foo1", Value = "bar1" });

			repo.Verify(x => x.Create(It.Is<Resource>(y => y.Key == "foo1" && y.Partition == "foo1" && y.Value == "bar1")));
		}

		[Fact]
		public void AddNullItems_ThrowsBadRequestException()
		{
			var repo = new Mock<IResourceRepository>();

			var domain = new ResourceDomain(repo.Object);

			Should.Throw<BadRequestException>(() => domain.UpdateList(null));
		}

		[Fact]
		public void AddItemsOfDifferentPartitions_ThrowsBadRequestException()
		{
			var repo = new Mock<IResourceRepository>();

			var domain = new ResourceDomain(repo.Object);

			var list = new List<ResourceDto>
			{
				new ResourceDto {Partition = "X", Key = "A", Value = "foo"},
				new ResourceDto {Partition = "Y", Key = "A", Value = "bar"}
			};

			Should.Throw<BadRequestException>(() => domain.UpdateList(list));
		}

		[Fact]
		public void AddANullItem_ThrowsBadRequestException()
		{
			const string partition = "part1";

			var repo = new Mock<IResourceRepository>();

			var domain = new ResourceDomain(repo.Object);

			var list = new List<ResourceDto>
			{
				new ResourceDto {Partition = partition, Key = "a", Value = "bar1"},
				new ResourceDto {Partition = partition, Key = "b", Value = "bar2"},
				null,
				new ResourceDto {Partition = partition, Key = "e", Value = "bar4"},
			};

			Should.Throw<BadRequestException>(() => domain.UpdateList(list));
		}

		[Fact]
		public void AddUpdateAndDeleteItems_ItemsAreAddedUpdatedAndDeleted()
		{
			const string partition = "part1";
			var dbList = new List<Resource>();
			var res1 = new Resource { Partition = partition, Key = "a", Value = "foo1" };
			var res2 = new Resource { Partition = partition, Key = "b", Value = "foo2" };
			var res3 = new Resource { Partition = partition, Key = "c", Value = "foo3" };
			dbList.Add(res1);
			dbList.Add(res2);
			dbList.Add(res3);

			var repo = new Mock<IResourceRepository>();
			repo.Setup(x => x.List(partition)).Returns(dbList.AsEnumerable());
			repo.Setup(x => x.Get(res1.Key, res1.Partition)).Returns(res1);
			repo.Setup(x => x.Get(res2.Key, res2.Partition)).Returns(res2);
			repo.Setup(x => x.Get(res3.Key, res3.Partition)).Returns(res3);

			var domain = new ResourceDomain(repo.Object);

			var list = new List<ResourceDto>
			{
				new ResourceDto {Partition = partition, Key = "a", Value = "bar1"},
				new ResourceDto {Partition = partition, Key = "b", Value = "bar2"},
				new ResourceDto {Partition = partition, Key = "d", Value = "bar3"},
				new ResourceDto {Partition = partition, Key = "e", Value = "bar4"},
			};

			domain.UpdateList(list);

			repo.Verify(x => x.Create(It.Is<Resource>(y =>
				y.Key == "d" && y.Partition == partition && y.Value == "bar3" ||
				y.Key == "e" && y.Partition == partition && y.Value == "bar4")), Times.Exactly(2));
			repo.Verify(x => x.Update(It.Is<Resource>(y =>
				y.Key == "a" && y.Partition == partition && y.Value == "bar1" ||
				y.Key == "b" && y.Partition == partition && y.Value == "bar2")), Times.Exactly(2));
			repo.Verify(x => x.Delete(It.Is<Resource>(y =>
				y.Key == "c" && y.Partition == partition)), Times.Exactly(1));
		}

		[Fact]
		public void GetResourceDto()
		{
			const string partition = "part1";
			var repo = new Mock<IResourceRepository>();
			var res1 = new Resource { Partition = partition, Key = "a", Value = "foo1" };
			repo.Setup(x => x.Get(res1.Key, res1.Partition)).Returns(res1);

			var domain = new ResourceDomain(repo.Object);

			var dto = domain.Get("part1", "a");
			dto.ShouldNotBeNull();
			dto.Partition.ShouldBe(partition);
			dto.Key.ShouldBe("a");
			dto.Value.ShouldBe("foo1");
		}
	}
}
