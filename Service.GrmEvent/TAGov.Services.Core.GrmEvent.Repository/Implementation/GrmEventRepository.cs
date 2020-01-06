using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TAGov.Services.Core.GrmEvent.Repository.Interfaces;
using TAGov.Services.Core.GrmEvent.Repository.Models.V1;

namespace TAGov.Services.Core.GrmEvent.Repository.Implementation
{
  public class GrmEventRepository : IGrmEventRepository
  {
    private readonly GrmEventContext _grmEventContext;
    private const int GrmEventTableId = 100435;
    private const int GrmEventGroupTableId = 161088;

    public GrmEventRepository( GrmEventContext grmEventContext )
    {
      _grmEventContext = grmEventContext;
    }

    public Models.V1.GrmEvent Get( int id )
    {
      return _grmEventContext.GrmEvent.Where( x =>
                                                x.Id == id )
                             .Select( grm => new Models.V1.GrmEvent
                                             {
                                               Id = grm.Id,
                                               TranId = grm.TranId,
                                               GRMEventGroupId = grm.GRMEventGroupId,
                                               ObjectType = grm.ObjectType,
                                               ObjectId = grm.ObjectId,
                                               TaxBillId = grm.TaxBillId,
                                               EventType = grm.EventType,
                                               EventDate = grm.EventDate,
                                               GRMModule = grm.GRMModule,
                                               BillTypeId = grm.BillTypeId,
                                               BillNumber = grm.BillNumber,
                                               TaxYear = grm.TaxYear,
                                               RevObjId = grm.RevObjId,
                                               PIN = grm.PIN,
                                               Info = grm.Info,
                                               EffDate = grm.EffDate,
                                               EffTaxYear = grm.EffTaxYear,

                                               EventTypeShortDescription =
                                                 _grmEventContext.SystemType.Where( sysType => sysType.Id == grm.EventType && sysType.effStatus == "A" )
                                                                 .OrderByDescending( orderedSysType => orderedSysType.begEffDate )
                                                                 .First()
                                                                 .shortDescr.Trim(),

                                             } ).SingleOrDefault();
    }

    public IEnumerable<GrmEventInformation> GetGrmEventInfo( int[] grmEventIdList )
    {
      string sql = @"
					  DECLARE @SysType          INT; EXEC grm_GetSysTypeId 'Object Type', 'SysType', @SysType OUTPUT
					  DECLARE @GRMEventReasonCd INT; EXEC grm_GetSysTypeCatId 'GRMEventReasonCd', @GRMEventReasonCd OUTPUT

					  ;WITH ge AS
					  (
							SELECT    ge.Id AS GRMEventId
									, ge.EventDate
									, ge.EffDate
									, ge.RevObjId
									, dbo.STDescr('12/31/9999', ge.EventType) AS EventType
							  FROM  GRMEvent ge
							WHERE  ge.Id in ({0})
					  ), gea AS
					  (
							SELECT  ge.GRMEventId
									, ge.EventDate
									, ge.RevObjId
									, dbo.STDescr('12/31/9999', gea.ObjectId) AS ChangeReason
							  FROM  ge
									INNER JOIN GRMEventArtifact gea 
									  ON  gea.GRMEventId = ge.GRMEventId
							 WHERE  gea.ObjectType = @SysType
									AND EXISTS(SELECT 1 FROM grm_fw_SysTypeByEffDate('12/31/9999', NULL) st WHERE st.Id = gea.ObjectId AND st.SysTypeCatId = @GRMEventReasonCd)
					  )

					  SELECT  
							ge.GRMEventId
							, ge.RevObjId AS RevenueObjectId
							, ge.EventDate
							, ge.EffDate AS EffectiveDate
							, CASE 
								  WHEN gea.GRMEventId IS NULL THEN ge.EventType
								  ELSE gea.ChangeReason
								END AS EventType
							, CONCAT(
								CASE 
								  WHEN gea.GRMEventId IS NULL THEN ge.EventType
								  ELSE gea.ChangeReason
								END
							  , ' - '
							  , CONVERT(VARCHAR, ge.EffDate, 101)
							  ) AS Descr
						FROM  ge
							  LEFT OUTER JOIN gea 
								ON  gea.GRMEventId = ge.GRMEventId
			";

      var parameterList = new List<SqlParameter>();
      var where = new StringBuilder();
      SqlParameter parameter;

      for ( var i = 0; i < grmEventIdList.Length; i++ )
      {
        where.Append( "@grmEventId" + i );

        parameter = new SqlParameter
                    {
                      ParameterName = "@grmEventId" + i,
                      DbType = DbType.Int32,
                      Value = grmEventIdList[ i ]
                    };
        parameterList.Add( parameter );
        if ( i != grmEventIdList.Length - 1 )
        {
          where.Append( "," );
        }
      }

      sql = string.Format( sql, where.ToString() );

      var grmEventInformation = _grmEventContext.GrmEventInformation.FromSql( sql, parameterList.ToArray() );

      return grmEventInformation;
    }

