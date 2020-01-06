using TAGov.Common.ResourceLocator.Repository.Models.V1;

namespace TAGov.Common.ResourceLocator.Repository.Tests
{
	public static class TestDataBuilder
	{
		public static void Build(ResourceContext resourceContext)
		{
			resourceContext.Resources.Add(new Resource { Key = "key1", Partition = "part1", Value = "value1" });
			resourceContext.Resources.Add(new Resource { Key = "key2", Partition = "part2", Value = "value2" });
			resourceContext.Resources.Add(new Resource { Key = "key3", Partition = "part2", Value = "value3" });

			resourceContext.SaveChanges();
		}
	}
}
