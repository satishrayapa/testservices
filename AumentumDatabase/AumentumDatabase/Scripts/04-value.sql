SET NOCOUNT ON

--BEGIN TRANSACTION
    CREATE TABLE #drv
    (   BVSId                 INT NOT NULL
      , BVSTranId             INT NOT NULL
      , RevObjId              INT NOT NULL
      , AsOf                 DATE NOT NULL
      , SequenceNumber        INT NOT NULL
      , AsmtEventId           INT     NULL
      , AsmtEventTranId       INT     NULL
      , AsmtEventType         INT     NULL    )

    CREATE TABLE #bvs
    (   Id                          INT         NOT NULL IDENTITY(1,1)
      , BVSTranId                   INT         NOT NULL
      , SubComponent                INT         NOT NULL
      , BaseYear                    INT         NOT NULL
      , BaseValue               DECIMAL(28,10)  NOT NULL
      , TrendYear                   INT         NOT NULL
      , FBYV                    DECIMAL(28,10)      NULL    )

    CREATE TABLE #CPIs  
    (   PctYear           INT         NOT NULL  
      , Pct           DECIMAL(28,10)  NOT NULL  )  

    BEGIN TRY
        DECLARE @TranId           INT = -1001

        DECLARE @BVS              INT; EXEC aa_GetValueTypeId 'BVS', @BVS OUTPUT;  
        DECLARE @BVSValue         INT; EXEC aa_GetValueTypeId 'BVSValue', @BVSValue OUTPUT;  

        DECLARE @CPI              INT; EXEC aa_GetValueTypeId 'CPI', @CPI OUTPUT  
        DECLARE @CntyWide         INT; EXEC aa_GetSysTypeId 'CntyWide', 'CntyWide', @CntyWide OUTPUT  

        DECLARE @BVSValueHdrOvr   INT; EXEC grm_GetSysTypeId 'GRMEventType', 'BVSValueHdrOvr', @BVSValueHdrOvr OUTPUT
        DECLARE @BVSValueHdrOvrId INT; SELECT  @BVSValueHdrOvrId = Id FROM GRMEvent WHERE EventType = @BVSValueHdrOvr AND TranId = @TranId
        IF ISNULL(@BVSValueHdrOvrId, 0) = 0
        BEGIN
              RAISERROR (N'Can not find GRMEvent for [BVSValueHdrOvr]', 11, 1)     
        END 

        INSERT INTO #drv (BVSId, BVSTranId, RevObjId, AsOf, SequenceNumber)
              SELECT    bvs.Id
                      , bvst.Id
                      , bvs.RevObjId
                      , bvs.AsOf
                      , bvs.SequenceNumber
                FROM  [Service.BaseValueSegment].BVS bvs
                      INNER JOIN [Service.BaseValueSegment].BVSTran bvst
                        ON  bvst.BVSId = bvs.Id
               WHERE  bvs.TranId = @TranId
                      AND NOT EXISTS( SELECT 1 FROM [Service.BaseValueSegment].[BVSValueHeader] bvsvh WHERE bvsvh.BVSTranId = bvst.Id )

        PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSTrans to process'

        ;WITH ae AS
        (
              SELECT    ae.Id AS AsmtEventId
                      , aet.Id AS AsmtEventTranId
                      , ae.RevObjId
                      , CONVERT(DATE, ae.EventDate) AS EventDate
                      , ae.AsmtEventType
                      , RANK() OVER(PARTITION BY    ae.RevObjId
                                                  , CONVERT(DATE, ae.EventDate)
                                        ORDER BY    ae.RevObjId
                                                  , CONVERT(DATE, ae.EventDate)
                                                  , ISNULL(w.Wgt, 9999)
                                                  , ae.Id ) AS Seq
                FROM  AsmtEvent ae
                      INNER JOIN AsmtEventTran aet
                        ON  aet.Id = (SELECT MAX(sub.Id) FROM AsmtEventTran sub WHERE sub.AsmtEventId = ae.Id )
                      LEFT JOIN dbo.grm_aa_ca_AsmtEventTypeWeights() w
                        ON	w.AsmtEventTypeId = ae.AsmtEventType
               WHERE  EXISTS( SELECT 1 FROM #drv drv WHERE drv.RevObjId = ae.RevObjId )
                      AND EXISTS( SELECT 1 FROM AsmtEventValue aev WHERE aev.AsmtEventTranId = aet.Id AND aev.ValueTypeId = @BVS )
        )
        UPDATE  #drv
           SET  AsmtEventId = ae.AsmtEventId
              , AsmtEventTranId = ae.AsmtEventTranId
              , AsmtEventType = ae.AsmtEventType
         FROM ae
        WHERE ae.RevObjId = #drv.RevObjId
              AND ae.EventDate = #drv.AsOf
              AND ae.Seq = #drv.SequenceNumber

        IF EXISTS(SELECT 1 FROM #drv WHERE AsmtEventId IS NULL)
        BEGIN
              RAISERROR (N'Found BVSTrans that could not be linked to AsmtEventTran', 11, 1)     
        END

        INSERT INTO #bvs (BVSTranId, SubComponent, BaseYear, TrendYear, BaseValue)
              SELECT    drv.BVSTranId
                      , bvsv.Attribute2
                      , bvs.Attribute4
                      , IIF( MONTH(drv.AsOf) BETWEEN 1 AND 6, YEAR(drv.AsOf), YEAR(drv.AsOf)+1 )
                      , SUM(bvsv.ValueAmount)
                FROM  #drv drv 
                      INNER JOIN AsmtEventValue bvs
                        ON  bvs.AsmtEventTranId = drv.AsmtEventTranId
                            AND bvs.ValueTypeId =  @BVS
                      INNER JOIN AsmtEventValue bvsv
                        ON  bvsv.AsmtEventTranId = drv.AsmtEventTranId
                            AND bvsv.ValueTypeId = @BVSValue
                            AND bvsv.Attribute1 = bvs.Attribute1
            GROUP BY    drv.BVSTranId
                      , bvsv.Attribute2
                      , bvs.Attribute4
                      , drv.AsOf

        INSERT INTO #CPIs (PctYear, Pct)  
              SELECT    vo.AddlObjectId   
                      , vo.ValueAmount  
                FROM  GRM_AA_ValueOverrideByEffYear(9999, 'A') vo    
               WHERE  vo.ValueTypeId = @CPI  
                      AND vo.ObjectId = @CntyWide  

        ;WITH tbv AS   
        (  
              SELECT    bvs.Id  
                      , bvs.TrendYear  
                      , CONVERT(INT, dbo.aa_Lesser(bvs.TrendYear, bvs.BaseYear)) AS TrendingYear  
                      , CONVERT(DECIMAL(28,10), 1.0) AS Pct  
                      , bvs.BaseValue AS Value  
                FROM  #bvs bvs  
              UNION ALL  
              SELECT  tbv.Id  
                      , tbv.TrendYear  
                      , tbv.TrendingYear+1 AS TrendingYear  
                      , cpi.Pct AS Pct  
                      , CONVERT(DECIMAL(28,10), FLOOR(tbv.Value * cpi.Pct)) AS Value  
                FROM tbv  
                      INNER JOIN #CPIs cpi  
                        ON cpi.PctYear = tbv.TrendingYear+1   
                WHERE  tbv.TrendYear > tbv.TrendingYear  
        )  
        UPDATE  #bvs
           SET  FBYV = tbv.Value  
          FROM  tbv   
         WHERE  tbv.Id = #bvs.Id  
                AND tbv.TrendingYear = #bvs.TrendYear  

        INSERT INTO [Service.BaseValueSegment].[BVSValueHeader] (BVSTranId, GRMEventId, BaseYear)
              SELECT DISTINCT
                        bvs.BVSTranId
                      , @BVSValueHdrOvrId
                      , bvs.TrendYear
                FROM  #bvs bvs

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSValueHeader records added'

        INSERT INTO [Service.BaseValueSegment].[BVSValue] (BVSValueHeaderId, SubComponent, ValueAmount, PctComplete, FullValueAmount, DynCalcStepTrackingId)
              SELECT    bvsvh.Id
                      , bvs.SubComponent
                      , SUM(bvs.FBYV)
                      , 100
                      , SUM(bvs.FBYV)
                      , 0
                FROM  #bvs bvs
                      INNER JOIN [Service.BaseValueSegment].[BVSValueHeader] bvsvh
                        ON  bvsvh.BVSTranId = bvs.BVSTranId
				WHERE bvs.FBYV IS NOT NULL
            GROUP BY  bvsvh.Id, bvs.SubComponent

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSValue records added'
    END TRY  
    BEGIN CATCH  
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR (@ErrorMessage, 11, 1)
    END CATCH  

    DROP TABLE #drv
    DROP TABLE #bvs
    DROP TABLE #CPIs

--ROLLBACK TRANSACTION