namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public static class UpdatedSeedQuery
	{

		private const string CrawlSql = @"DECLARE @IdxCount int
														DECLARE @Total int
														SELECT
														@IdxCount = FULLTEXTCATALOGPROPERTY(cat.name,'ItemCount')
														FROM sys.fulltext_catalogs AS cat
														select @Total=count(1) from [search].[legalpartysearch]
											            DECLARE @Perc decimal(18,2) = @IdxCount/@Total * 100
														Select @IdxCount 'IndexRows', @Total 'TotalRows', Cast(@Perc as varchar(20)) + '% completed' AS 'Status'";

		private const string DisableIndex = "alter fulltext index on [search].[LegalPartySearch] disable ";
		private const string EnableIndex = "alter fulltext index on [search].[LegalPartySearch] enable ";
		public const string ExecuteSql = @"if not exists ( select 1 from sys.objects where name = 'SimpleSearchSeed_Log' and schema_id = schema_id('search') )
  create table search.SimpleSearchSeed_Log
  ( Id int identity primary key, StartTime datetime, StatusTime datetime default getdate(), StatusMessage nvarchar(256),
    ElapsedTime as
      cast(cast(datediff(second,StartTime,StatusTime) as int) / 60 as varchar) + ' minutes ' +
      cast(cast(datediff(second,StartTime,StatusTime) as int) % 60 as varchar) + ' seconds '
  )
--go

set ansi_nulls on
--go
set quoted_identifier on
--go

declare @v_Message nvarchar(max)
-- TODO: Capturing @v_StartTime here rather than using getdate() in each insert so that all inserted rows will have the same timestamp.
-- This seemed more correct to me, but feel free to change this if desired.
declare @v_StartTime datetime = getdate()

declare @p_EffDate datetime = '9999-12-31'
declare @p_EffStatusFilter char(1)

declare @v_EffDate datetime = dbo.GetEndOfDay(@p_EffDate)
declare @v_EffStatusFilter char(1) = @p_EffStatusFilter

declare @v_EffYear smallint
declare @v_EarliestEffDate datetime = '1776-07-04 00:00:00.000'
set @v_EffYear = datepart(year, @v_EffDate)

declare @v_RowCount int = 0

declare @v_SQL nvarchar(max)

declare @t_Indexes table ( DisableSQL nvarchar(max), RebuildSQL nvarchar(max) )
insert into @t_Indexes
( DisableSQL, RebuildSQL )
select 'alter index ' + quotename(i.name) + ' on ' +  quotename(schema_name(t.schema_id))+'.'+ quotename(t.name) + ' disable',
  'alter index ' + quotename(i.name) + ' on ' +  quotename(schema_name(t.schema_id))+'.'+ quotename(t.name) + ' rebuild'
from sys.indexes i
join sys.tables t
  on i.object_id = t.object_id
  and quotename(schema_name(t.schema_id)) = '[search]'
  and quotename(t.name) = '[LegalPartySearch]'
where i.type_desc = 'nonclustered'
and i.name is not null
and i.is_disabled = 0

-- **************
-- ** #SysType **
-- **************
if object_id('tempdb..#SysType') is not null drop table #SysType
create table #SysType ( Id int, BegEffDate datetime, EffStatus char(1), Descr varchar(64) )

insert into #SysType
( Id, BegEffDate, EffStatus, Descr )
select st.id, st.begEffDate, st.effStatus, rtrim(st.descr)
from dbo.SysType as st
where st.BegEffDate = ( select max(BegEffDate) from SysType where Id = st.Id and BegEffDate <= @v_EffDate )
and ( @p_EffStatusFilter is null or st.effStatus = @p_EffStatusFilter )

create nonclustered index NC_IX_ID on #SysType (id asc) include (begEffDate, descr, effStatus)

-- ***************
-- ** #CommInfo **
-- ***************
if object_id('tempdb..#CommInfo') is not null drop table #CommInfo
create table #CommInfo
( CR_Id int, CR_BegEffDate datetime, CR_EffStatus char(1), CR_ObjectId int,
  Comm_Id int, Comm_BegEffDate datetime, Comm_Type int, Comm_TypeDescr varchar(64), Comm_PrimeAddrNumber int, Comm_StreetName varchar(64), Comm_StreetType varchar(8),
  Comm_DeliveryAddr varchar(64), Comm_City varchar(32), Comm_State varchar(16), Comm_PostalCd varchar(16)
)

insert into #CommInfo
( CR_Id, CR_BegEffDate, CR_EffStatus, CR_ObjectId, Comm_Id, Comm_BegEffDate, Comm_Type, Comm_TypeDescr,
  Comm_PrimeAddrNumber, Comm_StreetName, Comm_StreetType, Comm_DeliveryAddr, Comm_City, Comm_State, Comm_PostalCd
)
select cr.Id, cr.BegEffDate, cr.EffStatus, cr.ObjectId, cr.CommId, c.BegEffDate, c.CommType, st.descr, c.PrimeAddrNumber, rtrim(c.StreetName),
  rtrim(c.StreetType), rtrim(c.DeliveryAddr), rtrim(c.City), rtrim(c.state), rtrim(c.PostalCd)
from CommRole cr
join Comm c
  on c.Id = cr.CommId
  and c.CommType = 101850 -- MailingAddress
  and c.BegEffDate = ( select max(BegEffDate) from Comm where Id = c.Id and BegEffDate <= @v_EffDate )
  and ( @p_EffStatusFilter is null or c.effStatus = @p_EffStatusFilter )
left join #SysType st
  on st.Id = c.CommType
where cr.ObjectType = 100001 -- LegalParty
and cr.BegEffDate = ( select max(BegEffDate) from CommRole where Id = cr.Id and BegEffDate <= @v_EffDate )
and ( @p_EffStatusFilter is null or cr.effStatus = @p_EffStatusFilter )

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 1 of 10: ' + format(@v_RowCount, '#,#') + ' CommRole/Comm rows captured.' )

create index #CommInfo_index1 on #CommInfo ( CR_ObjectId )
create index #CommInfo_index2 on #CommInfo ( Comm_Id )

-- *********************
-- ** #LegalPartyInfo **
-- *********************
if object_id('tempdb..#LegalPartyInfo') is not null drop table #LegalPartyInfo
create table #LegalPartyInfo
( LPR_Id int, LPR_BegEffDate datetime, LPR_EffStatus char(1), LPR_ObjectType int, LPR_ObjectId int,
  LPR_PrimeLegalParty smallint, LPR_LPRoleType int, LPR_LPRoleTypeDescr varchar(64),
  LP_Id int, LP_PrimeLPId int, LP_DisplayName varchar(256), LP_LegalPartyType int, LP_LPTypeDescr varchar(64), LP_LPSubType int, LP_LPSubTypeDescr varchar(64),
  LP_AliasType int, LP_Aliases varchar(max)
)

insert into #LegalPartyInfo
( LPR_Id, LPR_BegEffDate, LPR_EffStatus, LPR_ObjectType, LPR_ObjectId, LPR_PrimeLegalParty, LPR_LPRoleType, LPR_LPRoleTypeDescr,
  LP_Id, LP_PrimeLPId, LP_DisplayName, LP_LegalPartyType, LP_LPTypeDescr, LP_LPSubType, LP_LPSubTypeDescr, LP_AliasType
)
select lpr.Id, isnull(lpr.BegEffDate,@v_EarliestEffDate), isnull(lpr.EffStatus,'A'), lpr.ObjectType, lpr.ObjectId, lpr.PrimeLegalParty, lpr.LPRoleType, st_lprole.Descr,
  lp.Id, lp.PrimeLPId, rtrim(lp.DisplayName), lp.LegalPartyType, st_lptype.Descr, lp.LPSubType, st_lpsubtype.Descr, lp.AliasType
from LegalParty lp
left join LegalPartyRole lpr
  on lpr.LegalPartyId = lp.Id
left join #SysType st_lprole
  on st_lprole.Id = lpr.LPRoleType
left join #SysType st_lptype
  on st_lptype.id = lp.LegalPartyType
left join #SysType st_lpsubtype
  on st_lpsubtype.id = lp.LPSubType

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 2 of 10: ' + format(@v_RowCount, '#,#') + ' LegalPartyRole/LegalParty rows captured.' )

create index #LegalPartyInfo_index1 on #LegalPartyInfo ( LP_Id )
create index #LegalPartyInfo_index2 on #LegalPartyInfo ( LPR_ObjectType, LPR_ObjectId, LPR_LPRoleType )
create index #LegalPartyInfo_index3 on #LegalPartyInfo ( LPR_BegEffDate ) include ( LPR_Id )

-- **************
-- ** #Aliases **
-- **************
if object_id('tempdb..#AliasInfo') is not null drop table #AliasInfo
create table #AliasInfo ( LP_Id int, LP_PrimeLPId int, LP_DisplayName varchar(256) )

insert into #AliasInfo ( LP_Id, LP_PrimeLPId, LP_DisplayName )
select lp.Id, lp.PrimeLPId, ltrim(rtrim(lp.DisplayName))
from LegalParty lp
where lp.AliasType != 0
and exists ( select 1 from #LegalPartyInfo where LP_Id = lp.PrimeLPId )

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 2.5 of 10: ' + format(@v_RowCount, '#,#') + ' LegalParty aliases captured.' )

update lpi
set lpi.LP_Aliases = aliases.AliasDisplayName
from #LegalPartyInfo lpi
join
  ( select lp.LP_PrimeLPId,
      ( select LP_DisplayName + ' ' as [text()]
        from #AliasInfo
        where LP_Id = lp.LP_Id
        for xml path (''), type
      ).value('.', 'varchar(max)') AliasDisplayName
      from #AliasInfo lp
  ) aliases
  on aliases.LP_PrimeLPId = lpi.LP_Id

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 3 of 10: ' + format(@v_RowCount, '#,#') + ' LegalParty aliases resolved.' )

-- *************
-- ** #RevObj **
-- *************
if object_id('tempdb..#RevObj') is not null drop table #RevObj
create table #RevObj
( Id int, BegEffDate datetime, EffStatus char(1), PIN varchar(32), UnformattedPIN varchar(32), AIN varchar(32), GeoCd varchar(32),
  LatestBegEffDate datetime, LatestPIN varchar(32)
)

insert into #RevObj
( Id, BegEffDate, EffStatus, PIN, UnformattedPIN, AIN, GeoCd )
select ro.Id, ro.BegEffDate, ro.EffStatus, rtrim(ro.PIN), rtrim(ro.UnformattedPIN), rtrim(ro.AIN), rtrim(ro.GeoCd)
from RevObj ro
where exists
  ( select 1
    from #LegalPartyInfo
    where LPR_ObjectId = ro.Id
    and LPR_ObjectType = 100002 -- RevObj
  )

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 4 of 10: ' + format(@v_RowCount, '#,#') + ' RevObj rows captured.' )

create index #RevObj_index1 on #RevObj ( Id, BegEffDate )

update ro
set ro.LatestBegEffDate = ro_latest.BegEffDate,
  ro.LatestPIN = ro_latest.PIN
from #RevObj ro
join #RevObj ro_latest
  on ro_latest.Id = ro.Id
  and ro_latest.BegEffDate = ( select max(BegEffDate) from #RevObj where Id = ro_latest.Id )

-- **************
-- ** #TAGInfo **
-- **************
if object_id('tempdb..#TAGInfo') is not null drop table #TAGInfo
create table #TAGInfo
( TAGRole_Id int, TAGRole_BegEffDate datetime, TAGRole_ObjectId int, TAG_Id int, TAG_BefEffYear smallint, TAG_Descr varchar(64) )

insert into #TAGInfo
( TAGRole_Id, TAGRole_BegEffDate, TAGRole_ObjectId, TAG_Id, TAG_BefEffYear, TAG_Descr )
select tr.Id, tr.BegEffDate, tr.ObjectId, tr.TAGId, t.BegEffYear, rtrim(t.Descr)
from TAGRole tr
join TAG t
  on t.Id = tr.TAGId
  and t.BegEffYear = ( select max(BegEffYear) from TAG   where Id = t.Id and BegEffYear <= @v_EffYear )
  and ( @p_EffStatusFilter is null or t.effStatus = @p_EffStatusFilter )
where tr.ObjectType = 100002 -- RevObj
and exists ( select 1 from #RevObj where Id = tr.ObjectId )
and tr.BegEffDate = ( select max(BegEffDate) from TAGRole where Id = tr.Id and BegEffDate <= @v_EffDate )
and ( @p_EffStatusFilter is null or tr.effStatus = @p_EffStatusFilter )

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 5 of 10: ' + format(@v_RowCount, '#,#') + ' TAGRole/TAG rows captured.' )

create index #TAGInfo_index1 on #TAGInfo ( TAGRole_ObjectId )

-- *******************
-- ** #ApplSiteInfo **
-- *******************
if object_id('tempdb..#ApplSiteInfo') is not null drop table #ApplSiteInfo
create table #ApplSiteInfo
( ApplSiteRole_Id int, ApplSiteRole_BegEffDate datetime, ApplSiteRole_EffStatus char(1), ApplSiteRole_ObjectId int,
  ApplSite_Id int, ApplSite_BegEffDate datetime, ApplSite_EffStatus char(1), ApplSite_ApplSiteName varchar(64),
  ApplSite_ApplClassCd int, ApplSite_ApplClassCdDescr varchar(64), ApplSite_NbhdCd int, ApplSite_NbhdCdDescr varchar(64),
  ApplSite_IsMineral bit default 0
)

insert into #ApplSiteInfo
( ApplSiteRole_Id, ApplSiteRole_BegEffDate, ApplSiteRole_EffStatus, ApplSiteRole_ObjectId, ApplSite_Id, ApplSite_BegEffDate, ApplSite_EffStatus,
  ApplSite_ApplSiteName, ApplSite_ApplClassCd, ApplSite_ApplClassCdDescr, ApplSite_NbhdCd, ApplSite_NbhdCdDescr, ApplSite_IsMineral
)
select asr.Id, asr.BegEffDate, asr.EffStatus, asr.ObjectId, asr.ApplSiteId, aps.BegEffDate, aps.EffStatus, aps.ApplSiteName,
  aps.ApplClassCd, classcd.Descr, aps.NbhdCd, neighborhood.Descr, isnull(mineral.IsMineral,0)
from ApplSiteRole asr
left join ApplSite aps
  on aps.Id = asr.ApplSiteId
  and aps.BegEffDate = ( select max(BegEffDate) from ApplSite where Id = aps.Id and BegEffDate <= @v_EffDate )
  and aps.EffStatus = 'A'
left join
  ( select SysTypeId, 1 IsMineral
    from dbo.grm_FW_SysTypeIntelByEffDate(@v_EffDate, '')
    where SysTypeCatIntId in (270174, 270175)
    and STIValue = 'True'
    group by SysTypeId
  ) mineral on mineral.SysTypeId = aps.ApplSiteType
left join #SysType classcd
  on classcd.Id = aps.ApplClassCd
left join #SysType neighborhood
  on neighborhood.id = aps.NbhdCd
where asr.ObjectType = 100002 -- RevObj
and asr.BegEffDate = ( select max(BegEffDate) from ApplSiteRole where Id = asr.Id and BegEffDate <= @v_EffDate )
and asr.EffStatus = 'A'
and exists ( select 1 from #RevObj where Id = asr.ObjectId )

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 6 of 10: ' + format(@v_RowCount, '#,#') + ' ApplSiteRole/ApplSite rows captured.' )

create index #ApplSiteInfo_index1 on #ApplSiteInfo ( ApplSiteRole_ObjectId )

-- ********************
-- ** #SitusAddrInfo **
-- ********************
if object_id('tempdb..#SitusAddrInfo') is not null drop table #SitusAddrInfo
create table #SitusAddrInfo
( SAR_Id int, SAR_BegEffDate datetime, SAR_EffStatus char(1), SAR_ObjectId int, SAR_PrimeAddr bit,
  SA_Id int, SA_BegEffDate datetime, SA_FreeFormAddr varchar(64), SA_City varchar(32), SA_State varchar(4),
  SA_PostalCd varchar(16), SA_UnitNumber varchar(8), SA_StreetNumber int, SA_StreetName varchar(64), SA_StreetType varchar(8)
)

insert into #SitusAddrInfo
( SAR_Id, SAR_BegEffDate, SAR_EffStatus, SAR_ObjectId, SAR_PrimeAddr,
  SA_Id, SA_BegEffDate, SA_FreeFormAddr, SA_City, SA_State, SA_PostalCd,
  SA_UnitNumber, SA_StreetNumber, SA_StreetName, SA_StreetType
)
select sar.Id, sar.BegEffDate, sar.EffStatus, sar.ObjectId, sar.PrimeAddr,
  sar.SitusAddrId, sa.BegEffDate, rtrim(sa.FreeFormAddr), rtrim(sa.City), rtrim(sa.[State]), rtrim(sa.PostalCd),
  rtrim(sa.UnitNumber), sa.StreetNumber, rtrim(sa.StreetName), rtrim(sa.StreetType)
from SitusAddrRole sar
join SitusAddr sa
  on sa.Id = sar.SitusAddrId
where sar.ObjectType = 100002 -- RevObj
--and sar.BegEffDate = ( select max(BegEffDate) from SitusAddrRole where Id = sar.Id and BegEffDate <= @v_EffDate ) -- TODO: Is this appropriate?
--and sar.EffStatus = 'A'                                                                                           -- TODO: Is this appropriate?

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 7 of 10: ' + format(@v_RowCount, '#,#') + ' SitusAddrRole/SitusAddr rows captured.' )

create index #SitusAddrInfo_index1 on #SitusAddrInfo ( SAR_ObjectId )

truncate table search.LegalPartySearch

-- disable nonclustered indexes
declare c_indexes cursor local static for
  select DisableSQL
  from @t_Indexes

open c_indexes
fetch next from c_indexes into @v_SQL
while @@fetch_status = 0
begin
  --print @v_SQL
  execute sp_executesql @v_SQL -- TODO: change to exec
  fetch next from c_indexes into @v_SQL
end
close c_indexes
deallocate c_indexes

-- Disable FullText index
-- alter fulltext index on [search].[LegalPartySearch] disable
{0}

insert into [search].[LegalPartySearch]
( [LegalPartyRoleId], [LegalPartyId], [LegalPartyRole], [BegEffDate], [EffStatus], [DisplayName],
  [Addr], [PIN], [UnformattedPIN], [AIN],
  [SearchAll],
  [RevObjId], [RevObjBegEffDate], [RevObjEffStatus], [GeoCode], [AddrId], [AddrBegEffDate], [AddrRoleId], [AddrRoleBegEffDate], [AddrRoleEffStatus],
  [AddrType], [AddrUnitNumber], [AddrStreetNumber], [AddrStreetName], [StreetType], [Tag], [TagId], [TagBegEffYear],
  [TagRoleId], [TagRoleBegEffDate], [LegalPartyType], [LegalPartySubType], [LegalPartyTypeId], [LegalPartySubTypeId],
  [AppraisalSiteId], [AppraisalBegEffDate], [AppraisalEffStatus], [AppraisalRoleId], [AppraisalRoleBegEffDate], [AppraisalRoleEffStatus],
  [AppraisalSiteName], [AppraisalClass], [Neighborhood], [Mineral], [Source], [LastUpdated],
  [PrimeOwner], [City], [State], [PostalCode], [RevObjBegEffDateLatest],
  [PINLatest]
)
select lp.LPR_Id, lp.LP_Id, lp.LPR_LPRoleTypeDescr, lp.LPR_BegEffDate, lp.LPR_EffStatus, rtrim(lp.LP_DisplayName),
  c.Comm_DeliveryAddr, isnull(ro.PIN,''), isnull(ro.UnformattedPIN,''), isnull(ro.AIN,''),
  rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(isnull(lp.LP_DisplayName,'') + ' ' + isnull(c.Comm_DeliveryAddr,''))
    + ' ' + isnull(ro.PIN,'')) + ' ' + isnull(ro.AIN,'')) + ' ' + isnull(ro.GeoCd,'')) + ' ' + isnull(t.TAG_Descr,''))
    + ' ' + isnull(ro.UnformattedPIN,'')) + ' ' + isnull(c.Comm_City,'')) + ' ' + isnull(c.Comm_State,'')) + ' ' + isnull(c.Comm_PostalCd,''))+ ' ' + isnull(lp.LP_Aliases,''),
  ro.Id, ro.BegEffDate, ro.EffStatus, isnull(ro.GeoCd,''), c.Comm_Id, c.Comm_BegEffDate, c.CR_Id, c.CR_BegEffDate, c.CR_EffStatus,
  'Mailing', '', c.Comm_PrimeAddrNumber, c.Comm_StreetName, c.Comm_StreetType, isnull(t.TAG_Descr,''), t.TAG_Id, t.TAG_BefEffYear,
  t.TAGRole_Id, t.TAGRole_BegEffDate, lp.LP_LPTypeDescr, lp.LP_LPSubTypeDescr, lp.LP_LegalPartyType, lp.LP_LPSubType,
  aps.ApplSite_Id, aps.ApplSite_BegEffDate, aps.ApplSite_EffStatus, aps.ApplSiteRole_Id, aps.ApplSiteRole_BegEffDate, aps.ApplSiteRole_EffStatus,
  rtrim(aps.ApplSite_ApplSiteName), isnull(aps.ApplSite_ApplClassCdDescr,''), isnull(aps.ApplSite_NbhdCdDescr,''), aps.ApplSite_IsMineral, 'migrations1', @v_StartTime,
  lp.LPR_PrimeLegalParty, c.Comm_City, c.Comm_State, c.Comm_PostalCd, isnull(ro.LatestBegEffDate,'9999-12-31 00:00:00.000'),
  ro.LatestPIN
