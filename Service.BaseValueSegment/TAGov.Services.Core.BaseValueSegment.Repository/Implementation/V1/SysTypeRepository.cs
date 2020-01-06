using System;
using System.Linq;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1
{
  public class SysTypeRepository : ISysTypeRepository
  {
    private readonly AumentumContext _aumentumContext;

    public SysTypeRepository( AumentumContext aumentumContext )
    {
      _aumentumContext = aumentumContext;
    }

    public int GetSysTypeId( string sysTypeCategory, string sysTypeShortDescription )
    {
      return ( from stc in _aumentumContext.SysTypeCats
               join st in _aumentumContext.SystemTypes on stc.Id equals st.SysTypeCatId
               where stc.ShortDescr == sysTypeCategory &&
                     stc.BegEffDate == ( from sub in _aumentumContext.SysTypeCats
                                         where sub.Id == stc.Id
                                         select sub ).Max(new Func<Models.V1.SysTypeCat, DateTime?>(subSysTypeCat => subSysTypeCat.BegEffDate)) &&
                     st.ShortDescr == sysTypeShortDescription &&
                     st.BeginEffectiveDate == ( from sub in _aumentumContext.SystemTypes
                                                where sub.Id == st.Id
                                                select sub ).Max(new Func<Models.V1.SystemType, DateTime?>(subSysType => subSysType.BeginEffectiveDate))
               select st.Id ).Single();
    }
  }
}
