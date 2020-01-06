using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Security.Repository.Interfaces;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Implementation
{
  public class AppFunctionForQueryRepository :IAppFunctionForQueryRepository
  {
    private readonly AumentumSecurityQueryContext _aumentumSecurityQueryContext;

    public AppFunctionForQueryRepository( AumentumSecurityQueryContext aumentumSecurityQueryContext )
    {
      _aumentumSecurityQueryContext = aumentumSecurityQueryContext;
    }

    public IEnumerable<AppFunctionForQuery> GetAllFieldAppFunctionsForApplicationLevelAppFunctions(IEnumerable<int> applicationLevelAppFunctionIds)
    {
      return
        (from af in _aumentumSecurityQueryContext.Permissions
         where applicationLevelAppFunctionIds.Contains(af.ParentId)
         select af).ToList();
    }
  }
}