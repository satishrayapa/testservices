using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class SeedOriginalBVSValueHeaderId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(@"

            IF OBJECT_ID( 'tempdb..#drv', 'u' ) IS NOT NULL DROP TABLE #drv
            IF OBJECT_ID( 'tempdb..#upd', 'u' ) IS NOT NULL DROP TABLE #upd

            CREATE TABLE #drv (
                RevObjId          INT NOT NULL
              , AsOf             DATE NOT NULL
              , SequenceNumber    INT NOT NULL
              , GRMEventId        INT  NULL
              , BVSValueHeaderId  INT NOT NULL
              , BVSTranTypeId     INT NOT NULL
              , VHSeq             INT NOT NULL )

            CREATE TABLE #upd (
                Id                INT NOT NULL
              , OriginalId        INT NOT NULL)

            INSERT INTO #drv (RevObjId, AsOf, SequenceNumber, GRMEventId, BVSValueHeaderId, BVSTranTypeId, VHSeq)
            SELECT bvs.RevObjId, bvs.AsOf, bvs.SequenceNumber, bvsvh.GRMEventId, bvsvh.Id
              , bvst.BVSTranTypeId
              , ROW_NUMBER() OVER (PARTITION BY bvs.RevObjId, bvsvh.GRMEventId ORDER BY bvs.RevObjId, bvsvh.GRMEventId, bvs.AsOf, bvs.SequenceNumber, bvst.Id, bvsvh.Id) VHSeq
            FROM [Service.BaseValueSegment].BVSValueHeader bvsvh
              JOIN [Service.BaseValueSegment].BVSTran bvst on bvst.Id = bvsvh.BVSTranId
              JOIN [Service.BaseValueSegment].BVS bvs on bvs.Id= bvst.BVSId
            WHERE bvs.RevObjId IN (SELECT sub.RevObjId 
                                      FROM [Service.BaseValueSegment].BVSTran subt 
                                      JOIN [Service.BaseValueSegment].BVS sub
                                      ON sub.Id = subt.BVSId
                                    WHERE subt.BVSTranTypeId != 3)

            INSERT INTO #upd(Id, OriginalId)
            select bvsvh.Id, Orig_Conv.BVSValueHeaderId
  
              FROM  [Service.BaseValueSegment].BVSValueHeader bvsvh
                    JOIN [Service.BaseValueSegment].BVSTran bvst
                      ON bvst.Id = bvsvh.BVSTranId
                    JOIN #drv curr 
                      ON  curr.BVSValueHeaderId = bvsvh.Id
                      AND curr.VHSeq > 1
                      AND curr.BVSTranTypeId != 3 -- conversion
                    JOIN #drv Orig_Conv -- GET MAX CONVERTED ROW PRIOR TO THIS BVS WITH SAME GRMEVENTID IF ONE EXISTS
                      ON  Orig_Conv.RevObjId=curr.RevObjId
                      AND Orig_Conv.GRMEventId=curr.grmeventId
                      AND Orig_Conv.BVSTranTypeId = 3 -- conversion
                      AND Orig_Conv.VHSeq = (SELECT MAX(sub.VHSeq) 
                                                FROM #drv sub 
                                              WHERE sub.RevObjId = Orig_Conv.RevObjId 
                                                AND sub.GRMEventId = Orig_Conv.GRMEventId 
                                                AND Orig_Conv.BVSTranTypeId = 3 
                                                AND (
                                                      sub.AsOf < curr.AsOf
                                                      OR (sub.AsOf = curr.AsOf AND sub.SequenceNumber < curr.SequenceNumber)
                                                    )
                                            )
            WHERE bvst.BVSTranTypeId != 3

            UPDATE bvsvh
            SET OriginalBVSValueHeaderId = upd.OriginalId
            FROM [Service.BaseValueSegment].BVSValueHeader bvsvh
              JOIN #upd upd ON upd.Id = bvsvh.Id

            TRUNCATE TABLE #upd

            INSERT INTO #upd(Id, OriginalId)
            select bvsvh.Id, Orig_NonConv.BVSValueHeaderId
            FROM  [Service.BaseValueSegment].BVSValueHeader bvsvh
                  JOIN [Service.BaseValueSegment].BVSTran bvst
                    ON bvst.Id = bvsvh.BVSTranId
                  JOIN #drv curr 
                    ON  curr.BVSValueHeaderId = bvsvh.Id
                    AND curr.VHSeq > 1
                    AND curr.BVSTranTypeId != 3 -- conversion
                  JOIN #drv Orig_NonConv -- GET FIRST NON-CONVERTED ROW FOR THIS GRMEVENTID IF ONE EXISTS
                    ON  Orig_NonConv.RevObjId=curr.RevObjId
                    AND Orig_NonConv.GRMEventId=curr.grmeventId
                    AND Orig_NonConv.VHSeq = 1
            WHERE bvst.BVSTranTypeId != 3
            AND bvsvh.OriginalBVSValueHeaderId IS NULL

            UPDATE bvsvh
            SET OriginalBVSValueHeaderId = upd.OriginalId
            FROM [Service.BaseValueSegment].BVSValueHeader bvsvh
              JOIN #upd upd ON upd.Id = bvsvh.Id
    			", true);

    }

    protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(@"
				    UPDATE [Service.BaseValueSegment].BVSValueHeader
            SET OriginalBVSValueHeaderId = NULL
            WHERE OriginalBVSValueHeaderId IS NOT NULL
			    ", true);

    }
  }
}
