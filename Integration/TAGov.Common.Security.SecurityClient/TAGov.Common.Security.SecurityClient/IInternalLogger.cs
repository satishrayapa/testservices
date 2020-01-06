using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAGov.Common.Security.SecurityClient
{
	public interface IInternalLogger
	{
		void AppendLog(string text);
	}
}
