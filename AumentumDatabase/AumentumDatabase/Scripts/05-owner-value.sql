SET NOCOUNT ON

--BEGIN TRANSACTION
    CREATE TABLE #drv
    (   BVSId                 INT         NOT NULL
      , BVSTranId             INT         NOT NULL
      , RevObjId              INT         NOT NULL
      , AsOf                 DATE         NOT NULL
      , SequenceNumber        INT         NOT NULL   
      
      , BVSValueHeaderId      INT             NULL
      , TotalBaseValue    DECIMAL(28,10)      NULL
      , CalcBaseValue     DECIMAL(28,10)      NULL    )

    CREATE TABLE #owners
    (   Id                    INT         NOT NULL IDENTITY(1,1)
      , BVSTranId             INT         NOT NULL
      , BVSOwnerId            INT         NOT NULL

      , LegalPartyRoleId      INT         NOT NULL
      , DisplayName       VARCHAR(256)        NULL

      , Pct               DECIMAL(28,19)  NOT NULL
      , BaseValue         DECIMAL(28,19)      NULL
      , BaseValueAdjusted DECIMAL(28,19)      NULL    )

    BEGIN TRY
        DECLARE @TranId           INT = -1001

        DECLARE @RevObj           INT; EXEC grm_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT

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
                      AND NOT EXISTS( SELECT  1 
                                        FROM  [Service.BaseValueSegment].[BVSOwner] bvso
                                              INNER JOIN [Service.BaseValueSegment].[BVSOwnerValue] bvsov 
                                                ON  bvsov.BVSOwnerId = bvso.Id
                                       WHERE  bvso.BVSTranId = bvst.Id )

        PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSTrans to process'

        ;WITH val AS
        (
                SELECT    bvsvh.BVSTranId
                        , bvsvh.Id AS BVSValueHeaderId
                        , SUM(bvsv.ValueAmount) AS BaseValue
                  FROM  [Service.BaseValueSegment].[BVSValueHeader] bvsvh
                        INNER JOIN [Service.BaseValueSegment].[BVSValue] bvsv
                          ON  bvsv.BVSValueHeaderId = bvsvh.Id
              GROUP BY  bvsvh.BVSTranId
                        , bvsvh.Id
        )
        UPDATE #drv
           SET    BVSValueHeaderId = val.BVSValueHeaderId
                , TotalBaseValue = val.BaseValue
          FROM  val
         WHERE  val.BVSTranId = #drv.BVSTranId

        INSERT INTO #owners (BVSTranId, BVSOwnerId, LegalPartyRoleId, Pct, BaseValue)
              SELECT    drv.BVSTranId
                      , bvso.Id
                      , bvso.LegalPartyRoleId
                      , bvso.BIPercent
                      , FLOOR( drv.TotalBaseValue * ( bvso.BIPercent * 0.01 ) )
                FROM  #drv drv
                      INNER JOIN [Service.BaseValueSegment].[BVSOwner] bvso
                        ON  bvso.BVSTranId = drv.BVSTranId

        ;WITH lp AS
        (
            SELECT  o.Id
                    , lp.DisplayName
              FROM  #drv drv
                    INNER JOIN #owners o
                      ON  o.BVSTranId = drv.BVSTranId
                    INNER JOIN LegalPartyRole lpr
                      ON  lpr.Id = o.LegalPartyRoleId
                          AND lpr.BegEffDate = (SELECT MAX(sub.BegEffDate) FROM LegalPartyRole sub WHERE sub.Id = lpr.Id AND DATEDIFF(DAY, sub.BegEffDate, drv.AsOf) >= 0)
                    INNER JOIN LegalParty lp
                      ON  lp.Id = lpr.LegalPartyId
        )
        UPDATE  #owners
           SET  DisplayName = lp.DisplayName
          FROM  lp
         WHERE  lp.Id = #owners.Id

        ;WITH val AS
        (
            SELECT    o.BVSTranId
                    , SUM(o.BaseValue) AS BaseValue
              FROM  #owners o
          GROUP BY  o.BVSTranId
        )
        UPDATE  #drv
           SET  CalcBaseValue = val.BaseValue
          FROM  val
         WHERE  val.BVSTranId = #drv.BVSTranId

        ;WITH val AS
        (
              SELECT    o.Id
                      , drv.TotalBaseValue - drv.CalcBaseValue AS Diff
                      , RANK() OVER (PARTITION BY drv.BVSTranId
                                          ORDER BY drv.BVSTranId
                                                , o.DisplayName
                                                , o.Pct DESC
                                                , o.LegalPartyRoleId
                                    ) AS Seq
                FROM  #drv drv
                      INNER JOIN #owners o
                        ON  o.BVSTranId = drv.BVSTranId
        )
        UPDATE  #owners
            SET  BaseValueAdjusted = BaseValue + IIF(val.Seq <= val.Diff, 1, 0)
          FROM  val
         WHERE  val.Id = #owners.Id

        INSERT INTO [Service.BaseValueSegment].[BVSOwnerValue] (BVSOwnerId, BVSValueHeaderId, BaseValue, DynCalcStepTrackingId)
              SELECT    own.BVSOwnerId
                      , drv.BVSValueHeaderId
                      , own.BaseValueAdjusted
                      , 0
                FROM  #drv drv
                      INNER JOIN #owners own
                        ON  own.BVSTranId = drv.BVSTranId
               WHERE  drv.BVSValueHeaderId IS NOT NULL
                      AND own.BaseValueAdjusted IS NOT NULL
                      

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSOwnerValue records added'
    END TRY  
    BEGIN CATCH  
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR (@ErrorMessage, 11, 1)
    END CATCH  

    DROP TABLE #drv
    DROP TABLE #owners

--ROLLBACK TRANSACTION