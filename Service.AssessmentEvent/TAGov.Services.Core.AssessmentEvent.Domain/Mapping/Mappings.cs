using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;

namespace TAGov.Services.Core.AssessmentEvent.Domain.Mapping
{
  public static class Mappings
  {
    static Mappings()
    {
      Mapper.Initialize(configuration =>
      {
        configuration.AddConditionalObjectMapper().Where((src, dest) => src.Name == dest.Name + "Dto");
        configuration.AddConditionalObjectMapper().Where((src, dest) => src.Name == dest.Name.Replace("Dto", ""));

        //This is necessary because there are members on AssessmentEventDto that do not come from the repository directly via automapping
        configuration.CreateMap<Repository.Models.V1.AssessmentEvent, Models.V1.AssessmentEventDto>(MemberList.None);
      });
    }

    /// <summary>
    /// This is used in Startup so the static constructor can be invoked.
    /// </summary>
    public static void Init()
    {
      // Do nothing.
    }

    public static Models.V1.RevenueObjectBasedAssessmentEventDto ToDomain(this Repository.Models.V1.RevenueObjectBasedAssessmentEvent revenueObjectBasedAssessmentEvent)
    {
      return Mapper.Map<Models.V1.RevenueObjectBasedAssessmentEventDto>(revenueObjectBasedAssessmentEvent);
    }

    public static Models.V1.AssessmentEventDto ToDomain(this Repository.Models.V1.AssessmentEvent assesmentEvent)
    {
      return Mapper.Map<Models.V1.AssessmentEventDto>(assesmentEvent);
    }

    public static Models.V1.AssessmentEventTransactionDto ToDomain(this Repository.Models.V1.AssessmentEventTransaction assesmentEventTransaction)
    {
      return Mapper.Map<Models.V1.AssessmentEventTransactionDto>(assesmentEventTransaction);
    }

    public static IList<Models.V1.AssessmentEventTransactionDto> ToDomain(this IList<Repository.Models.V1.AssessmentEventTransaction> assessmentEventTransactions)
    {
      var domains = Mapper.Map<IList<Repository.Models.V1.AssessmentEventTransaction>, IList<Models.V1.AssessmentEventTransactionDto>>(assessmentEventTransactions);

      return domains;
    }

    public static Models.V1.AssessmentRevisionDto ToDomain(this Repository.Models.V1.AssessmentRevision assessmentRevision)
    {
      return Mapper.Map<Models.V1.AssessmentRevisionDto>(assessmentRevision);
    }

    public static Models.V1.StatutoryReferenceDto ToDomain(this Repository.Models.V1.StatutoryReference statutoryReference)
    {
      return Mapper.Map(statutoryReference, new Models.V1.StatutoryReferenceDto());
    }

    public static Models.V1.AssessmentEventIsEditableFlagDto ToDomain(this Repository.Models.V1.AssessmentEventIsEditableFlag assessmentEventIsEditableFlag)
    {
      return Mapper.Map(assessmentEventIsEditableFlag, new Models.V1.AssessmentEventIsEditableFlagDto());
    }

    public static Models.V1.AssessmentEventEffectiveDateDto ToDomain(this Repository.Models.V1.AssessmentEventEffectiveDate assessmentEventEffectiveDate)
    {
      return Mapper.Map(assessmentEventEffectiveDate, new Models.V1.AssessmentEventEffectiveDateDto());
    }
  }
}
