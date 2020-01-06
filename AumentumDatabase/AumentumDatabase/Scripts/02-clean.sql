SET NOCOUNT ON

DECLARE @TAG VARCHAR(16) = '' --'018-333' for a specific TAG id--in this case 018-333--leaving TAG empty converts the whole database

-------------------
--BEGIN TRANSACTION

  CREATE TABLE #drv
  (   RevObjId    INT NOT NULL        )

  CREATE TABLE #bvs
  (   RevObjId    INT NOT NULL
    , BVSId       INT NOT NULL
    , BVSTranId   INT NOT NULL        )

  BEGIN TRY
      DECLARE @RevObj INT; EXEC grm_GetSysTypeId 'Object Type', 'RevObj', @RevObj OUTPUT

      INSERT INTO #drv (RevObjId)
            SELECT  ro.Id
              FROM  grm_levy_TAGByEffYear(9999, 'A') tag
                    INNER JOIN grm_records_TagRoleByEffDate('12/31/9999', 'A') tr
                      ON  tr.TAGId = tag.Id 
                          AND tr.ObjectType = @RevObj
                    INNER JOIN grm_records_RevObjByEffDate('12/31/9999', 'A') ro
                      ON  ro.Id = tr.ObjectId
              WHERE  LEN(ISNULL(@TAG, '')) = 0 OR tag.ShortDescr = @TAG 

      PRINT 'Found ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' RevObjs for TAG ' + @TAG

      INSERT INTO #bvs (RevObjId, BVSId, BVSTranId)
            SELECT    drv.RevObjId
                    , bvs.Id
                    , bvst.Id
              FROM  #drv drv
                    INNER JOIN [Service.BaseValueSegment].[BVS] bvs
                      ON  bvs.RevObjId = drv.RevObjId
                    INNER JOIN [Service.BaseValueSegment].[BVSTran] bvst
                      ON  bvst.BVSId = bvs.Id

      BEGIN -- AsmtRevnBVS
          DELETE 
            FROM  [Service.BaseValueSegment].[AsmtRevnBVS]
           WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSId = [Service.BaseValueSegment].[AsmtRevnBVS].BVSId )

          PRINT '  ' + CONVERT(VARCHAR(100), @@ROWCOUNT) + ' AsmtRevnBVS records deleted'
      END 

      BEGIN -- BVSOwnerValue
          ;WITH drv AS
          (
              SELECT  bvsvh.Id
                FROM  [Service.BaseValueSegment].[BVSValueHeader] bvsvh
               WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = bvsvh.BVSTranId )
          )
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSOwnerValue]
           WHERE  EXISTS( SELECT 1 FROM drv WHERE drv.Id = [Service.BaseValueSegment].[BVSOwnerValue].BVSValueHeaderId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSOwnerValue records deleted (by BVSValueHeader)'

          ;WITH drv AS
          (
              SELECT  bvso.Id
                FROM  [Service.BaseValueSegment].[BVSOwner] bvso
               WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = bvso.BVSTranId )
          )
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSOwnerValue]
           WHERE  EXISTS( SELECT 1 FROM drv WHERE drv.Id = [Service.BaseValueSegment].[BVSOwnerValue].BVSOwnerId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSOwnerValue records deleted (by BVSOwner)'
      END

      BEGIN -- BVSValue
          ;WITH drv AS
          (
              SELECT  bvsvh.Id
                FROM  [Service.BaseValueSegment].[BVSValueHeader] bvsvh
               WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = bvsvh.BVSTranId )
          )
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSValue]
           WHERE  EXISTS( SELECT 1 FROM drv WHERE drv.Id = [Service.BaseValueSegment].[BVSValue].BVSValueHeaderId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSValue records deleted'
      END

      BEGIN -- BVSValueHeader
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSValueHeader]
           WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = [Service.BaseValueSegment].[BVSValueHeader].BVSTranId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSValueHeader records deleted'
      END

      BEGIN -- BVSOwner
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSOwner]
           WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = [Service.BaseValueSegment].[BVSOwner].BVSTranId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSOwner records deleted'
      END

      BEGIN -- BVSTran
          DELETE 
            FROM  [Service.BaseValueSegment].[BVSTran]
           WHERE  EXISTS( SELECT 1 FROM #bvs bvs WHERE bvs.BVSTranId = [Service.BaseValueSegment].[BVSTran].Id )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVSTran records deleted'
      END

      BEGIN -- BVS
          DELETE 
            FROM  [Service.BaseValueSegment].[BVS]
           WHERE  EXISTS( SELECT 1 FROM #drv drv WHERE drv.RevObjId = [Service.BaseValueSegment].[BVS].RevObjId )

          PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' BVS records deleted'
      END
  END TRY  
  BEGIN CATCH  
      DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
      RAISERROR (@ErrorMessage, 11, 1)
  END CATCH  

  DROP TABLE #drv
  DROP TABLE #bvs

--ROLLBACK TRANSACTION