from #LegalPartyInfo lp
join #CommInfo c
  on c.CR_ObjectId = lp.LP_Id
left join #RevObj ro
  on lp.LPR_ObjectType = 100002 -- RevObj
  and ro.Id = lp.LPR_ObjectId
  and lp.LPR_LPRoleType = 100701 -- Owner
left join #TAGInfo t
  on t.TAGRole_ObjectId = ro.Id
left join #ApplSiteInfo aps
  on aps.ApplSiteRole_ObjectId = ro.Id

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 8 of 10: ' + format(@v_RowCount, '#,#') + ' rows inserted into LegalPartySearch (1st insert).' )

insert into [search].[LegalPartySearch]
( LegalPartyRoleId, LegalPartyId, LegalPartyRole, BegEffDate, EffStatus, DisplayName, Addr,
    PIN, UnformattedPIN, AIN, SearchAll, RevObjId,
  RevObjBegEffDate, RevObjEffStatus, GeoCode, AddrId, AddrBegEffDate, AddrRoleId, AddrRoleBegEffDate, AddrRoleEffStatus, AddrType,
  AddrUnitNumber, AddrStreetNumber, AddrStreetName, StreetType, Tag, TagId, TagBegEffYear,
  TagRoleId, TagRoleBegEffDate, LegalPartyType, LegalPartySubType, LegalPartyTypeId, LegalPartySubTypeId,
  AppraisalSiteId, AppraisalBegEffDate, AppraisalEffStatus, AppraisalRoleId, AppraisalRoleBegEffDate, AppraisalRoleEffStatus,
  AppraisalSiteName, AppraisalClass, Neighborhood, Mineral, [Source], LastUpdated,
  PrimeAddress, PrimeOwner, City, [State], PostalCode, RevObjBegEffDateLatest, PINLatest
)
select lp.LPR_Id, lp.LP_Id, lp.LPR_LPRoleTypeDescr, lp.LPR_BegEffDate, lp.LPR_EffStatus, lp.LP_DisplayName, sa.SA_FreeFormAddr,
  ro.PIN, ro.UnformattedPIN, ro.AIN,
  rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(rtrim(isnull(lp.LP_DisplayName,'') + ' ' + isnull(sa.SA_FreeFormAddr,'')) + ' '
    + isnull(ro.PIN,'')) + ' ' + isnull(ro.AIN,'')) + ' ' + isnull(ro.GeoCd,'')) + ' ' + isnull(t.TAG_Descr,'')) + ' '
    + isnull(ro.UnformattedPIN,'')) + ' ' + isnull(sa.SA_City,'')) + ' ' + isnull(sa.SA_State,'')) + ' ' + isnull(sa.SA_PostalCd,''))
    + ' ' + isnull(lp.LP_Aliases,''),
  ro.Id, ro.BegEffDate, ro.EffStatus, isnull(ro.GeoCd,''), sa.SA_Id, sa.SA_BegEffDate, sa.SAR_Id, sa.SAR_BegEffDate, sa.SAR_EffStatus, 'Situs',
  sa.SA_UnitNumber, sa.SA_StreetNumber, rtrim(sa.SA_StreetName), sa.SA_StreetType, isnull(t.TAG_Descr,''), t.TAG_Id, t.TAG_BefEffYear,
  t.TAGRole_Id, t.TAGRole_BegEffDate, lp.LP_LPTypeDescr, lp.LP_LPSubTypeDescr, lp.LP_LegalPartyType, lp.LP_LPSubType,
  asi.ApplSite_Id, asi.ApplSite_BegEffDate, asi.ApplSite_EffStatus, asi.ApplSiteRole_Id, asi.ApplSiteRole_BegEffDate, asi.ApplSiteRole_EffStatus,
  rtrim(asi.ApplSite_ApplSiteName), isnull(asi.ApplSite_ApplClassCdDescr,''), isnull(asi.ApplSite_NbhdCdDescr,''), asi.ApplSite_IsMineral, 'migrations2', @v_StartTime,
  sa.SAR_PrimeAddr, lp.LPR_PrimeLegalParty, sa.SA_City, sa.SA_State, sa.SA_PostalCd, isnull(ro.LatestBegEffDate,'9999-12-31 00:00:00.000'), ro.LatestPIN
