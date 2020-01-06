using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;
using TAGov.Services.Core.RevenueObject.Repository.Maps.V1;

namespace TAGov.Services.Core.RevenueObject.Repository.Implementation.V1
{
  public class MarketAndRestrictedValueRepository : IMarketAndRestrictedValueRepository
  {
    private readonly RevenueObjectContext _revenueObjectContext;

    public MarketAndRestrictedValueRepository( RevenueObjectContext revenueObjectContext )
    {
      _revenueObjectContext = revenueObjectContext;
    }

    public IList<Models.V1.MarketAndRestrictedValue> Get( int revenueObjectId, DateTime effectiveDate )
    {
      var sql = @"
            DECLARE @Restricted INT; EXEC grm_GetSysTypeId 'ValCd', 'Restricted', @Restricted OUTPUT

            ;WITH rp AS
              (
                SELECT 
                  IIF(api.ParentObjectType <> 0, api.ParentObjectType, api.ObjectType) AS ObjectType
                  , IIF(api.ParentObjectId <> 0, api.ParentObjectId, api.ObjectId) AS ObjectId
                  , IIF( api.ValueType IN('Building', 'Feature', 'Land', 'LivImp'), api.ValueAmount, 0 ) AS MarketValue
                  , IIF( api.ValCd = @Restricted AND api.ValueType IN('BuildingRstrd', 'FeatureRstrd', 'LandRstrd', 'LivImpRstrd'), api.ValueAmount, 0 ) AS RestrictedValue
                FROM GRM_RPA_API_ConcludedDailyValues(DEFAULT) api
                WHERE api.RevObjId = @RevObjId
                AND DATEDIFF(DAY, api.EffDate, @EffDate) = 0
              )

              SELECT 
                vtoi.Id AS SubComponent
                , SUM(rp.MarketValue) AS MarketValue
                , SUM(rp.RestrictedValue) AS RestrictedValue
              FROM rp
                INNER JOIN ValueTypeObjectIndex vtoi
                  ON vtoi.ObjectType = rp.ObjectType
                AND vtoi.ObjectId = rp.ObjectId
                GROUP BY vtoi.Id
          ";

      var parameterList = new List<SqlParameter>
                          {
                            new SqlParameter
                            {
                              ParameterName = "@RevObjId",
                              DbType = System.Data.DbType.Int32,
                              Value = revenueObjectId
                            },
                            new SqlParameter
                            {
                              ParameterName = "@EffDate",
                              DbType = System.Data.DbType.DateTime,
                              Value = effectiveDate
                            }
                          };
      
      var marketAndRestrictedValues = _revenueObjectContext.MarketAndRestrcitedValue.FromSql( sql, parameterList.ToArray() ).ToListAsync().Result;

      return marketAndRestrictedValues;
    }

  }
}
