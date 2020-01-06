using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Common.ResourceLocator.Repository.Models.V1
{
	[Table("Resource", Schema = "Common.Resource")]
	public class Resource
	{
		[Key, Column(Order = 0)]
		[MaxLength(200)]
		public string Key { get; set; }

		[Key, Column(Order = 1)]
		[MaxLength(200)]
		public string Partition { get; set; }

		[MaxLength(1000)]
		public string Value { get; set; }
	}
}
