using System.Collections.Generic;
using TAGov.Common.ResourceLocator.Domain.Models.V1;

namespace TAGov.Common.ResourceLocator.Domain.Interfaces
{
	public interface IResourceDomain
	{
		IEnumerable<ResourceDto> List(string partition);
		void Update(ResourceDto resourceDto);
		void Create(ResourceDto resourceDto);
		ResourceDto Get(string partition, string key);

		void UpdateList(IEnumerable<ResourceDto> resourceDtos);
	}
}
