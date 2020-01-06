namespace TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1
{
  public interface IStatutoryReferenceRepository
  {
    Models.V1.StatutoryReference GetStatutoryReferenceByAssessmentTransactionId( int assessmentTransactionId );
  }
}