from #LegalPartyInfo lp
join #RevObj ro
  on lp.LPR_ObjectType = 100002 -- RevObj
  and ro.Id = lp.LPR_ObjectId
left join #SitusAddrInfo sa
  on sa.SAR_ObjectId = ro.Id
left join #TAGInfo t
  on t.TAGRole_ObjectId = ro.Id
left join #ApplSiteInfo asi
  on asi.ApplSiteRole_ObjectId = ro.Id

set @v_RowCount = @@rowcount
insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 9 of 10: ' + format(@v_RowCount, '#,#') + ' rows inserted into LegalPartySearch (2nd insert).' )

-- rebuild nonclustered indexes
declare c_indexes cursor local static for
  select RebuildSQL
  from @t_Indexes

open c_indexes
fetch next from c_indexes into @v_SQL
while @@fetch_status = 0
begin
  execute sp_executesql @v_SQL
  fetch next from c_indexes into @v_SQL
end
close c_indexes
deallocate c_indexes

-- Enable FullText index
-- alter fulltext index on [search].[LegalPartySearch] enable
{1}

insert into search.SimpleSearchSeed_Log ( StartTime, StatusMessage )
values ( @v_StartTime, 'Step 10 of 10: Indexes re-enabled on LegalPartySearch table.' )
";


		public const string SqlErrorNumberForLocked = "51000";
		private const string GetLockSqlBeforeSqlErrorCode = @"
DECLARE @getAppLockResult int;
EXEC @getAppLockResult = sp_getapplock @Resource = 'UpdatedSeedQuery',
	@LockMode = 'Exclusive',
	@LockOwner = 'Session',
	@LockTimeout = 0;
IF @getAppLockResult = -1	-- the SQL is already running
	THROW ";

		private const string GetSqlSqlAfterSqlErrorCode = @", 'UpdatedSeedQuery is already running', 1;";
		private const string GetLockSql = GetLockSqlBeforeSqlErrorCode + SqlErrorNumberForLocked + GetSqlSqlAfterSqlErrorCode;

		private const string ReleaseLockSql = @"EXEC sp_releaseapplock @Resource = 'UpdatedSeedQuery',
@LockOwner = 'Session';";
		public static string BuildSeedQuery()
		{
			string rebuildAllSql = string.Format(ExecuteSql, string.Empty, string.Empty);
			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}

		public static string RebuildSeedQuery()
		{
			string rebuildAllSql = string.Format(ExecuteSql, DisableIndex, EnableIndex);
			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}

		public static string GetCrawlProgress()
		{
			string rebuildAllSql = string.Format(CrawlSql);
			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}
	}
}
