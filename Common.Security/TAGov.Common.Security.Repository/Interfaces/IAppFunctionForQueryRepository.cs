using System.Collections.Generic;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Interfaces
{
  public interface IAppFunctionForQueryRepository
  {
    IEnumerable<AppFunctionForQuery> GetAllFieldAppFunctionsForApplicationLevelAppFunctions(IEnumerable<int> applicationLevelAppFunctionIds);
  }
}