namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public static class RebuildSearchLegalPartyIndex
	{
		public const string SqlErrorNumberForLocked = "51000";
		private const string TruncateSql = "TRUNCATE TABLE search.LegalPartySearch";
		private const string DeleteLegalPartyIdsInSql = "DELETE FROM [search].[LegalPartySearch] WHERE LegalPartyId IN ({0})";
		private const string AndLegalPartyIdsInSql = "AND lp.Id IN ({0})";
		private const string RebuildSql = @"
DECLARE @p_EffDate datetime
DECLARE @p_EffStatusFilter char(1)
DECLARE @EffYear SMALLINT
DECLARE @EarliestEffDate datetime = '1776-07-04 00:00:00.000'
DECLARE @MailingAddress INT
SET @p_EffDate = '12/31/9999'
SET @EffYear = DATEPART(YEAR, @p_EffDate)
SET @MailingAddress = 101850 
{0}

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
										,[Addr], [PIN], [UnformattedPIN], [AIN], [SearchAll], [RevObjId], [RevObjBegEffDate], [RevObjEffStatus]
										,[GeoCode], [AddrId], [AddrBegEffDate], [AddrRoleId], [AddrRoleBegEffDate],[AddrRoleEffStatus], [AddrType], [AddrUnitNumber]
										,[AddrStreetNumber], [AddrStreetName], [StreetType], [Tag], [TagId], [TagBegEffYear], [TagRoleId], [TagRoleBegEffDate], [LegalPartyType]
										,[LegalPartySubType], [LegalPartyTypeId], [LegalPartySubTypeId], [AppraisalSiteId], [AppraisalBegEffDate], [AppraisalEffStatus]
										,[AppraisalRoleId], [AppraisalRoleBegEffDate], [AppraisalRoleEffStatus], [AppraisalSiteName], [AppraisalClass], [Neighborhood], [Mineral]
										,[Source],[LastUpdated],[PrimeOwner],[City],[State],[PostalCode],[RevObjBegEffDateLatest],[PINLatest])
SELECT lpr.[Id]
	,lp.[Id] 
	,lprole.descr as LegalPartyRole
	,COALESCE(lpr.BegEffDate, @EarliestEffDate) --if no legal party role assume effective
	,COALESCE(lpr.EffStatus, 'A') --if no legal party role assume effective
	,lp.DisplayName
	,c.DeliveryAddr as Addr
	,COALESCE(RTRIM(ro.PIN),'') as PIN
	,COALESCE(RTRIM(ro.UnformattedPIN),'') as UnformattedPIN 
	,COALESCE(RTRIM(ro.AIN),'') as AIN
	,RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(lp.DisplayName + ' ' + c.DeliveryAddr) + ' ' + COALESCE(RTRIM(ro.PIN),'')) + ' ' + COALESCE(RTRIM(ro.AIN),'')) + ' ' + COALESCE(RTRIM(ro.GeoCd),'')) + ' ' + COALESCE(RTRIM(t.Descr),'')) + ' ' + COALESCE(RTRIM(ro.UnformattedPIN),'')) + ' ' + COALESCE(RTRIM(c.City),'')) + ' ' + COALESCE(RTRIM(c.State),'')) + ' ' + COALESCE(RTRIM(c.PostalCd),''))  as SearchAll
	,ro.Id as RevObjId
	,ro.BegEffDate as RevObjBegEffDate
    ,ro.EffStatus
	,COALESCE(RTRIM(ro.GeoCd),'') as GeoCode
	,c.Id as commId
	,c.BegEffDate as AddrBegEffDate
	,cr.Id as AddrRoleId
	,cr.BegEffDate as AddrRoleBegEffDate
	,cr.EffStatus
	,'Mailing' as AddrType
	,'' as AddrUnitNumber
	,c.PrimeAddrNumber as AddrStreetNumber
	,RTRIM(c.StreetName) as AddStreetName
	,c.StreetType
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
	,CASE WHEN aps.Id is null then null when mn.SysTypeId is NULL then 0 ELSE 1 END as Mineral
	,'{2}'
	,GETDATE()
	,lpr.PrimeLegalParty
	,RTRIM(c.City) as City
	,RTRIM(c.State) as State
	,RTRIM(c.PostalCd) as PostalCd
	,(select isnull(max(ro2.BegEffDate), @p_EffDate) as RevBegEffDateLatestas from revobj ro2 where ro2.id=ro.id)
	,(select ro2.Pin as PINLatest from revobj ro2  where ro2.BegEffDate = (select max(ro1.BegEffDate) as RevBegEffDateLatest  from revobj ro1  where  ro1.id=ro.id) and ro2.id= ro.id)

