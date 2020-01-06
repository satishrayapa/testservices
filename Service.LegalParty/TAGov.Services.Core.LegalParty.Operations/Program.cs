using TAGov.Common.Operations;

namespace TAGov.Services.Core.LegalParty.Operations
{
  public class Program
  {
    public static void Main( string[] args )
    {
      Bootstrap.Execute( args, new Operations() );
    }
  }
}
