using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class SeedLegalPartySearch3 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
//			migrationBuilder.Sql(@"
//TRUNCATE TABLE [search].[LegalPartySearch]

//DECLARE @p_EffDate datetime
//DECLARE @p_EffStatusFilter char(1)
//DECLARE @EffYear SMALLINT 

//SET @p_EffDate = '12/31/9999'
//--SET @p_EffStatusFilter = 'A'
//SET @EffYear = DATEPART(YEAR, @p_EffDate)

//INSERT INTO [search].[LegalPartySearch]
//           ([LegalPartyRoleId]
//           ,[LegalPartyId]
//           ,[LegalPartyRole]
//		   ,[BegEffDate]
//		   ,[EffStatus]
//           ,[DisplayName]
//           ,[Addr]
//		   ,PIN
//		   ,AIN
//           ,[SearchDoc]
//		   ,[SearchPin]
//		   ,[SearchGeoTag]
//		   ,[RevObjId]
//		   ,[RevObjBegEffDate]
//		   ,[GeoCode]
//		   ,[AddrId]
//		   ,[AddrBegEffDate]
//		   ,[AddrRoleId]
//		   ,[AddrRoleBegEffDate]
//		   ,[AddrType]
//		   ,[AddrUnitNumber]
//		   ,[AddrStreetNumber]
//		   ,[AddrStreetName]
//		   ,[Tag]
//		   ,[TagId]
//		   ,[TagBegEffYear]
//		   ,[TagRoleId]
//		   ,[TagRoleBegEffDate]
//		   ,[LegalPartyType]
//		   ,[LegalPartySubType]
//		   ,[LegalPartyTypeId]
//		   ,[LegalPartySubTypeId]
//		   ,[AppraisalSiteId]
//		   ,[AppraisalBegEffDate]
//	       ,[AppraisalEffStatus]
//	       ,[AppraisalRoleId]
//	       ,[AppraisalRoleBegEffDate]
//	       ,[AppraisalRoleEffStatus]
//	       ,[AppraisalSiteName]
//	       ,[AppraisalClass]
//	       ,[Neighborhood])
//     SELECT lpr.[Id]
//       ,[LegalPartyId] 
//	   ,lprole.descr as LegalPartyRole
//	   ,lpr.BegEffDate
//	   ,lpr.EffStatus
//	   ,lp.DisplayName
//	   ,c.DeliveryAddr as Addr
//	   ,COALESCE(RTRIM(ro.PIN),'') as PIN
//	   ,COALESCE(RTRIM(ro.AIN),'') as AIN
//	   ,RTRIM(lp.DisplayName + ' ' + c.DeliveryAddr) as SearchDoc	   
//	   ,RTRIM(COALESCE(RTRIM(ro.PIN),'') + ' ' + COALESCE(RTRIM(ro.AIN),'')) as SearchPin
//	   ,RTRIM(COALESCE(RTRIM(ro.GeoCd),'') + ' ' + COALESCE(RTRIM(t.Descr),'')) as SearchGeoTag
//	   ,COALESCE(ro.Id,NULL) as RevObjId
//	   ,COALESCE(ro.BegEffDate,NULL) as RevObjBegEffDate
//	   ,COALESCE(RTRIM(ro.GeoCd),'') as GeoCd
//	   ,c.Id
//	   ,c.BegEffDate
//	   ,cr.Id
//	   ,cr.BegEffDate
//	   ,'Mailing'
//	   ,'' as AddrUnitNumber
//	   ,c.PrimeAddrNumber as AddrStreetNumber
//	   ,RTRIM(c.StreetName) as AddStreetName
//	   ,COALESCE(RTRIM(t.Descr),'') as Tag
//	   ,COALESCE(t.Id,NULL) as TagId
//	   ,COALESCE(t.BegEffYear,NULL) as TagBegEffYear
//	   ,COALESCE(tr.Id,NULL) as TagRoleId
//	   ,COALESCE(tr.BegEffDate,NULL) as TagRoleBegEffDate
//	   ,lpType.descr as LegalPartyType
//	   ,lpSType.descr as LegalPartySubType
//	   ,lp.LegalPartyType as LegalPartyTypeId
//	   ,lp.LPSubType as LegalPartySubTypeId
//	   ,aps.Id as AppraisalSiteId
//	   ,aps.BegEffDate as AppraisalBegEffDate
//	   ,aps.EffStatus as AppraisalEffStatus
//	   ,asr.Id as AppraisalRoleId
//	   ,asr.BegEffDate as AppraisalBegEffDate
//	   ,asr.EffStatus as AppraisalEffStatus
//	   ,COALESCE(aps.ApplSiteName,NULL) as AppraisalSiteName
//	   ,COALESCE(dbo.STLongDescr(aps.ApplClassCd),NULL) AS 'AppraisalClass'
//	   ,COALESCE(dbo.STLongDescr(aps.NbhdCd),NULL) AS 'Neighborhood'
//  FROM [dbo].[LegalPartyRole] as lpr
//  inner join SysType as lprole on lprole.id = lpr.LPRoleType
//  inner join LegalParty as lp on lp.Id = lpr.LegalPartyId
//  inner join CommRole as cr on cr.ObjectType = 100001 and cr.ObjectId = lp.Id
//  inner join Comm as c on c.Id = cr.CommId
//  inner join SysType as lpType on lpType.id = lp.LegalPartyType
//  inner join SysType as lpSType on lpStype.id = lp.LPSubType
//  left join grm_records_RevObjByEffDate(@p_EffDate, @p_EffStatusFilter) ro
//            on ro.Id = lpr.ObjectId
//            and lpr.ObjectType = 100002 -- ObjectType.RevObj
//            and lpr.LPRoleType = 100701 -- LPRoleType.Owner
//  left join grm_records_TAGRoleByEffDate(@p_EffDate, @p_EffStatusFilter) as tr on tr.ObjectType = 100002
//			and tr.ObjectId = ro.Id
//  left join grm_levy_TAGByEffYear(@EffYear, @p_EffStatusFilter) as t on t.Id = tr.TAGId
//  LEFT JOIN GRM_RPA_ApplSiteRoleByEffDate(@p_EffDate,'A') asr
//            ON ro.Id = asr.ObjectId 
//				AND asr.ObjectType = 100002 -- RevObj 
//	        LEFT JOIN GRM_RPA_ApplSiteByEffDate(@p_EffDate, 'A') aps
//		        ON asr.ApplSiteId = aps.Id
//	        where