FROM dbo.LegalParty as lp
LEFT JOIN
(
  dbo.LegalPartyRole as lpr
  JOIN (select id, max(begEffDate) as begEffDate from dbo.LegalPartyRole where begEffDate <= @p_EffDate group by id) as fltLpr on fltLpr.id = lpr.id AND fltlpr.begEffDate = lpr.begEffDate
  JOIN #sysType as lprole on lprole.id = lpr.LPRoleType
)
on lp.Id = lpr.LegalPartyId
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
JOIN dbo.Comm as c on c.Id = cr.CommId and c.CommType=@MailingAddress
JOIN
(
	select id, max(begEffDate) as begEffDate
	from dbo.Comm
	where begEffDate<CASE WHEN @p_EffDate >= '12/31/9999' THEN '12/31/9999 23:59:59.997' ELSE DATEADD(DAY,1, CAST(@p_EffDate AS DATE)) END
	group by id
) as fltComm on fltComm.id = c.Id AND fltComm.begEffDate = c.BegEffDate
LEFT JOIN RevObj ro on ro.Id = lpr.ObjectId and lpr.ObjectType = 100002 and lpr.LPRoleType = 100701
LEFT JOIN (select id, begEffDate, objectType, objectId, TAGId from dbo.grm_records_TAGRoleByEffDate(@p_EffDate, @p_EffStatusFilter) where ObjectType = 100002) as tr on tr.ObjectId = ro.Id
LEFT JOIN (select id, descr, BegEffYear from dbo.grm_levy_TAGByEffYear(@EffYear, @p_EffStatusFilter)) as t on t.Id = tr.TAGId
LEFT JOIN (select Id, objectId, ObjectType, ApplSiteId, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteRoleByEffDate(@p_EffDate,'A') where ObjectType = 100002) as asr ON ro.Id = asr.ObjectId
LEFT JOIN (select id, ApplSiteName, ApplClassCd, NbhdCd, ApplSiteType, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteByEffDate(@p_EffDate,'A') ) as aps ON asr.ApplSiteId = aps.Id
LEFT JOIN
(
	SELECT SysTypeId
        FROM dbo.grm_FW_SysTypeIntelByEffDate(@p_EffDate, '')
        WHERE SysTypeCatIntId IN(270174, 270175) AND STIValue = 'True'
        group by SysTypeId
) as mn on mn.SysTypeId = aps.ApplSiteType