    public IEnumerable<GrmEventInformation> GetGrmEventInfoByRevObjIdAndEffectiveDate( int revenueObjectId,
                                                                                       DateTime effectiveDate )
    {

      string sql = @"
					DECLARE @ParcelValueSummary INT; EXEC grm_GetSysTypeId 'Object Type', 'ParcelValueSum', @ParcelValueSummary OUTPUT

						  ;WITH rp AS
						  (
									SELECT    pvs.Id AS ParcelValueSummaryId
											, pvs.ReasonCd
											, pvs.BegEffDate
									  FROM  ParcelValueSummary pvs
									 WHERE  pvs.RevObjId = @RevObjId
											AND pvs.EffStatus = 'A'
											AND pvs.BegEffDate = (SELECT MAX(sub.BegEffDate) FROM ParcelValueSummary sub WHERE sub.Id = pvs.Id AND DATEDIFF(DAY, sub.BegEffDate, pvs.BegEffDate) = 0)
						  )
						  SELECT  gea.GRMEventId
								, dbo.STDescr('12/31/9999', rp.ReasonCd) + ' - ' + CONVERT(VARCHAR(10), rp.BegEffDate, 10) AS Descr
								, @RevObjId AS RevenueObjectId
								, rp.BegEffDate AS EffectiveDate
								, ge.EventDate
								, st.descr AS EventType
							FROM  rp
								  INNER JOIN GRMEventArtifact gea
									ON  gea.ObjectType = @ParcelValueSummary
										AND gea.ObjectId = rp.ParcelValueSummaryId
								  INNER JOIN GRMEvent ge
									ON ge.Id = gea.GRMEventId
								  INNER JOIN SysType st
									ON st.id = ge.EventType 
						ORDER BY  rp.BegEffDate DESC, rp.ParcelValueSummaryId DESC
					";

      var parameterList = new List<SqlParameter>
                          {
                            new SqlParameter
                            {
                              ParameterName = "@RevObjId",
                              DbType = System.Data.DbType.Int32,
                              Value = revenueObjectId
                            }
                          };

      var legalPartyDocuments = _grmEventContext.GrmEventInformation.FromSql( sql, parameterList.ToArray() );

      return legalPartyDocuments;
    }

