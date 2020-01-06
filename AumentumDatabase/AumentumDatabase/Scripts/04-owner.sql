SET NOCOUNT ON

--BEGIN TRANSACTION
    CREATE TABLE #drv
    (   BVSId                 INT NOT NULL
      , BVSTranId             INT NOT NULL
      , RevObjId              INT NOT NULL
      , AsOf                 DATE NOT NULL
      , SequenceNumber        INT NOT NULL    )

    BEGIN TRY
        DECLARE @TranId           INT = -1001

        DECLARE @RevObj           INT; EXEC grm_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT
        DECLARE @BVSValueHdrOvr   INT; EXEC grm_GetSysTypeId 'GRMEventType', 'BVSValueHdrOvr', @BVSValueHdrOvr OUTPUT
        DECLARE @Owner            INT; EXEC grm_GetSysTypeId 'LPRole Type', 'Owner', @Owner OUTPUT

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
                      AND NOT EXISTS( SELECT 1 FROM [Service.BaseValueSegment].[BVSOwner] bvso WHERE bvso.BVSTranId = bvst.Id )

        PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSTrans to process'

        INSERT INTO [Service.BaseValueSegment].[BVSOwner] (BVSTranId, LegalPartyRoleId, BIPercent, DynCalcStepTrackingId, GRMEventId)
              SELECT    drv.BVSTranId
                      , lpr.Id
                      , lpr.PercentBeneficialInt
                      , 0
                      , @BVSValueHdrOvrId
                FROM  #drv drv
                      INNER JOIN LegalPartyRole lpr
                        ON  lpr.ObjectType = @RevObj
                            AND lpr.ObjectId = drv.RevObjId
                            AND lpr.LPRoleType = @Owner
                            AND lpr.EffStatus = 'A'
                            AND lpr.BegEffDate = (SELECT MAX(sub.BegEffDate) FROM LegalPartyRole sub WHERE sub.Id = lpr.Id AND DATEDIFF(DAY, sub.BegEffDate, drv.AsOf) >= 0)
               WHERE  lpr.PercentBeneficialInt > 0

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSOwner records added'

    END TRY  
    BEGIN CATCH  
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR (@ErrorMessage, 11, 1)
    END CATCH  

    DROP TABLE #drv

--ROLLBACK TRANSACTION