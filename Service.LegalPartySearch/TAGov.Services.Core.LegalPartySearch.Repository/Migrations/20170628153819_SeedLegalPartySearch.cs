using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class SeedLegalPartySearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
//	        migrationBuilder.Sql(@"
//IF ((SELECT COUNT(1) FROM [search].[LegalPartySearch])=0)
//BEGIN
//		DECLARE @p_EffDate datetime
//		DECLARE @p_EffStatusFilter char(1)

//		SET @p_EffDate = '12/31/9999'
//		INSERT INTO [search].[LegalPartySearch]
//				   ([LegalPartyRoleId]
//				   ,[LegalPartyId]
//				   ,[LegalPartyRole]
//				   ,[BegEffDate]
//				   ,[EffStatus]
//				   ,[DisplayName]
//				   ,[Addr]
//				   ,PIN
//				   ,AIN
//				   ,[SearchDoc]
//				   ,[SearchPin]
//				   ,[RevObjId]
//				   ,[RevObjBegEffDate]
//				   ,[AddrId]
//				   ,[AddrBegEffDate]
//				   ,[AddrRoleId]
//				   ,[AddrRoleBegEffDate]
//				   ,[AddrType])
//			 SELECT lpr.[Id]
//			   ,[LegalPartyId]      
//			   ,lprole.descr as LegalPartyRole
//			   ,lpr.BegEffDate
//			   ,lpr.EffStatus
//			   ,lp.DisplayName
//			   ,c.DeliveryAddr
//			   ,RTRIM(ro.PIN) as PIN
//			   ,RTRIM(ro.AIN) as AIN
//			   ,lp.DisplayName + ' ' + c.DeliveryAddr as SearchDoc
//			   ,COALESCE(RTRIM(ro.PIN),'') + ' ' + COALESCE(RTRIM(ro.AIN),'') as SearchPin
//			   ,ro.Id
//			   ,ro.BegEffDate
//			   ,c.Id
//			   ,c.BegEffDate
//			   ,cr.Id
//			   ,cr.BegEffDate
//			   ,'Mailing'
//		  FROM [dbo].[LegalPartyRole] as lpr
//		  inner join SysType as lprole on lprole.id = lpr.LPRoleType
//		  inner join LegalParty as lp on lp.Id = lpr.LegalPartyId
//		  inner join CommRole as cr on cr.ObjectType = 100001 and cr.ObjectId = lp.Id
//		  inner join Comm as c on c.Id = cr.CommId
//		  left join grm_records_RevObjByEffDate(@p_EffDate, @p_EffStatusFilter) ro
//					on ro.Id = lpr.ObjectId
//					and lpr.ObjectType = 100002 -- ObjectType.RevObj
//					and lpr.LPRoleType = 100701 -- LPRoleType.Owner
//		  where
//			LPR.begEffDate = ( select max(sub.begEffDate) from LegalPartyRole sub where sub.begEffDate <= @p_EffDate AND sub.Id = LPR.Id )
//				  AND   ( @p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)
//			AND cr.begEffDate = (select max(CRSUB.begEffDate) from CommRole CRSUB where CRSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND CRSUB.Id = cr.Id )  
//			  AND (@p_EffStatusFilter IS NULL OR cr.effStatus = @p_EffStatusFilter) 
//			  AND c.begEffDate = (select max(CRSUB.begEffDate) from Comm CRSUB where CRSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND CRSUB.Id = c.Id )  
//			  AND (@p_EffStatusFilter IS NULL OR c.effStatus = @p_EffStatusFilter) 
//			  AND lprole.begEffDate = ( select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lprole.Id )
//				  AND   ( @p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
//END
//			");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DELETE FROM [search].[LegalPartySearch]");
		}
    }
}
