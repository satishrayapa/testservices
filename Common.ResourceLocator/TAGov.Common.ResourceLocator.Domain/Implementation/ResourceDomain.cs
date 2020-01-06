using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Exceptions;
using TAGov.Common.ResourceLocator.Domain.Interfaces;
using TAGov.Common.ResourceLocator.Domain.Mapping;
using TAGov.Common.ResourceLocator.Domain.Models.V1;
using TAGov.Common.ResourceLocator.Repository.Interfaces;

namespace TAGov.Common.ResourceLocator.Domain.Implementation
{
	public class ResourceDomain : IResourceDomain
	{
		private readonly IResourceRepository _resourceRepository;

		public ResourceDomain(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		public IEnumerable<ResourceDto> List(string partition)
		{
			var resources = _resourceRepository.List(partition).Select(x => x.ToDomain()).ToList();

			if (resources.Count == 0)
				throw new NotFoundException($"Resources with partition: {partition} are not found.");

			return resources;
		}

		public void Update(ResourceDto resourceDto)
		{
			if (resourceDto == null)
				throw new BadRequestException("Resource cannot be null.");

			var existing = _resourceRepository.Get(resourceDto.Key, resourceDto.Partition);

			if (existing == null)
				throw new NotFoundException($"Resource with partition: {resourceDto.Partition} and key: {resourceDto.Key} is not found.");

			existing.Value = resourceDto.Value;

			_resourceRepository.Update(existing);
		}

		public void Create(ResourceDto resourceDto)
		{
			if (resourceDto == null)
				throw new BadRequestException("Resource cannot be null.");

			_resourceRepository.Create(resourceDto.ToEntity());
		}

		public ResourceDto Get(string partition, string key)
		{
			var dto = _resourceRepository.Get(key, partition);

			dto.ThrowRecordNotFoundExceptionIfNull(new IdInfo("partition", partition), new IdInfo("key", key));
			return dto.ToDomain();
		}

		public void UpdateList(IEnumerable<ResourceDto> resourceDtos)
		{
			if (resourceDtos == null)
				throw new BadRequestException("Resources cannot be null.");

			var newList = resourceDtos.ToList();
			if (newList.Count > 0)
			{
				var partition = newList.First().Partition;

				// Ensure partition key is the same for all.
				newList.ToList().ForEach(x =>
				{
					if (x == null)
						throw new BadRequestException("Invalid resource item");

					if (x.Partition != partition)
						throw new BadRequestException("Partition key in one or more of the list is invalid.");
				});

				var originalList = _resourceRepository.List(partition).Select(x => x.ToDomain()).ToList();
				var tocreateList = newList.Except(originalList).ToList();
				var toDeleteList = originalList.Except(newList).ToList();
				var toUpdateList = newList.Intersect(originalList).ToList();

				toDeleteList.ForEach(x =>
				{
					var existing = _resourceRepository.Get(x.Key, partition);
					if (existing != null)
					{
						_resourceRepository.Delete(existing);
					}
				});
				tocreateList.ForEach(Create);
				toUpdateList.ForEach(Update);
			}
		}
	}
}
