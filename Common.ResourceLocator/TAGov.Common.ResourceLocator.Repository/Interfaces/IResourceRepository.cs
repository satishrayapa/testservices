using System.Collections.Generic;
using TAGov.Common.ResourceLocator.Repository.Models.V1;

namespace TAGov.Common.ResourceLocator.Repository.Interfaces
{
	public interface IResourceRepository
	{
		Resource Get(string key, string partition);

		IEnumerable<Resource> List(string parition);

		void Create(Resource resource);

		void Update(Resource resource);
		void Delete(Resource resource);
	}
}
