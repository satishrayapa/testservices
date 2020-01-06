using System.Linq;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository
{
  public static class QueryExtensions
  {
    public static IQueryable<SystemType> GetSystemTypeShortDescription( this IQueryable<SystemType> sysTypes, int sysTypeId )
    {

      return from st in sysTypes
             where st.Id == sysTypeId &&
                   st.begEffDate == ( from sub in sysTypes
                                      where sub.Id == st.Id
                                      select sub.begEffDate ).DefaultIfEmpty().Max()
             select st;

    }
  }
}
