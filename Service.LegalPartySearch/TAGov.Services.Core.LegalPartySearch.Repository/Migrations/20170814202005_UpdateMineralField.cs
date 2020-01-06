using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class UpdateMineralField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch",
                nullable: true,
                oldClrType: typeof(bool));
			/*
			migrationBuilder.Sql(@"	DECLARE @p_EffDate datetime
									DECLARE @p_EffStatusFilter char(1)
									DECLARE @EffYear SMALLINT

									SET @p_EffDate = '12/31/9999'
									--SET @p_EffStatusFilter = 'A'
									SET @EffYear = DATEPART(YEAR, @p_EffDate)



									if object_ID('tempdb..#sysType') is not null DROP TABLE #sysType

									select st.id, st.begEffDate, st.descr, st.effStatus
									into #sysType
									from dbo.systype as st
									join
									(
										select id, max(begEffDate) as begEffDate
										from SysType
										where begEffDate <= @p_EffDate
										group by id
									) as flt on flt.id = st.id AND flt.begEffDate = st.begEffDate


									CREATE NONCLUSTERED INDEX NC_IX_ID on #sysType (id asc) include (begEffDate, descr, effStatus)


									INSERT INTO[search].[LegalPartySearch]([LegalPartyRoleId], [LegalPartyId], [LegalPartyRole], [BegEffDate], [EffStatus], [DisplayName]
																			,[Addr], [PIN], [AIN], [SearchDoc], [SearchPin], [SearchGeoTag], [RevObjId], [RevObjBegEffDate]
																			,[GeoCode], [AddrId], [AddrBegEffDate], [AddrRoleId], [AddrRoleBegEffDate], [AddrType], [AddrUnitNumber]
																			,[AddrStreetNumber], [AddrStreetName], [Tag], [TagId], [TagBegEffYear], [TagRoleId], [TagRoleBegEffDate], [LegalPartyType]
																			,[LegalPartySubType], [LegalPartyTypeId], [LegalPartySubTypeId], [AppraisalSiteId], [AppraisalBegEffDate], [AppraisalEffStatus]
																			,[AppraisalRoleId], [AppraisalRoleBegEffDate], [AppraisalRoleEffStatus], [AppraisalSiteName], [AppraisalClass], [Neighborhood], [Mineral])
									SELECT lpr.[Id]
										,[LegalPartyId] 
										,lprole.descr as LegalPartyRole
										,lpr.BegEffDate
										,lpr.EffStatus
										,lp.DisplayName
										,c.DeliveryAddr as Addr
										,COALESCE(RTRIM(ro.PIN),'') as PIN
										,COALESCE(RTRIM(ro.AIN),'') as AIN
										,RTRIM(lp.DisplayName + ' ' + c.DeliveryAddr) as SearchDoc	   
										,RTRIM(COALESCE(RTRIM(ro.PIN),'') + ' ' + COALESCE(RTRIM(ro.AIN),'')) as SearchPin
										,RTRIM(COALESCE(RTRIM(ro.GeoCd),'') + ' ' + COALESCE(RTRIM(t.Descr),'')) as SearchGeoTag
										,ro.Id as RevObjId
										,ro.BegEffDate as RevObjBegEffDate
										,COALESCE(RTRIM(ro.GeoCd),'') as GeoCode
										,c.Id as commId
										,c.BegEffDate as AddrBegEffDate
										,cr.Id as AddrRoleId
										,cr.BegEffDate as AddrRoleBegEffDate
										,'Mailing' as AddrType
										,'' as AddrUnitNumber
										,c.PrimeAddrNumber as AddrStreetNumber
										,RTRIM(c.StreetName) as AddStreetName
										,COALESCE(RTRIM(t.Descr),'') as Tag
										,t.Id as TagId
										,t.BegEffYear as TagBegEffYear
										,tr.Id as TagRoleId
										,tr.BegEffDate as TagRoleBegEffDate
										,lpType.descr as LegalPartyType
										,lpSType.descr as LegalPartySubType
										,lp.LegalPartyType as LegalPartyTypeId
										,lp.LPSubType as LegalPartySubTypeId
										,aps.Id as AppraisalSiteId
										,aps.BegEffDate as AppraisalBegEffDate
										,aps.EffStatus as AppraisalEffStatus
										,asr.Id as AppraisalRoleId
										,asr.BegEffDate as AppraisalRoleBegEffDate
										,asr.EffStatus as AppraisalRoleEffStatus
										,aps.ApplSiteName as AppraisalSiteName
										,dbo.STLongDescr(aps.ApplClassCd)  AS 'AppraisalClass'
										,dbo.STLongDescr(aps.NbhdCd) AS 'Neighborhood'
										,CASE WHEN mn.nb is null then null when mn.nb = 0 then 0 ELSE 1 END as Mineral
									FROM dbo.LegalPartyRole as lpr
									JOIN (select id, max(begEffDate) as begEffDate from dbo.LegalPartyRole where begEffDate <= @p_EffDate group by id) as fltLpr on fltLpr.id = lpr.id AND fltlpr.begEffDate = lpr.begEffDate
									JOIN #sysType as lprole on lprole.id = lpr.LPRoleType
									JOIN dbo.LegalParty as lp on lp.Id = lpr.LegalPartyId
									JOIN #sysType as lpType on lpType.id = lp.LegalPartyType
									JOIN #sysType as lpSType on lpStype.id = lp.LPSubType
									JOIN dbo.CommRole as cr on cr.ObjectType = 100001 and cr.ObjectId = lp.Id
									JOIN
									(
										select id, max(begEffDate) as begEffDate
										from dbo.CommRole
										where begEffDate<CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END
									  group by id
									) as fltCR on fltCR.id = cr.Id AND fltCR.begEffDate = cr.BegEffDate
									JOIN dbo.Comm as c on c.Id = cr.CommId
									JOIN
									(
										select id, max(begEffDate) as begEffDate
										from dbo.Comm
										where begEffDate<CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END
									  group by id
									) as fltComm on fltComm.id = c.Id AND fltComm.begEffDate = c.BegEffDate
									LEFT JOIN (select id, PIN, AIN, GeoCd, BegEffDate from dbo.grm_records_RevObjByEffDate(@p_EffDate, @p_EffStatusFilter) ) as ro on ro.Id = lpr.ObjectId and lpr.ObjectType = 100002 and lpr.LPRoleType = 100701
									LEFT JOIN (select id, begEffDate, objectType, objectId, TAGId from dbo.grm_records_TAGRoleByEffDate(@p_EffDate, @p_EffStatusFilter) where ObjectType = 100002) as tr on tr.ObjectId = ro.Id
									LEFT JOIN (select id, descr, BegEffYear from dbo.grm_levy_TAGByEffYear(@EffYear, @p_EffStatusFilter)) as t on t.Id = tr.TAGId
									LEFT JOIN (select Id, objectId, ObjectType, ApplSiteId, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteRoleByEffDate(@p_EffDate,'A') where ObjectType = 100002) as asr ON ro.Id = asr.ObjectId
									LEFT JOIN (select id, ApplSiteName, ApplClassCd, NbhdCd, ApplSiteType, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteByEffDate(@p_EffDate,'A') ) as aps ON asr.ApplSiteId = aps.Id
									LEFT JOIN
									(
										SELECT SysTypeId, COUNT(*) as nb
										FROM dbo.grm_FW_SysTypeIntelByEffDate(@p_EffDate, 'A')
										WHERE SysTypeCatIntId IN(270174, 270175) AND STIValue = 'True'
										group by SysTypeId
									) as mn on mn.SysTypeId = aps.ApplSiteType

									WHERE (@p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)
									  AND (@p_EffStatusFilter IS NULL OR cr.effStatus = @p_EffStatusFilter)
									  AND (@p_EffStatusFilter IS NULL OR c.effStatus = @p_EffStatusFilter)
									  AND (@p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
									  AND (@p_EffStatusFilter IS NULL OR lpType.effStatus = @p_EffStatusFilter)
									  AND (@p_EffStatusFilter IS NULL OR lpSType.effStatus = @p_EffStatusFilter)");
			*/
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Mineral",
                schema: "search",
                table: "LegalPartySearch",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
