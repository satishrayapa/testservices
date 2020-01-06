using AutoMapper;
using AutoMapper.Mappers;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;



namespace TAGov.Services.Core.LegalPartySearch.Domain.Mappings
{
	public static class Mappings
	{
		static Mappings()
		{
			Mapper.Initialize(configuration =>
			{
				configuration.AddConditionalObjectMapper().Where((src, dest) => src.Name == dest.Name + "Dto");
				configuration.AddConditionalObjectMapper().Where((src, dest) => src.Name == dest.Name.Replace("Dto", ""));

				configuration.CreateMap<SearchLegalPartyQueryDto, SearchLegalPartyQuery>()
					.ForMember(x => x.ExcludeSearchAll, o => o.Ignore());

				configuration.CreateMap<SearchLegalParty, SearchLegalPartyDto>()
					.ForMember(x => x.IsActive, o => o.Ignore());
			});
		}

		/// <summary>
		/// This is used in Startup so the static constructor can be invoked.
		/// </summary>
		public static void Init()
		{
			// Do nothing.
		}

		public static SearchLegalPartyDto ToDomain(this SearchLegalParty searchLegalParty)
		{
			var dto = Mapper.Map<SearchLegalPartyDto>(searchLegalParty);
			dto.DisplayName = dto.DisplayName.Trim();

			if (!string.IsNullOrEmpty(dto.Address))
				dto.Address = dto.Address.Trim();

			if (!string.IsNullOrEmpty(dto.LegalPartyRole))
				dto.LegalPartyRole = dto.LegalPartyRole.Trim();

			if (!string.IsNullOrEmpty(dto.GeoCode))
				dto.GeoCode = dto.GeoCode.Trim();

			if (!string.IsNullOrEmpty(dto.Tag))
				dto.Tag = dto.Tag.Trim();

			if (!string.IsNullOrEmpty(dto.LegalPartyType))
				dto.LegalPartyType = dto.LegalPartyType.Trim();

			if (!string.IsNullOrEmpty(dto.LegalPartySubType))
				dto.LegalPartySubType = dto.LegalPartySubType.Trim();

			if (!string.IsNullOrEmpty(dto.StreetType))
				dto.StreetType = dto.StreetType.Trim();

			dto.IsActive = searchLegalParty.EffectiveStatus == "A";
			return dto;
		}

		public static SearchLegalPartyQuery ToExclusions(this SearchLegalPartyQueryDto searchLegalPartyQueryDto)
		{
			return Mapper.Map(searchLegalPartyQueryDto, new SearchLegalPartyQuery());
		}
	}
}
