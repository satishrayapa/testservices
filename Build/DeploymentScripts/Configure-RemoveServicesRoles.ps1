param(
     [Parameter(Mandatory=$true)]  [string]$Username
    ,[Parameter(Mandatory=$true)]  [string]$ConnectionString
)

Import-Module $PSScriptRoot\lib\SqlHelpers.psm1

$query = "
        IF OBJECT_ID('tempdb..#af') IS NOT NULL
        DROP TABLE #af

        create table #af (Id INT IDENTITY(1, 1),AppFunctionId INT)

        insert into #af (AppFunctionId)
        select af.Id from AppFunction af where 1=1
        and af.AppFunctionType ='field'
        and af.App LIKE 'api.%'
        and exists(select 1 from RoleFunction rf where rf.AppFunctionId = af.Id)

        declare @AdminRoleId INT;

        select @AdminRoleId = r.Id
        from [Role] r
        where 1=1
        and r.Id in (select distinct RoleId from UserProfileRole where UserProfileId=(select Id from UserProfile where UserLogin='$Username' and EffStatus='A'))
        and r.Name = 'admin'


        delete from RoleFunction
        where roleId = @AdminRoleId and AppFunctionId in ( select appfunctionId from #af )
"

ExecuteSQLQuery -ConnectionString $connectionString -Query $query -OutputLabel " removing services roles"
Write-Host
