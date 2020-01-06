using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1
{
  public class BaseValueSegmentRepository : IBaseValueSegmentRepository
  {
    private readonly BaseValueSegmentQueryContext _baseValueSegmentQueryContext;

    public BaseValueSegmentRepository( BaseValueSegmentQueryContext baseValueSegmentQueryContext )
    {
      _baseValueSegmentQueryContext = baseValueSegmentQueryContext;
    }

    public async Task<Models.V1.BaseValueSegment> CreateAsync( Models.V1.BaseValueSegment baseValueSegment, IEnumerable<BaseValueSegmentOwnerValue> baseValueSegmentOwnerValuesList )
    {
      using ( var transaction = await _baseValueSegmentQueryContext.Database.BeginTransactionAsync() )
      {
        try
        {
          await _baseValueSegmentQueryContext.BaseValueSegments.AddAsync( baseValueSegment );
          await _baseValueSegmentQueryContext.SaveChangesAsync();

          var baseValueSegmentOwnerValues = baseValueSegmentOwnerValuesList.ToList();
          if ( baseValueSegmentOwnerValues.Count > 0 )
          {
            baseValueSegmentOwnerValues.ForEach( x =>
                                                 {
                                                   if ( x.Header == null )
                                                   {
                                                     // This allows us to determine in the DTO that was submitted.
                                                     throw new NullReferenceException( $"Header cannot be set for Base Value Segment Owner with Id: {x.Id}" );
                                                   }

                                                   x.Id = 0;
                                                   x.BaseValueSegmentOwnerId = x.Owner.Id;
                                                   x.BaseValueSegmentValueHeaderId = x.Header.Id;
                                                 } );

            await _baseValueSegmentQueryContext.BaseValueSegmentOwnerValues.AddRangeAsync( baseValueSegmentOwnerValues );
            await _baseValueSegmentQueryContext.SaveChangesAsync();
          }

          transaction.Commit();

          return baseValueSegment;
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public Models.V1.BaseValueSegment Get( int id )
    {
      var baseValueSegments = _baseValueSegmentQueryContext.BaseValueSegments.Where( x => x.Id == id )
                                                           .Include( x => x.BaseValueSegmentTransactions )
                                                           .Include( x => x.BaseValueSegmentTransactions )
                                                           .ThenInclude( y => y.BaseValueSegmentTransactionType )
                                                           .Include( x => x.BaseValueSegmentTransactions )
                                                           .ThenInclude( y => y.BaseValueSegmentOwners )
                                                           .ThenInclude( y => y.BaseValueSegmentOwnerValueValues )
                                                           .Include( x => x.BaseValueSegmentTransactions )
                                                           .ThenInclude( y => y.BaseValueSegmentValueHeaders )
                                                           .ThenInclude( y => y.BaseValueSegmentValues )
                                                           .Include( x => x.BaseValueSegmentTransactions )
                                                           .ThenInclude( y => y.BaseValueSegmentValueHeaders )
                                                           .ThenInclude( y => y.BaseValueSegmentOwnerValues )
                                                           .Include( x => x.BaseValueSegmentAssessmentRevisions )
                                                           .ThenInclude( y => y.BaseValueSegmentStatusType )
                                                           .OrderByDescending( x => x.AsOf )
                                                           .ThenByDescending( x => x.SequenceNumber );
      if ( !baseValueSegments.Any() )
      {
        return null;
      }

      var baseValueSegment = baseValueSegments.ToList().FirstOrDefault();
      var bvsActiveTransactions = baseValueSegment.BaseValueSegmentTransactions
                                                  .Where( x => x.BaseValueSegmentTransactionType != GetUserDeletedTransactionType() )
                                                  .ToList();
      baseValueSegment.BaseValueSegmentTransactions.Clear();
      foreach ( BaseValueSegmentTransaction bvsActiveTransAction in bvsActiveTransactions )
      {
        baseValueSegment.BaseValueSegmentTransactions.Add( bvsActiveTransAction );
      }

      return baseValueSegment;
    }

    public Models.V1.BaseValueSegment GetByRevenueObjectIdAndAssessmentEventDate( int revenueObjectId,
                                                                                  DateTime assessmentEventDate )
    {
      var baseValueSegments = _baseValueSegmentQueryContext.BaseValueSegments
                                                           .Where( x => x.RevenueObjectId == revenueObjectId )
                                                           .Where( x => x.AsOf == assessmentEventDate )
                                                           .OrderByDescending( x => x.AsOf )
                                                           .ThenByDescending( x => x.SequenceNumber );
      if ( !baseValueSegments.Any() )
      {
        return null;
      }

      return Get( baseValueSegments.First().Id );
    }

    public IEnumerable<BaseValueSegmentEvent> GetBvsEventsByRevenueObjectId( int revenueObjectId )
    {
      return ( from bvs in _baseValueSegmentQueryContext.BaseValueSegments
               join bvsTran in _baseValueSegmentQueryContext.BaseValueSegmentTransactions on bvs.Id equals bvsTran
                 .BaseValueSegmentId
               join bvsOwnwer in _baseValueSegmentQueryContext.BaseValueSegmentOwners on bvsTran.Id equals bvsOwnwer
                 .BaseValueSegmentTransactionId
               where bvs.RevenueObjectId == revenueObjectId && bvsTran.BaseValueSegmentTransactionType != GetUserDeletedTransactionType()
               orderby bvs.AsOf descending, bvs.SequenceNumber descending
               select new BaseValueSegmentEvent
                      {
                        BvsId = bvs.Id,
                        BvsAsOf = bvs.AsOf,
                        GRMEventId = bvsOwnwer.GRMEventId,
                        RevenueObjectId = bvs.RevenueObjectId,
                        SequenceNumber = bvs.SequenceNumber
                      } ).ToList()
                         .GroupBy( x => x.BvsAsOf )
                         .Select( x => x.First() )
                         .ToList();
    }

    public async Task<IEnumerable<SubComponentDetail>> GetSubComponentDetailsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      const string sql = @"
            DECLARE @Building         INT; EXEC aa_GetSysTypeId 'Object Type', 'Building', @Building OUTPUT  
            DECLARE @Feature          INT; EXEC aa_GetSysTypeId 'Object Type', 'Feature', @Feature  OUTPUT  
            DECLARE @LandLine         INT; EXEC aa_GetSysTypeId 'Object Type', 'LandLine', @LandLine OUTPUT  
            DECLARE @LandUseDetail    INT; EXEC aa_GetSysTypeId 'Object Type', 'LandUseDetail', @LandUseDetail OUTPUT

            ;WITH rp AS 
            ( 
               SELECT    CASE WHEN api.ObjectType = @LandUseDetail THEN api.ParentObjectType ELSE api.ObjectType END AS ObjectType
                       , CASE WHEN api.ObjectType = @LandUseDetail THEN api.ParentObjectId ELSE api.ObjectId END AS ObjectId
                       , api.EffDate as BegEffDate
                       , DENSE_RANK() OVER(ORDER BY api.EffDate DESC ) AS Seq
                 FROM  dbo.GRM_RPA_API_ConcludedDailyValues( DEFAULT ) api
                WHERE  api.RevObjId = @RevObjId
                       AND api.EffDate <= @AsOfDate
             GROUP BY    CASE WHEN api.ObjectType = @LandUseDetail THEN api.ParentObjectType ELSE api.ObjectType END
                       , CASE WHEN api.ObjectType = @LandUseDetail THEN api.ParentObjectId ELSE api.ObjectId END
                       , api.EffDate
            )
            SELECT    vtoi.Id AS SubComponentId
                    , IIF(rp.ObjectType = @Building OR rp.ObjectType = @Feature OR rp.ObjectType = @LandLine, 
                          dbo.GRM_RPA_API_ObjectDescriptionByObjectType( rp.ObjectType, rp.ObjectId, rp.BegEffDate ), 
                          dbo.STDescr(@AsOfDate, vtoi.ObjectType))  AS SubComponent
                    , sc.ComponentType AS ComponentTypeId
                    , st.Descr AS Component
              FROM  rp
                    INNER JOIN ValueTypeObjectIndex vtoi
                      ON  vtoi.ObjectType = rp.ObjectType
                      AND vtoi.ObjectId = rp.ObjectId
                    INNER JOIN dbo.GRM_AA_ca_RP_SubComponents( @RevObjId, @AsOfDate ) sc
                      ON  sc.ObjectType = rp.ObjectType
                      AND sc.ObjectId = rp.ObjectId
                    INNER JOIN dbo.GRM_FW_SysTypeByEffDate( @AsOfDate, NULL ) st
                      ON  st.Id = sc.ComponentType
              WHERE  rp.Seq = 1
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
                              ParameterName = "@AsOfDate",
                              DbType = System.Data.DbType.DateTime,
                              Value = asOfDate
                            }
                          };

      return await _baseValueSegmentQueryContext.SubComponentDetails
                                                // FromSql does takes in an array of SqlParameters.
                                                // ReSharper disable once CoVariantArrayConversion
                                                // ReSharper disable once FormatStringProblem
                                                .FromSql( sql, parameterList.ToArray() )
                                                .ToListAsync();
    }

    public IEnumerable<BeneficialInterestEvent> GetBeneficialInterestsByRevenueObjectId( int revenueObjectId, DateTime asOfDate )
    {
      var sql = @"
					  DECLARE @RevObj           INT; EXEC aa_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT
					  DECLARE @RightTransfer    INT; EXEC aa_GetSysTypeId 'Object Type', 'RightTransfer', @RightTransfer OUTPUT
					  DECLARE @Transfer   INT; EXEC aa_GetSysTypeId 'GRMEventType', 'Transfer', @Transfer OUTPUT
  
					  CREATE TABLE #drv
					  (   LegalPartyRoleId INT NOT NULL
						, LegalPartyId   INT NOT NULL
						, PercentBeneficialInt DECIMAL(14, 10) NOT NULL 
						, DisplayName VARCHAR(500) 
					  )

					  CREATE TABLE #gainers
					  (   LegalPartyRoleId    INT NOT NULL
						, RightTransferId     INT NOT NULL
						, PctGain           DECIMAL(14,10) NOT NULL
						, GRMEventId          INT     NULL
					  )

					  INSERT INTO #drv (LegalPartyRoleId, LegalPartyId, PercentBeneficialInt, DisplayName)
						  SELECT    lpr.Id LegalPartyRoleId
								  , lp.Id LegalPartyId
								  , lpr.PercentBeneficialInt
								  , lp.DisplayName
							FROM  grm_records_LegalPartyRoleByEffDate(@AsOf, 'A') lpr
								  LEFT OUTER JOIN LegalParty lp
									ON  lp.Id = lpr.LegalPartyId
						   WHERE  lpr.ObjectType = @RevObj
								  AND lpr.ObjectId = @RevObjId
								  AND lpr.PercentBeneficialInt > 0

					  INSERT INTO #gainers ( LegalPartyRoleId, RightTransferId, PctGain)
							SELECT  drv.LegalPartyRoleId, tee.RightTransferId, lprTee.PercentBeneficialInt - ISNULL(lprTor.PercentBeneficialInt, 0) as PctGain
							  FROM  #drv drv
									INNER JOIN  grm_records_RightHistByEffDate('12/31/9999', 'A') tee
									  ON  tee.LPRId = drv.LegalPartyRoleId
										  AND tee.GrantorGrantee = 1
									INNER JOIN LegalPartyRole lprTee
									  ON  lprTee.Id = tee.LPRId
										  AND lprTee.BegEffDate = tee.LPRBegEffDate

									LEFT OUTER JOIN  grm_records_RightHistByEffDate('12/31/9999', 'A') tor
										ON  tor.RightTransferId = tee.RightTransferId
											AND tor.LPRId = drv.LegalPartyRoleId
 											AND tor.GrantorGrantee = 0
									LEFT OUTER JOIN LegalPartyRole lprTor
									  ON  lprTor.Id = tor.LPRId
										  AND lprTor.BegEffDate = tor.LPRBegEffDate
							 WHERE  lprTee.PercentBeneficialInt > ISNULL(lprTor.PercentBeneficialInt, 0)

					  ;WITH ge AS 
					  (
							SELECT  gain.RightTransferId, COALESCE(MAX(ge.Id), MAX(ge2.Id)) AS GRMEventId
							  FROM  #gainers gain
									LEFT OUTER JOIN GRMEventArtifact gea
									  ON  gea.ObjectType = @RightTransfer
										  AND gea.ObjectId = gain.RightTransferId
									LEFT OUTER JOIN GRMEvent ge
									  ON  ge.Id = gea.GRMEventId
										  AND ge.EventType = @Transfer
									LEFT OUTER JOIN GRMEvent ge2 -- to catch converted data
									  ON ge2.RevObjId = @RevObjId
										  AND ge2.EffDate <= @AsOf
										  AND ge2.EventType = @Transfer
						   GROUP BY  gain.RightTransferId
					  )
					  UPDATE #gainers
						 SET  GRMEventId = ge.GRMEventId 
						FROM  ge
					   WHERE  ge.RightTransferId = #gainers.RightTransferId

						--SELECT * FROM #drv
						--SELECT * FROM #gainers

						SELECT  drv.LegalPartyId 
								, drv.LegalPartyroleId
								, gain.GRMEventId
								, dbo.STDescr('12/31/9999', grme.EventType) AS EventType
								, grme.EventDate
								, grme.EffDate AS EffectiveDate
								, od.DocDate
								, od.DocNumber
								, dbo.STDescr('12/31/9999', od.DocType) AS DocType
								, drv.PercentBeneficialInt  AS BeneficialInterestPercentage   
								, gain.PctGain AS PercentageInterestGain   
								, drv.DisplayName AS OwnerName
								, null AS OwnerId
						  FROM  #drv drv
								INNER JOIN #gainers gain
								  ON  gain.LegalPartyroleId = drv.LegalPartyroleId
								INNER JOIN grm_records_RightTransferByEffDate('12/31/9999', 'A') rt 
								  ON  rt.Id = gain.RightTransferId 
								INNER JOIN grm_records_OfficialDocByEffDate('12/31/9999', 'A') od 
								  ON  od.Id = rt.OfficialDocId 
								INNER JOIN GRMEvent grme
								  ON grme.Id = gain.GRMEventId
							ORDER BY EventDate DESC, OwnerName ASC

						DROP TABLE #drv
						DROP TABLE #gainers

					  ";

      List<SqlParameter> parameterList = new List<SqlParameter>();
      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@RevObjId",
                           DbType = System.Data.DbType.Int32,
                           Value = revenueObjectId
                         } );

      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@AsOf",
                           DbType = System.Data.DbType.DateTime,
                           Value = asOfDate
                         } );

      var beneficialInterests = _baseValueSegmentQueryContext.BeneficialInterests
                                                             .FromSql( sql, parameterList.ToArray() )
                                                             .ToListAsync()
                                                             .Result;

      List<BeneficialInterestEvent> beneficialInterestEvents = new List<BeneficialInterestEvent>();

      beneficialInterestEvents = beneficialInterests
        .Where( x => x.EffectiveDate <= asOfDate )
        .Select( x => new BeneficialInterestEvent
                      {
                        GrmEventId = x.GrmEventId,
                        EventDate = x.EventDate,
                        EventType = x.EventType,
                        DocNumber = x.DocNumber,
                        DocType = x.DocType,
                        DocDate = x.DocDate,
                        EffectiveDate = x.EffectiveDate,
                        Owners = beneficialInterests
                          .Where( y => y.GrmEventId == x.GrmEventId )
                          .Select( y => new OwnerDetail()
                                        {
                                          GrmEventId = y.GrmEventId,
                                          LegalPartyId = y.LegalPartyId,
                                          OwnerId = y.OwnerId,
                                          OwnerName = y.OwnerName,
                                          LegalPartyRoleId = y.LegalPartyRoleId,
                                          BeneficialInterestPercentage = y.BeneficialInterestPercentage,
                                          PercentageInterestGain = y.PercentageInterestGain
                                        } ).ToArray()
                      } ).Distinct( new BeneficialInterestEventComparer() ).ToList();

      //Add BeneficialInterestEvent for Override
      var overrideBeneficialInterestEvent = this.GetOverrideBeneficialInterests( revenueObjectId, asOfDate );

      //Add Override Evnet
      beneficialInterestEvents.Add( overrideBeneficialInterestEvent );

      return beneficialInterestEvents;
    }

    private BeneficialInterestEvent GetOverrideBeneficialInterests( int revenueObjectId, DateTime asOfDate )
    {
      var sql = @"
						DECLARE @RevObj           INT; EXEC aa_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT
						DECLARE @Owner            INT; EXEC aa_GetSysTypeId 'LPRole Type', 'Owner',  @Owner OUTPUT

						SELECT   
								  lpr.Id AS LegalPartyRoleId
								, lpr.LegalPartyId
								, lpr.PercentBeneficialInt AS BeneficialInterestPercentage
								, lp.DisplayName AS OwnerName
								, 0 AS OwnerId
								, 0 AS GrmEventId
								, '' AS EventType
								, '' AS DocNumber
								, '' AS DocType
								, null AS DocDate
								, null AS EffectiveDate
								, lpr.BegEffDate AS EventDate
								, 0 AS PercentageInterestGain

						FROM  grm_records_LegalPartyRoleByEffDate(@AsOf, 'A') lpr
							LEFT OUTER JOIN LegalParty lp
								ON  lp.Id = lpr.LegalPartyId
						WHERE  lpr.ObjectType = @RevObj
							AND lpr.ObjectId = @RevObjId
							AND lpr.PercentBeneficialInt > 0
							AND lpr.LPRoleType = @Owner
					  ";

      var parameterList = new List<SqlParameter>();
      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@RevObjId",
                           DbType = System.Data.DbType.Int32,
                           Value = revenueObjectId
                         } );

      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@AsOf",
                           DbType = System.Data.DbType.DateTime,
                           Value = asOfDate
                         } );

      var overrideBeneficialInterestEvents = _baseValueSegmentQueryContext.BeneficialInterests
                                                                          .FromSql( sql, parameterList.ToArray() )
                                                                          .ToListAsync()
                                                                          .Result;

      //Add BeneficialInterestEvent for Override
      BeneficialInterestEvent overrideBeneficialInterestEvent = new BeneficialInterestEvent();
      overrideBeneficialInterestEvent.GrmEventId = 0;
      overrideBeneficialInterestEvent.EventDate = DateTime.Now; //Not Used,
      overrideBeneficialInterestEvent.EventType = "<Override>";
      overrideBeneficialInterestEvent.DocNumber = string.Empty;
      overrideBeneficialInterestEvent.DocType = string.Empty;
      overrideBeneficialInterestEvent.DocDate = null;
      var overrideOwnerDetails = from overrideBi in overrideBeneficialInterestEvents
                                 select new OwnerDetail()
                                        {
                                          GrmEventId = overrideBi.GrmEventId,
                                          LegalPartyId = overrideBi.LegalPartyId,
                                          OwnerId = overrideBi.OwnerId,
                                          OwnerName = overrideBi.OwnerName,
                                          LegalPartyRoleId = overrideBi.LegalPartyRoleId,
                                          BeneficialInterestPercentage = overrideBi.BeneficialInterestPercentage
                                        };
      overrideBeneficialInterestEvent.Owners = overrideOwnerDetails.ToArray();

      return overrideBeneficialInterestEvent;
    }


    public BaseValueSegmentTransactionType GetUserTransactionType()
    {
      return _baseValueSegmentQueryContext.BaseValueSegmentTransactionTypes.Single( x => x.Name == "User" );
    }

    public BaseValueSegmentStatusType GetNewStatusType()
    {
      return _baseValueSegmentQueryContext.BaseValueSegmentStatusTypes.Single( x => x.Name == "New" );
    }

    public BaseValueSegmentTransactionType GetUserDeletedTransactionType()
    {
      return _baseValueSegmentQueryContext.BaseValueSegmentTransactionTypes.FirstOrDefault(
        x => x.Description == "User Deleted" );
    }

    public IEnumerable<BaseValueSegmentConclusion> GetBaseValueSegmentConclusions( int revenueObjectId, DateTime effectiveDate )
    {
      var sql = @"
				  DECLARE @ParcelValueSummary INT; 
				  EXEC grm_GetSysTypeId 'Object Type', 'ParcelValueSum', @ParcelValueSummary OUTPUT

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
				  SELECT  gea.GRMEventId AS GrmEventId
						, dbo.STDescr('12/31/9999', rp.ReasonCd) as Description 
                        ---, CONVERT(VARCHAR(10), rp.BegEffDate, 10) as ConclusionDate
                        , rp.BegEffDate as ConclusionDate
					FROM  rp
						  INNER JOIN GRMEventArtifact gea
							ON  gea.ObjectType = @ParcelValueSummary
								AND gea.ObjectId = rp.ParcelValueSummaryId
					WHERE rp.BegEffDate <= @AsOfDate
				ORDER BY  rp.BegEffDate DESC, rp.ParcelValueSummaryId DESC
			";

      List<SqlParameter> parameterList = new List<SqlParameter>();
      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@RevObjId",
                           DbType = System.Data.DbType.Int32,
                           Value = revenueObjectId
                         } );

      parameterList.Add( new SqlParameter()
                         {
                           ParameterName = "@AsOfDate",
                           DbType = System.Data.DbType.DateTime,
                           Value = effectiveDate
                         } );

      var conclusions = _baseValueSegmentQueryContext.BaseValueSegmentConclusions
                                                     .FromSql( sql, parameterList.ToArray() )
                                                     .ToListAsync()
                                                     .Result;

      return conclusions;
    }

    public Models.V1.BaseValueSegment Get( int revenueObjectId, DateTime asOf, int sequenceNumber )
    {
      var startOfDay = asOf.Date;
      var endOfDay = startOfDay.AddDays( 1 ).AddSeconds( -1 );

      return _baseValueSegmentQueryContext.BaseValueSegments.SingleOrDefault(
        x => x.RevenueObjectId == revenueObjectId && x.AsOf >= startOfDay && x.AsOf <= endOfDay &&
             x.SequenceNumber == sequenceNumber );
    }

    public IEnumerable<Models.V1.BaseValueSegment> List( int revenueObjectId )
    {
      return _baseValueSegmentQueryContext.BaseValueSegments.Where(
        x => x.RevenueObjectId == revenueObjectId ).ToList();
    }

    public IEnumerable<BaseValueSegmentHistory> GetBaseValueSegmentHistory( int revenueObjectId,
                                                                            DateTime fromDate, DateTime toDate )
    {
      var bvsHistory = ( from bvs in _baseValueSegmentQueryContext.BaseValueSegments
                         join bvsTran in _baseValueSegmentQueryContext.BaseValueSegmentTransactions
                           on bvs.Id equals bvsTran.BaseValueSegmentId
                         join bvsOwner in _baseValueSegmentQueryContext.BaseValueSegmentOwners
                           on bvsTran.Id equals bvsOwner.BaseValueSegmentTransactionId
                         join bvsValueHeader in _baseValueSegmentQueryContext.BaseValueSegmentValueHeaders
                           on bvsTran.Id equals bvsValueHeader.BaseValueSegmentTransactionId
                         join bvsOwnerValue in _baseValueSegmentQueryContext.BaseValueSegmentOwnerValues
                           on bvsOwner.Id equals bvsOwnerValue.BaseValueSegmentOwnerId
                         join bvsValue in _baseValueSegmentQueryContext.BaseValueSegmentValues
                           on bvsValueHeader.Id equals bvsValue.BaseValueSegmentValueHeaderId
                         join bvsTranType in _baseValueSegmentQueryContext.BaseValueSegmentTransactionTypes
                           on bvsTran.BaseValueSegmentTransactionTypeId equals bvsTranType.Id
                         where bvs.RevenueObjectId == revenueObjectId &&
                               bvs.AsOf >= fromDate &&
                               bvs.AsOf <= toDate
                         orderby bvs.Id, bvs.AsOf
                         select new BaseValueSegmentHistory()
                                {
                                  BvsId = bvs.Id,
                                  AsOf = bvs.AsOf,
                                  BaseYear = bvsValueHeader.BaseYear,
                                  BaseValue = bvsOwnerValue.BaseValue,
                                  BeneficialInterestPercentage = bvsOwner.BeneficialInterestPercent,
                                  BvsTransactionType = bvsTranType.Description,
                                  LegalPartyRoleId = bvsOwner.LegalPartyRoleId,
                                  OwnerGrmEventId = bvsOwner.GRMEventId,
                                  SubComponentId = bvsValue.SubComponent,
                                  TransactionId = bvsTran.TransactionId,
                                  ValueHeaderGrmEventId = bvsValueHeader.GRMEventId
                                } ).ToList();

      List<BaseValueSegmentHistory> distinctBvsHistory = new List<BaseValueSegmentHistory>();
      distinctBvsHistory = bvsHistory
        .Select( x => new BaseValueSegmentHistory()
                      {
                        BvsId = x.BvsId,
                        AsOf = x.AsOf,
                        BaseYear = x.BaseYear,
                        BaseValue = x.BaseValue,
                        BeneficialInterestPercentage = x.BeneficialInterestPercentage,
                        BvsTransactionType = x.BvsTransactionType,
                        LegalPartyRoleId = x.LegalPartyRoleId,
                        OwnerGrmEventId = x.OwnerGrmEventId,
                        SubComponentId = x.SubComponentId,
                        TransactionId = x.TransactionId,
                        ValueHeaderGrmEventId = x.ValueHeaderGrmEventId
                      } ).Distinct( new BaseValueSegmentHistoryComparer() ).ToList();

      return distinctBvsHistory;
    }
  }
}
