namespace TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1
{
  public interface ISysTypeRepository
  {
    int GetSysTypeId( string sysTypeCategory, string sysTypeShortDescription );
  }
}