WHERE (@p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)
	AND (@p_EffStatusFilter IS NULL OR cr.effStatus = @p_EffStatusFilter)
	AND (@p_EffStatusFilter IS NULL OR c.effStatus = @p_EffStatusFilter)
	AND (@p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
	AND (@p_EffStatusFilter IS NULL OR lpType.effStatus = @p_EffStatusFilter)
	AND (@p_EffStatusFilter IS NULL OR lpSType.effStatus = @p_EffStatusFilter)
{1}

INSERT INTO [search].[LegalPartySearch]
           ([LegalPartyRoleId]
           ,[LegalPartyId]
           ,[LegalPartyRole]
		   ,[BegEffDate]
		   ,[EffStatus]
           ,[DisplayName]
           ,[Addr]
		   ,PIN
		   ,UnformattedPIN
		   ,AIN
           ,[SearchAll]
		   ,[RevObjId]
		   ,[RevObjBegEffDate]
		   ,[RevObjEffStatus]
		   ,[GeoCode]
		   ,[AddrId]
		   ,[AddrBegEffDate]
		   ,[AddrRoleId]
		   ,[AddrRoleBegEffDate]
		   ,[AddrRoleEffStatus]
		   ,[AddrType]
		   ,[AddrUnitNumber]
		   ,[AddrStreetNumber]
		   ,[AddrStreetName]
		   ,[StreetType]
		   ,[Tag]
		   ,[TagId]
		   ,[TagBegEffYear]
		   ,[TagRoleId]
		   ,[TagRoleBegEffDate]
		   ,[LegalPartyType]
		   ,[LegalPartySubType]
		   ,[LegalPartyTypeId]
		   ,[LegalPartySubTypeId]
		   ,[AppraisalSiteId]
		   ,[AppraisalBegEffDate]
		   ,[AppraisalEffStatus]
		   ,[AppraisalRoleId]
		   ,[AppraisalRoleBegEffDate], [AppraisalRoleEffStatus], [AppraisalSiteName], [AppraisalClass], [Neighborhood], [Mineral]
		   ,[Source],[LastUpdated]
		   ,[PrimeAddress]
		   ,[PrimeOwner]
		   ,[City]
		   ,[State]
		   ,[PostalCode]
		   ,[RevObjBegEffDateLatest]
           ,[PINLatest])
SELECT lpr.[Id]
       ,[LegalPartyId]      
	   ,lprole.descr as LegalPartyRole
	   ,lpr.BegEffDate
	   ,lpr.EffStatus
	   ,lp.DisplayName
	   ,RTRIM(sa.FreeFormAddr) as SitusAddress
	   ,RTRIM(ro.PIN) as PIN
       ,RTRIM(ro.UnformattedPIN) as UnformattedPIN
	   ,RTRIM(ro.AIN) as AIN
	   ,RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(RTRIM(lp.DisplayName + ' ' + RTRIM(COALESCE(sa.FreeFormAddr,''))) + ' ' + COALESCE(RTRIM(ro.PIN),'')) + ' ' + COALESCE(RTRIM(ro.AIN),'')) + ' ' + COALESCE(RTRIM(ro.GeoCd),'')) + ' ' + COALESCE(RTRIM(t.Descr),'')) + ' ' + COALESCE(RTRIM(ro.UnformattedPIN), '')) + ' ' + COALESCE(RTRIM(sa.City), '')) + ' ' + COALESCE(RTRIM(sa.State), '')) + ' ' + COALESCE(RTRIM(sa.PostalCd), ''))
			+ ' ' + COALESCE
		              (RTRIM(	-- This contatenates all the alias display names (if any) together so a search on any alias will result in a hit on the primary legal party
				        (
				          SELECT alp.DisplayName + ' ' as [text()]
				          FROM dbo.LegalParty alp
				          where alp.PrimeLPId = lp.Id
				          and alp.AliasType <> 0
				          FOR XML PATH (''), TYPE
				        ).value('.', 'NVARCHAR(MAX)')
				      ), ''))	as SearchAll
	   ,ro.Id
	   ,ro.BegEffDate
	   ,ro.EffStatus
	   ,COALESCE(RTRIM(ro.GeoCd),'') as GeoCode
	   ,sa.Id
	   ,sa.BegEffDate
	   ,sar.Id
	   ,sar.BegEffDate
	   ,sar.EffStatus
	   ,'Situs'
	   ,sa.UnitNumber as AddrUnitNumber
	   ,sa.StreetNumber as AddrStreetNumber
	   ,RTRIM(sa.StreetName) as AddStreetName
	   ,sa.StreetType
	   ,COALESCE(RTRIM(t.Descr),'') as Tag
	   ,COALESCE(t.Id,NULL) as TagId
	   ,COALESCE(t.BegEffYear,NULL) as TagBegEffYear
	   ,COALESCE(tr.Id,NULL) as TagRoleId
	   ,COALESCE(tr.BegEffDate,NULL) as TagRoleBegEffDate
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
		,CASE WHEN aps.Id is null then null when mn.SysTypeId is NULL then 0 ELSE 1 END as Mineral
		,'{2}'
		,GETDATE()
		,sar.PrimeAddr
		,lpr.PrimeLegalParty
		,RTRIM(sa.City) as City
		,RTRIM(sa.State) as State
		,RTRIM(sa.PostalCd) as PostalCd
		,(select isnull(max(ro2.BegEffDate), @p_EffDate) as RevBegEffDateLatest from revobj ro2 where ro2.id=ro.id)
		,(select ro2.Pin as PINLatest from revobj ro2  where ro2.BegEffDate = (select max(ro1.BegEffDate) as RevBegEffDateLatest  from revobj ro1  where  ro1.id=ro.id) and ro2.id= ro.id)
  FROM [dbo].[LegalPartyRole] as lpr
  inner join SysType as lprole on lprole.id = lpr.LPRoleType
  inner join LegalParty as lp on lp.Id = lpr.LegalPartyId
  inner join RevObj ro
            on ro.Id = lpr.ObjectId
            and lpr.ObjectType = 100002 -- ObjectType.RevObj
  left join SitusAddrRole as sar on sar.ObjectType = 100002 and sar.ObjectId = ro.Id
  left join SitusAddr as sa on sa.Id = sar.SitusAddrId
  inner join SysType as lpType on lpType.id = lp.LegalPartyType
  inner join SysType as lpSType on lpStype.id = lp.LPSubType
  left join grm_records_TAGRoleByEffDate(@p_EffDate, @p_EffStatusFilter) as tr on tr.ObjectType = 100002
			and tr.ObjectId = ro.Id
  left join grm_levy_TAGByEffYear(@EffYear, @p_EffStatusFilter) as t on t.Id = tr.TAGId
	LEFT JOIN (select Id, objectId, ObjectType, ApplSiteId, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteRoleByEffDate(@p_EffDate,'A') where ObjectType = 100002) as asr ON ro.Id = asr.ObjectId
	LEFT JOIN (select id, ApplSiteName, ApplClassCd, NbhdCd, ApplSiteType, BegEffDate, EffStatus from dbo.GRM_RPA_ApplSiteByEffDate(@p_EffDate,'A') ) as aps ON asr.ApplSiteId = aps.Id
	LEFT JOIN
	(
			SELECT SysTypeId
             FROM dbo.grm_FW_SysTypeIntelByEffDate(@p_EffDate, '')
             WHERE SysTypeCatIntId IN(270174, 270175) AND STIValue = 'True'
             group by SysTypeId
	) as mn on mn.SysTypeId = aps.ApplSiteType
  where
    LPR.begEffDate = ( select max(sub.begEffDate) from LegalPartyRole sub where sub.begEffDate <= @p_EffDate AND sub.Id = LPR.Id )
          AND   ( @p_EffStatusFilter IS NULL OR LPR.effStatus = @p_EffStatusFilter)
	AND lprole.begEffDate = ( select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lprole.Id )
          AND   ( @p_EffStatusFilter IS NULL OR lprole.effStatus = @p_EffStatusFilter)
	AND lpType.begEffDate = ( select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpType.Id )
          AND   ( @p_EffStatusFilter IS NULL OR lpType.effStatus = @p_EffStatusFilter)
	AND lpSType.begEffDate = ( select max(sub.begEffDate) from SysType sub where sub.begEffDate <= @p_EffDate AND sub.Id = lpSType.Id )
          AND   ( @p_EffStatusFilter IS NULL OR lpSType.effStatus = @p_EffStatusFilter)
{1} 
{3}";



		private const string HandleNewlyCreatedLegalParties = @"
	IF NOT EXISTS (select * from search.legalpartysearch where legalpartyid in ({0}))

	BEGIN
	
	INSERT INTO [search].[LegalPartySearch](
	[LegalPartyId], 
	[DisplayName],
	[BegEffDate],
	[LegalPartyRoleId],
	[Addr], 
	[SearchAll], 
	[AddrId], 
	[AddrBegEffDate], 
	[AddrRoleId], 
	[AddrRoleBegEffDate],
	[AddrType], 
	[AddrUnitNumber],
	[AddrStreetNumber], 
	[AddrStreetName], 
	[LegalPartyType],
	[LegalPartySubType], 
	[LegalPartyTypeId], 
	[LegalPartySubTypeId])

		SELECT 
			lp.Id,
			lp.DisplayName,
			'1776-07-04',
			0,
			c.DeliveryAddr as Addr,
			RTRIM(lp.DisplayName + ' ' + c.DeliveryAddr) as SearchAll,	   
			c.Id as commId,
			c.BegEffDate as AddrBegEffDate,
			cr.Id as AddrRoleId,
			cr.BegEffDate as AddrRoleBegEffDate,
			'Mailing' as AddrType,
			'' as AddrUnitNumber,
			c.PrimeAddrNumber as AddrStreetNumber,
			RTRIM(c.StreetName) as AddStreetName,
			lpType.descr as LegalPartyType,
			lpSType.descr as LegalPartySubType,
			lp.LegalPartyType as LegalPartyTypeId,
			lp.LPSubType as LegalPartySubTypeId	

			FROM dbo.LegalParty as lp
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
			WHERE lp.Id in ({0})
	END";

		private const string GetLockSqlBeforeSqlErrorCode = @"
