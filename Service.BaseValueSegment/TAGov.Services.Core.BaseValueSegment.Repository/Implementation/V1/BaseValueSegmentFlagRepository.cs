using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.Constants;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1
{
  public class BaseValueSegmentFlagRepository : IBaseValueSegmentFlagRepository
  {
    private readonly AumentumContext _aumentumContext;

    public BaseValueSegmentFlagRepository( AumentumContext aumentumContext )
    {
      _aumentumContext = aumentumContext;
    }

    public async Task<IEnumerable<BaseValueSegmentFlag>> ListAsync( int revenueObjectId )
    {
      return await (from flagRole in _aumentumContext.FlagRoles
                    join flagHeader in _aumentumContext.FlagHeaders on flagRole.FlagHeaderId equals flagHeader.Id
                    join sysType in _aumentumContext.SystemTypes on flagHeader.FlagHeaderTypeId equals sysType.Id
                    where
                      flagRole.ObjectId == revenueObjectId &&
                      flagRole.ObjectType == SystemTypes.RevenueObject &&
                      flagRole.EffectiveStatus == EffectiveStatuses.Active &&
                      flagHeader.EffectiveStatus == EffectiveStatuses.Active &&
                      flagRole.Status == EffectiveStatuses.Active &&
                      flagRole.StartDate <= DateTime.Now &&
                      flagRole.TerminationDate >= DateTime.Now &&

                      flagRole.BeginEffectiveDate ==
                      (from maxEffectiveDateFlagRole in _aumentumContext.FlagRoles
                       where maxEffectiveDateFlagRole.Id == flagRole.Id &&
                     maxEffectiveDateFlagRole.BeginEffectiveDate <= DateTime.Now
                       //If no rows are returned then there is nothing to max and we should throw
                       //but Max has to handle this possibility otherwise the LINQ engine won't
                       //convert Max to SQL
                       //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                       select maxEffectiveDateFlagRole).Max(new Func<FlagRole, DateTime?>(selectedFlagRole => selectedFlagRole.BeginEffectiveDate)) &&

                      flagHeader.BeginEffectiveDate ==
                      (from maxEffectiveDateFlagHeader in _aumentumContext.FlagHeaders
                       where maxEffectiveDateFlagHeader.Id == flagHeader.Id &&
                     maxEffectiveDateFlagHeader.BeginEffectiveDate <= DateTime.Now
                       //If no rows are returned then there is nothing to max and we should throw
                       //but Max has to handle this possibility otherwise the LINQ engine won't
                       //convert Max to SQL
                       //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                       select maxEffectiveDateFlagHeader).Max(new Func<FlagHeader, DateTime?>(selectedFlagHeader => selectedFlagHeader.BeginEffectiveDate)) &&

                      sysType.BeginEffectiveDate ==
                      (from maxEffectiveDateSysType in _aumentumContext.SystemTypes
                       where maxEffectiveDateSysType.Id == sysType.Id &&
                       maxEffectiveDateSysType.BeginEffectiveDate <= DateTime.Now
                       //If no rows are returned then there is nothing to max and we should throw
                       //but Max has to handle this possibility otherwise the LINQ engine won't
                       //convert Max to SQL
                       //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                       select maxEffectiveDateSysType).Max(new Func<SystemType, DateTime?>(selectedSysType => selectedSysType.BeginEffectiveDate))

                    select new BaseValueSegmentFlag
                    {
                      Description = sysType.Description,
                      Id = sysType.Id,
                      RevenueObjectId = revenueObjectId
                    }).ToListAsync();
    }
  }
}
