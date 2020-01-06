using TAGov.Common.Operations;

namespace TAGov.Services.Facade.AssessmentHeader.Operations
{
  public class Program
  {
    public static void Main( string[] args )
    {
      Bootstrap.Execute( args, new Operations() );
    }
  }
}