DECLARE @getAppLockResult int;
EXEC @getAppLockResult = sp_getapplock @Resource = 'RebuildAllSearchLegalPartyIndex',
	@LockMode = 'Exclusive',
	@LockOwner = 'Session',
	@LockTimeout = 0;
IF @getAppLockResult = -1	-- the SQL is already running
	THROW ";

		private const string GetSqlSqlAfterSqlErrorCode = @", 'RebuildAllSearchLegalPartyIndex is already running', 1;";

		private const string GetLockSql = GetLockSqlBeforeSqlErrorCode + SqlErrorNumberForLocked + GetSqlSqlAfterSqlErrorCode;

		private const string ReleaseLockSql = @"EXEC sp_releaseapplock @Resource = 'RebuildAllSearchLegalPartyIndex',
@LockOwner = 'Session';";

		public static string GetRebuildVersion()
		{
			return string.Format(RebuildSql, DeleteLegalPartyIdsInSql, AndLegalPartyIdsInSql, "rebuildapi", HandleNewlyCreatedLegalParties);
		}

		public static string GetMigrationVersion()
		{
			return string.Format(RebuildSql, TruncateSql, string.Empty, "migrations", string.Empty);
		}

		public static string GetRebuildAllVersion()
		{
			string rebuildAllSql = string.Format(RebuildSql, TruncateSql, string.Empty, "rebuildall", string.Empty);

			return string.Concat(GetLockSql, rebuildAllSql, ReleaseLockSql);
		}
	}
}