//	        LPR.begEffDate = (select max(sub.begEffDate) from LegalPartyRole sub where sub.begEffDate <= @p_EffDate AND sub.Id = LPR.Id)

//	        AND(@p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)

//	        AND cr.begEffDate = (select max(CRSUB.begEffDate) from CommRole CRSUB where CRSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND CRSUB.Id = cr.Id )  
//	        AND(@p_EffStatusFilter IS NULL OR cr.effStatus = @p_EffStatusFilter)
//	        AND c.begEffDate = (select max(CRSUB.begEffDate) from Comm CRSUB where CRSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND CRSUB.Id = c.Id )  
//	        AND(@p_EffStatusFilter IS NULL OR c.effStatus = @p_EffStatusFilter)
//	        AND lprole.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lprole.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
//	        AND lpType.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpType.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lpType.effStatus = @p_EffStatusFilter)
//	        AND lpSType.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpSType.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lpSType.effStatus = @p_EffStatusFilter)

//INSERT INTO [search].[LegalPartySearch]
//           ([LegalPartyRoleId]
//           ,[LegalPartyId]
//           ,[LegalPartyRole]
//		   ,[BegEffDate]
//		   ,[EffStatus]
//           ,[DisplayName]
//           ,[Addr]
//		   ,PIN
//		   ,AIN
//           ,[SearchDoc]
//		   ,[SearchPin]
//		   ,[SearchGeoTag]
//		   ,[RevObjId]
//		   ,[RevObjBegEffDate]
//		   ,[AddrId]
//		   ,[AddrBegEffDate]
//		   ,[AddrRoleId]
//		   ,[AddrRoleBegEffDate]
//		   ,[AddrType]
//		   ,[AddrUnitNumber]
//		   ,[AddrStreetNumber]
//		   ,[AddrStreetName]
//		   ,[Tag]
//		   ,[TagId]
//		   ,[TagBegEffYear]
//		   ,[TagRoleId]
//		   ,[TagRoleBegEffDate]
//		   ,[LegalPartyType]
//		   ,[LegalPartySubType]
//		   ,[LegalPartyTypeId]
//		   ,[LegalPartySubTypeId]
//		   ,[AppraisalSiteId]
//		   ,[AppraisalBegEffDate]
//	       ,[AppraisalEffStatus]
//	       ,[AppraisalRoleId]
//	       ,[AppraisalRoleBegEffDate]
//	       ,[AppraisalRoleEffStatus]
//	       ,[AppraisalSiteName]
//	       ,[AppraisalClass]
//	       ,[Neighborhood])
//SELECT lpr.[Id]
//       ,[LegalPartyId]      
//	   ,lprole.descr as LegalPartyRole
//	   ,lpr.BegEffDate
//	   ,lpr.EffStatus
//	   ,lp.DisplayName
//	   ,RTRIM(sa.StreetNumber) + ' ' + RTRIM(sa.StreetName) as SitusAddress
//	   ,RTRIM(ro.PIN) as PIN
//	   ,RTRIM(ro.AIN) as AIN
//	   ,RTRIM(lp.DisplayName + ' ' + RTRIM(sa.StreetNumber) + ' ' + RTRIM(sa.StreetName)) as SearchDoc
//	   ,RTRIM(COALESCE(RTRIM(ro.PIN),'') + ' ' + COALESCE(RTRIM(ro.AIN),'')) as SearchPin
//	   ,RTRIM(COALESCE(RTRIM(ro.GeoCd),'') + ' ' + COALESCE(RTRIM(t.Descr),'')) as SearchGeoTag
//	   ,ro.Id
//	   ,ro.BegEffDate
//	   ,sa.Id
//	   ,sa.BegEffDate
//	   ,sar.Id
//	   ,sar.BegEffDate
//	   ,'Situs'
//	   ,RTRIM(sa.UnitNumber)
//	   ,sa.StreetNumber
//	   ,RTRIM(sa.StreetName)
//	   ,COALESCE(RTRIM(t.Descr),'') as Tag
//	   ,COALESCE(t.Id,NULL) as TagId
//	   ,COALESCE(t.BegEffYear,NULL) as TagBegEffYear
//	   ,COALESCE(tr.Id,NULL) as TagRoleId
//	   ,COALESCE(tr.BegEffDate,NULL) as TagRoleBegEffDate
//	   ,lpType.descr as LegalPartyType
//	   ,lpSType.descr as LegalPartySubType
//	   ,lp.LegalPartyType as LegalPartyTypeId
//	   ,lp.LPSubType as LegalPartySubTypeId
//	   ,aps.Id as AppraisalSiteId
//	   ,aps.BegEffDate as AppraisalBegEffDate
//	   ,aps.EffStatus as AppraisalEffStatus
//	   ,asr.Id as AppraisalRoleId
//	   ,asr.BegEffDate as AppraisalBegEffDate
//	   ,asr.EffStatus as AppraisalEffStatus
//	   ,COALESCE(aps.ApplSiteName,NULL) as AppraisalSiteName
//	   ,COALESCE(dbo.STLongDescr(aps.ApplClassCd),NULL) AS 'AppraisalClass'
//	   ,COALESCE(dbo.STLongDescr(aps.NbhdCd),NULL) AS 'Neighborhood'
//  FROM [dbo].[LegalPartyRole] as lpr
//  inner join SysType as lprole on lprole.id = lpr.LPRoleType
//  inner join LegalParty as lp on lp.Id = lpr.LegalPartyId
//  inner join grm_records_RevObjByEffDate(@p_EffDate, @p_EffStatusFilter) ro
//            on ro.Id = lpr.ObjectId
//            and lpr.ObjectType = 100002 -- ObjectType.RevObj
//            and lpr.LPRoleType = 100701 -- LPRoleType.Owner
//  inner join SitusAddrRole as sar on sar.ObjectType = 100002 and sar.ObjectId = ro.Id
//  inner join SitusAddr as sa on sa.Id = sar.SitusAddrId
//  inner join SysType as lpType on lpType.id = lp.LegalPartyType
//  inner join SysType as lpSType on lpStype.id = lp.LPSubType
//  left join grm_records_TAGRoleByEffDate(@p_EffDate, @p_EffStatusFilter) as tr on tr.ObjectType = 100002
//			and tr.ObjectId = ro.Id
//  left join grm_levy_TAGByEffYear(@EffYear, @p_EffStatusFilter) as t on t.Id = tr.TAGId
//  LEFT JOIN GRM_RPA_ApplSiteRoleByEffDate(@p_EffDate,'A') asr
//            ON ro.Id = asr.ObjectId 
//				AND asr.ObjectType = 100002 -- RevObj 
//	        LEFT JOIN GRM_RPA_ApplSiteByEffDate(@p_EffDate, 'A') aps
//		        ON asr.ApplSiteId = aps.Id
//	        where

//	        LPR.begEffDate = (select max(sub.begEffDate) from LegalPartyRole sub where sub.begEffDate <= @p_EffDate AND sub.Id = LPR.Id)

//	        AND(@p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)

//	        AND sar.begEffDate = (select max(SARSUB.begEffDate) from SitusAddrRole SARSUB where SARSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND SARSUB.Id = sar.Id )  
//	        AND(@p_EffStatusFilter IS NULL OR sar.effStatus = @p_EffStatusFilter)
//	        AND sa.begEffDate = (select max(SARSUB.begEffDate) from SitusAddr SARSUB where SARSUB.begEffDate < CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END AND SARSUB.Id = sa.Id )  
//	        AND(@p_EffStatusFilter IS NULL OR sa.effStatus = @p_EffStatusFilter)
//	        AND lprole.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lprole.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
//	        AND lpType.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpType.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lpType.effStatus = @p_EffStatusFilter)
//	        AND lpSType.begEffDate = (select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpSType.Id )
//	        AND(@p_EffStatusFilter IS NULL OR lpSType.effStatus = @p_EffStatusFilter)");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("TRUNCATE TABLE [search].[LegalPartySearch]");
		}
	}
}
