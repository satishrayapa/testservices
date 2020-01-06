using System.Threading.Tasks;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1
{
  public interface IAssessmentHeaderDomain
  {
    Task<Models.V1.AssessmentHeader> Get( int assessmentEventId );
  }
}
