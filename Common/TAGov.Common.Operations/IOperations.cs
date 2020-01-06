using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;

namespace TAGov.Common.Operations
{
	public interface IOperations
	{
		IEnumerable<string> GetEfMigrations(IConfiguration configuration);
		void ApplyEfMigrations(IConfiguration configuration);
		int Apply(IConfiguration configuration);
		void AddOption(CommandLineApplication commandLineApplication);
	}
}
