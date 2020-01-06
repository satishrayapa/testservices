using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Mapping
{
  public static class Mappings
  {
    static Mappings()
    {
      Mapper.Initialize( configuration =>
                         {
                           configuration.AddConditionalObjectMapper().Where( ( src, dest ) => src.Name == dest.Name + "Dto" );
                           configuration.AddConditionalObjectMapper().Where( ( src, dest ) => src.Name == dest.Name.Replace( "Dto", "" ) );
                           configuration.CreateMap<Repository.Models.V1.BaseValueSegment, BaseValueSegmentInfoDto>();

                           configuration.CreateMap<BaseValueSegmentOwnerDto, BaseValueSegmentOwner>()
                                        .ForMember( m => m.IsUserOverride, opts => opts.MapFrom( src => src.IsOverride ) );

                           configuration.CreateMap<BaseValueSegmentValueDto, BaseValueSegmentValue>()
                                        .ForMember( m => m.IsUserOverride, opts => opts.MapFrom( src => src.IsOverride ) );

                           configuration.CreateMap<BaseValueSegmentOwner, BaseValueSegmentOwnerDto>()
                                        .ForMember( m => m.IsOverride, opts => opts.MapFrom( src => src.IsUserOverride ) );

                           configuration.CreateMap<BaseValueSegmentValue, BaseValueSegmentValueDto>()
                                        .ForMember( m => m.IsOverride, opts => opts.MapFrom( src => src.IsUserOverride ) );


                           configuration.CreateMap<BaseValueSegmentFlag, BaseValueSegmentFlagDto>();

                           configuration.CreateMap<BaseValueSegmentDto, Repository.Models.V1.BaseValueSegment>()
                                        .ForMember( m => m.DynCalcStepTrackingId, opts => opts.Ignore() );

                           configuration.CreateMap<BaseValueSegmentTransactionDto, BaseValueSegmentTransaction>()
                                        .ForMember( m => m.BaseValueSegment, opts => opts.Ignore() )
                                        .ForMember( m => m.BaseValueSegmentTransactionValues, opts => opts.Ignore() )
                                        .ForMember( m => m.BaseValueSegmentTransactionType, opts => opts.Ignore() );

                           configuration.CreateMap<BaseValueSegmentOwnerValueDto, BaseValueSegmentOwnerValue>()
                                        .ForMember( m => m.Owner, opts => opts.Ignore() )
                                        .ForMember( m => m.Header, opts => opts.Ignore() );

                           configuration.CreateMap<BaseValueSegmentValueHeaderDto, BaseValueSegmentValueHeader>()
                                        .ForMember( m => m.DynCalcStepTrackingId, opts => opts.Ignore() )
                                        .ForMember( m => m.BaseValueSegmentTransaction, opts => opts.Ignore() );

                           configuration.CreateMap<AssessmentRevisionBaseValueSegmentDto, AssessmentRevisionBaseValueSegment>()
                                        .ForMember( m => m.BaseValueSegment, opts => opts.Ignore() );

                           configuration.CreateMap<Repository.Models.V1.BaseValueSegment, BaseValueSegmentDto>()
                                        .ForMember( m => m.AssessmentEventTransactionId, opts => opts.Ignore() );

                           configuration.CreateMap<BaseValueSegmentTransactionTypeDto, BaseValueSegmentTransactionType>()
                                        .ForMember( m => m.Id, opts => opts.Ignore() )
                                        .ForMember( m => m.BaseValueSegmentTransactions, opts => opts.Ignore() );

                           configuration.CreateMap<BaseValueSegmentStatusTypeDto, BaseValueSegmentStatusType>()
                                        .ForMember(m => m.Id, opts => opts.Ignore())
                                        .ForMember(m => m.AssessmentRevisionBaseValueSegments, opts => opts.Ignore());
                         });
    }

    /// <summary>
    /// This is used in Startup so the static constructor can be invoked.
    /// </summary>
    public static void Init()
    {
      // Do nothing.
    }

    public static BaseValueSegmentDto ToDomain( this Repository.Models.V1.BaseValueSegment baseValueSegment )
    {
      return Mapper.Map<BaseValueSegmentDto>( baseValueSegment );
    }

    public static BaseValueSegmentInfoDto ToInfoDomain( this Repository.Models.V1.BaseValueSegment baseValueSegment )
    {
      return Mapper.Map<BaseValueSegmentInfoDto>( baseValueSegment );
    }

    public static BaseValueSegmentTransaction ToEntity( this BaseValueSegmentTransactionDto baseValueSegmentTransactionDto )
    {
      return Mapper.Map<BaseValueSegmentTransaction>( baseValueSegmentTransactionDto );
    }

    public static BaseValueSegmentTransactionDto ToDomain( this BaseValueSegmentTransaction bvsTransaction )
    {
      return Mapper.Map<BaseValueSegmentTransactionDto>( bvsTransaction );
    }

    public static BaseValueSegmentEventDto ToDomain( this BaseValueSegmentEvent bvsEvent )
    {
      return Mapper.Map<BaseValueSegmentEventDto>( bvsEvent );
    }

    public static IList<SubComponentDetailDto> ToDomain( this IEnumerable<SubComponentDetail> subComponentDetails )
    {
      return Mapper.Map<IList<SubComponentDetailDto>>( subComponentDetails );
    }

    public static CaliforniaConsumerPriceIndexDto ToDomain( this CaliforniaConsumerPriceIndex caConsumerPriceIndex )
    {
      return Mapper.Map<CaliforniaConsumerPriceIndexDto>( caConsumerPriceIndex );
    }

    public static IEnumerable<CaliforniaConsumerPriceIndexDto> ToDomain( this IEnumerable<CaliforniaConsumerPriceIndex> caConsumerPriceIndices )
    {
      return Mapper.Map<IList<CaliforniaConsumerPriceIndexDto>>( caConsumerPriceIndices );
    }

    public static IList<BeneficialInterestEventDto> ToDomain( this IEnumerable<BeneficialInterestEvent> beneficialInterests )
    {
      return Mapper.Map<IList<BeneficialInterestEventDto>>( beneficialInterests );
    }

    public static IList<BaseValueSegmentConclusionDto> ToDomain( this IEnumerable<BaseValueSegmentConclusion> baseValueSegmentConclusions )
    {
      return Mapper.Map<IList<BaseValueSegmentConclusionDto>>( baseValueSegmentConclusions );
    }

    public static Repository.Models.V1.BaseValueSegment ToEntity( this BaseValueSegmentDto baseValueSegment )
    {
      return Mapper.Map<Repository.Models.V1.BaseValueSegment>( baseValueSegment );
    }

    public static IList<BaseValueSegmentHistoryDto> ToDomain( this IEnumerable<BaseValueSegmentHistory> baseValueSegmentHistory )
    {
      return Mapper.Map<IList<BaseValueSegmentHistoryDto>>( baseValueSegmentHistory );
    }

    public static BaseValueSegmentFlagDto ToDomain( this BaseValueSegmentFlag baseValueSegmentFlag )
    {
      return Mapper.Map<BaseValueSegmentFlagDto>( baseValueSegmentFlag );
    }
  }
}

