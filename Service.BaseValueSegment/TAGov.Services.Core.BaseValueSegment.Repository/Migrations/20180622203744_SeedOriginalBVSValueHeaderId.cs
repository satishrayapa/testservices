using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Migrations
{
    public partial class SeedOriginalBVSValueHeaderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*
           // Moved to be seeded in next migration with performance improvements.
	        migrationBuilder.Sql(@"

            ;WITH upd AS (
              SELECT bvs.RevObjId, bvs.AsOf, bvs.SequenceNumber, bvsvh.GRMEventId, bvsvh.Id
                , bvst.BVSTranTypeId
                , ROW_NUMBER() OVER (PARTITION BY bvs.RevObjId, bvsvh.GRMEventId ORDER BY bvs.RevObjId, bvsvh.GRMEventId, bvs.AsOf, bvs.SequenceNumber, bvst.Id, bvsvh.Id) VHSeq
              FROM [Service.BaseValueSegment].BVSValueHeader bvsvh
                JOIN [Service.BaseValueSegment].BVSTran bvst on bvst.Id = bvsvh.BVSTranId
                JOIN [Service.BaseValueSegment].BVS bvs on bvs.Id= bvst.BVSId
            )
            UPDATE  bvsvh
               SET  bvsvh.OriginalBVSValueHeaderId = COALESCE(Orig_Conv.Id, Orig_NonConv.Id)
              FROM  [Service.BaseValueSegment].BVSValueHeader bvsvh
                    JOIN upd curr 
                      ON  curr.Id = bvsvh.Id
                      AND curr.VHSeq > 1
                      AND curr.BVSTranTypeId != 3 -- conversion
                    LEFT JOIN upd Orig_Conv -- GET MAX CONVERTED ROW PRIOR TO THIS BVS WITH SAME GRMEVENTID IF ONE EXISTS
                      ON  Orig_Conv.RevObjId=curr.RevObjId
                      AND Orig_Conv.GRMEventId=curr.grmeventId
                      AND Orig_Conv.BVSTranTypeId = 3 -- conversion
                      AND Orig_Conv.VHSeq = (SELECT MAX(sub.VHSeq) 
                                               FROM upd sub 
                                              WHERE sub.RevObjId = Orig_Conv.RevObjId 
                                                AND sub.GRMEventId = Orig_Conv.GRMEventId 
                                                AND Orig_Conv.BVSTranTypeId = 3 
                                                AND (
                                                      sub.AsOf < curr.AsOf
                                                      OR (sub.AsOf = curr.AsOf AND sub.SequenceNumber < curr.SequenceNumber)
                                                    )
                                            )
                    LEFT JOIN upd Orig_NonConv -- GET FIRST NON-CONVERTED ROW FOR THIS GRMEVENTID IF ONE EXISTS
                      ON  Orig_NonConv.RevObjId=curr.RevObjId
                      AND Orig_NonConv.GRMEventId=curr.grmeventId
                      AND Orig_NonConv.VHSeq = 1

    			", true);
          */
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
