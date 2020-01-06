SET NOCOUNT ON

DECLARE @TAG VARCHAR(16) = '' --'018-333' for a specific TAG id--in this case 018-333--leaving TAG empty converts the whole database

--BEGIN TRANSACTION
    CREATE TABLE #ro
    (   RevObjId              INT NOT NULL ) 

    CREATE TABLE #drv
    (   AsmtEventId           INT NOT NULL
      , AsmtEventTranId       INT NOT NULL
      , RevObjId              INT NOT NULL
      , EventDate            DATE NOT NULL
      , AsmtEventType         INT NOT NULL
      , Seq                   INT NOT NULL      )

    BEGIN TRY
        DECLARE @TranId           INT = -1001

        DECLARE @RevObj           INT; EXEC grm_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT
        DECLARE @BVS              INT; EXEC aa_GetValueTypeId 'BVS', @BVS OUTPUT;  
        DECLARE @Conversion       INT; EXEC [Service.BaseValueSegment].aa_GetBVSTranType 'Conversion', @Conversion OUTPUT

        INSERT INTO #ro (RevObjId)
              SELECT  ro.Id
                FROM  grm_levy_TAGByEffYear(9999, 'A') tag
                      INNER JOIN grm_records_TagRoleByEffDate('12/31/9999', 'A') tr
                        ON  tr.TAGId = tag.Id 
                            AND tr.ObjectType = @RevObj
                      INNER JOIN grm_records_RevObjByEffDate('12/31/9999', 'A') ro
                        ON  ro.Id = tr.ObjectId
              WHERE  LEN(ISNULL(@TAG, '')) = 0 OR tag.ShortDescr = @TAG 

        PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' RevObjs for TAG ' + @TAG

        IF EXISTS(SELECT 1 FROM [Service.BaseValueSegment].BVS bvs WHERE bvs.RevObjId IN(SELECT RevObjId FROM #ro))
        BEGIN
              RAISERROR (N'BVS records already exists for target parcels', 11, 1)     
        END

        INSERT INTO #drv (AsmtEventId, AsmtEventTranId, RevObjId, EventDate, AsmtEventType, Seq)
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
               WHERE  EXISTS( SELECT 1 FROM #ro ro WHERE ro.RevObjId = ae.RevObjId )
                      AND EXISTS( SELECT 1 FROM AsmtEventValue aev WHERE aev.AsmtEventTranId = aet.Id AND aev.ValueTypeId = @BVS )

        PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' AsmtEvents to process'

        INSERT INTO [Service.BaseValueSegment].BVS (RevObjId, AsOf, SequenceNumber, TranId, DynCalcInstanceId)
              SELECT    RevObjId
                      , EventDate
                      , Seq
                      , @TranId
                      , 0
                FROM  #drv

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVS records added'

        INSERT INTO [Service.BaseValueSegment].BVSTran (BVSId, TranId, EffStatus, BVSTranTypeId, DynCalcStepTrackingId)
              SELECT    bvs.Id
                      , @TranId
                      , 'A'
                      , @Conversion
                      , 0
                FROM  #drv drv
                      INNER JOIN [Service.BaseValueSegment].BVS bvs
                        ON  bvs.RevObjId = drv.RevObjId
                            AND bvs.AsOf = drv.EventDate
                            AND bvs.SequenceNumber = drv.Seq
                WHERE  bvs.TranId = @TranId

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSTran records added'
    END TRY  
    BEGIN CATCH  
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR (@ErrorMessage, 11, 1)
    END CATCH  

    DROP TABLE #ro
    DROP TABLE #drv

--ROLLBACK TRANSACTION