    public IEnumerable<SubComponentValue> GetSubComponentValues( string pin, DateTime effectiveDate )
    {
      const string sql = @"
	DECLARE @RevObjId     INT;
	SELECT @RevObjId = MAX(ro.Id) FROM RevObj ro WHERE PIN = @pin

      CREATE TABLE #drv
      (   EffDate                DATE
        , ObjectType              INT
        , ObjectId                INT )

      DECLARE @LatestDate        DATE

      INSERT INTO #drv (EffDate, ObjectType, ObjectId)
            SELECT    rpa.BegEffDate
                    , rpa.ObjectType
                    , rpa.ObjectId 
              FROM  GRM_RPA_API_ConcludedSummary() rpa
             WHERE  rpa.RevObjId = @RevObjId
                    AND rpa.BegEffDate <= @EffDate


      SELECT  @LatestDate = MAX(EffDate) FROM #drv   

      ;WITH drv AS
      (
          SELECT    drv.ObjectType
                  , drv.ObjectId
                  , api.ValueType
                  , api.ValueAmount
            FROM  #drv drv
                  LEFT OUTER JOIN GRM_RPA_API_ConcludedDailyValues(DEFAULT) api
                    ON  api.ObjectType = drv.ObjectType
                        AND api.ObjectId = drv.ObjectId
                        AND api.RevObjId = @RevObjId
                        AND api.EffDate = @EffDate
           WHERE  drv.EffDate = @LatestDate
      )
      SELECT    vtoi.Id AS SubComponentId
              , dbo.GRM_RPA_API_ObjectDescriptionByObjectType( drv.ObjectType, drv.ObjectId, @EffDate)  AS Description
              , SUM( IIF( drv.ValueType IN('Building', 'Feature', 'Land', 'LivImp', 'Production'), drv.ValueAmount, 0) ) AS MarketValue
              , SUM( IIF( drv.ValueType IN('NewConstruction', 'PartialNewCon', 'Restoration', 'PartialRestore', 'CalamityVal', 'Demolition'), drv.ValueAmount, 0) ) AS DeltaValue
        FROM  drv
              INNER JOIN ValueTypeObjectIndex vtoi
                ON  vtoi.ObjectType = drv.ObjectType
                    AND vtoi.ObjectId = drv.ObjectId
    GROUP BY  vtoi.Id, drv.ObjectType, drv.ObjectId



    DROP TABLE #drv
			";

      return _grmEventContext.SubComponentValues.FromSql( sql, new SqlParameter
                                                               {
                                                                 ParameterName = "@pin",
                                                                 DbType = DbType.String,
                                                                 Value = pin
                                                               }, new SqlParameter
                                                                  {
                                                                    ParameterName = "@EffDate",
                                                                    DbType = DbType.DateTime,
                                                                    Value = effectiveDate
                                                                  } ).ToList();
    }

    public Models.V1.GrmEvent Get( int revenueObjectId, DateTime effectiveDate, int eventType )
    {
      return
        _grmEventContext.GrmEvent.FirstOrDefault(
          x => x.RevObjId == revenueObjectId && x.EffDate.Date == effectiveDate.Date && x.EventType == eventType );
    }


