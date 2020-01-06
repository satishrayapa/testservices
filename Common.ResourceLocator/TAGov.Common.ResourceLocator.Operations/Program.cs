
using TAGov.Common.Operations;

namespace TAGov.Common.ResourceLocator.Operations
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Bootstrap.Execute(args, new Operations());
		}
	}
}
