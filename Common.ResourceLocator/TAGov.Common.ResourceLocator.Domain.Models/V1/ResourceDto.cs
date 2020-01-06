namespace TAGov.Common.ResourceLocator.Domain.Models.V1
{
	public class ResourceDto
	{
		public string Key { get; set; }

		public string Partition { get; set; }

		public string Value { get; set; }

		public override bool Equals(object obj)
		{
			var compare = (ResourceDto)obj;
			return compare.Partition == Partition && compare.Key == Key;
		}

		public override int GetHashCode()
		{
			return unchecked(Key.GetHashCode() + Partition.GetHashCode());
		}
	}
}