    public IEnumerable<GrmEventCreate> CreateGrmEvents( IEnumerable<GrmEventComponentCreate> grmEventComponentCreate )
    {
      var grmEventCreateList = new List<GrmEventCreate>();

      using ( var transaction = _grmEventContext.Database.BeginTransaction() )
      {
        try
        {
          foreach ( var eventComponentCreate in grmEventComponentCreate )
          {
            var existingGrmEvent = Get( eventComponentCreate.GrmEventCreateDto.RevenueObjectId, eventComponentCreate.GrmEventCreateDto.EffectiveDateTime, eventComponentCreate.GrmEventCreateDto.EventType );

            if ( existingGrmEvent != null )
            {
              eventComponentCreate.GrmEventCreateDto.GrmEventId = existingGrmEvent.Id;
            }
            else
            {
              var grmEvent = eventComponentCreate.GrmEventComponentDto.GrmEvent;
              var grmEventGroup = eventComponentCreate.GrmEventComponentDto.GrmEventGroup;
              var transactionHeader = eventComponentCreate.GrmEventComponentDto.TransactionHeader;
              var transactionDetail = eventComponentCreate.GrmEventComponentDto.TransactionDetail;

              // Create TransactionHeader
              _grmEventContext.TransactionHeaders.Add( transactionHeader );
              _grmEventContext.SaveChanges();

              // Create TransactionDetail
              transactionDetail.Id = transactionHeader.Id;
              _grmEventContext.TransactionDetails.Add( transactionDetail );
              _grmEventContext.SaveChanges();

              // Create GrmEventGroup
              grmEventGroup.Id = GetGrmEventNextNumber( GrmEventGroupTableId, transaction.GetDbTransaction() );
              grmEventGroup.TranId = transactionHeader.Id;

              _grmEventContext.GrmEventGroups.Add( grmEventGroup );
              _grmEventContext.SaveChanges();

              // Create GrmEvent
              grmEvent.Id = GetGrmEventNextNumber( GrmEventTableId, transaction.GetDbTransaction() );
              grmEvent.GRMEventGroupId = grmEventGroup.Id;
              grmEvent.TranId = transactionHeader.Id;

              _grmEventContext.GrmEvent.Add( grmEvent );
              _grmEventContext.SaveChanges();

              eventComponentCreate.GrmEventCreateDto.GrmEventId = grmEvent.Id;
            }

            grmEventCreateList.Add( eventComponentCreate.GrmEventCreateDto );
          }

          transaction.Commit();
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }

      return grmEventCreateList;
    }

    public void Delete( int grmEventId )
    {
      var grmEvent = Get( grmEventId );

      using ( var transaction = _grmEventContext.Database.BeginTransaction() )
      {
        var tranHeader = _grmEventContext.TransactionHeaders.FirstOrDefault( x => x.Id == grmEvent.TranId );
        _grmEventContext.TransactionHeaders.Remove( tranHeader );

        var tranDetail = _grmEventContext.TransactionDetails.FirstOrDefault( x => x.Id == grmEvent.TranId );
        _grmEventContext.TransactionDetails.Remove( tranDetail );

        var grmEventGroup = _grmEventContext.GrmEventGroups.FirstOrDefault( x => x.TranId == grmEvent.TranId );
        _grmEventContext.GrmEventGroups.Remove( grmEventGroup );

        _grmEventContext.GrmEvent.Remove( grmEvent );

        _grmEventContext.SaveChanges();

        transaction.Commit();
      }
    }

    public int GetGrmEventNextNumber( int tableId, DbTransaction trans )
    {

      int nextNumber = -1;
      bool closeConnection = false;

      DbCommand cmd = _grmEventContext.Database.GetDbConnection().CreateCommand();
      cmd.Transaction = trans;

      cmd.CommandText = "dbo.GetSeqNumber";
      cmd.CommandType = CommandType.StoredProcedure;

      cmd.Parameters.Add( new SqlParameter( "@p_Id", SqlDbType.Int ) { Value = tableId } );
      cmd.Parameters.Add( new SqlParameter( "@p_NextNumber", SqlDbType.Int ) { Direction = ParameterDirection.Output } );

      if ( cmd.Connection.State != ConnectionState.Open )
      {
        cmd.Connection.Open();
        closeConnection = true;
      }

      cmd.ExecuteNonQuery();

      nextNumber = ( int ) cmd.Parameters[ "@p_NextNumber" ].Value;

      if ( closeConnection )
      {
        cmd.Connection.Close();
      }


      return nextNumber;
    }


    //public async Task GetGRMEventNextNumber(int tableId)
    //{
    //	int nextNumber = -1;

    //	DbCommand cmd = _grmEventContext.Database.GetDbConnection().CreateCommand();

    //	cmd.CommandText = "dbo.GetSeqNumber";
    //	cmd.CommandType = CommandType.StoredProcedure;

    //	cmd.Parameters.Add(new SqlParameter("@p_Id", SqlDbType.Int) { Value = tableId });
    //	cmd.Parameters.Add(new SqlParameter("@p_NextNumber", SqlDbType.Int) { Direction = ParameterDirection.Output });

    //	if (cmd.Connection.State != ConnectionState.Open)
    //	{
    //		cmd.Connection.Open();
    //	}

    //	var test = await cmd.ExecuteNonQueryAsync();

    //	nextNumber = (int)cmd.Parameters["@p_NextNumber"].Value;

    //	cmd.Connection.Close();

    //	//return nextNumber;
    //}

  }
}