using System.Collections.Generic;
using System.Linq;
using TAGov.Common.ResourceLocator.Repository.Interfaces;
using TAGov.Common.ResourceLocator.Repository.Models.V1;

namespace TAGov.Common.ResourceLocator.Repository.Implementation
{
	public class ResourceRepository : IResourceRepository
	{
		private readonly ResourceContext _resourceContext;

		public ResourceRepository(ResourceContext resourceContext)
		{
			_resourceContext = resourceContext;
		}

		/// <summary>
		/// This is to illustrate the composite key elements of how to get a unique resource.
		/// </summary>
		/// <param name="key">key.</param>
		/// <param name="partition">partition.</param>
		/// <returns>Resource.</returns>
		public Resource Get(string key, string partition)
		{
			return _resourceContext.Resources.SingleOrDefault(x => x.Key == key && x.Partition == partition);
		}

		public IEnumerable<Resource> List(string parition)
		{
			return _resourceContext.Resources.Where(x => x.Partition == parition).ToList();
		}

		public void Create(Resource resource)
		{
			_resourceContext.Resources.Add(resource);
			_resourceContext.SaveChanges();
		}

		public void Update(Resource resource)
		{		
			_resourceContext.SaveChanges();
		}

		public void Delete(Resource resource)
		{
			_resourceContext.Remove(resource);
			_resourceContext.SaveChanges();
		}
	}
}
