using AutoMapper;

namespace TAGov.Common.ResourceLocator.Domain.Mapping
{
	public static class Mappings
	{
		static Mappings()
		{
			Mapper.Initialize(configuration =>
			{
				configuration.CreateMap<Repository.Models.V1.Resource, Models.V1.ResourceDto>();
				configuration.CreateMap<Models.V1.ResourceDto, Repository.Models.V1.Resource>();
			});
		}

		public static Models.V1.ResourceDto ToDomain(this Repository.Models.V1.Resource resource)
		{
			return Mapper.Map<Models.V1.ResourceDto>(resource);
		}

		public static Repository.Models.V1.Resource ToEntity(this Models.V1.ResourceDto resourceDto)
		{
			return Mapper.Map<Repository.Models.V1.Resource>(resourceDto);
		}
	}
}
