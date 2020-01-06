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
    and not exists(select 1 from RoleFunction rf where rf.AppFunctionId = af.Id)

    declare @AdminRoleId INT;

    select @AdminRoleId = r.Id from [Role] r where 1=1
        and r.Id in (select distinct RoleId from UserProfileRole
            where UserProfileId=(select Id from UserProfile where UserLogin='$username' and EffStatus='A'))
        and r.Name = 'admin'

    INSERT INTO RoleFunction(Id, TranId, RoleId, AppFunctionId, CanCreate, CanModify,CanExecute,CanView,CanDelete)
    select (select max(irf.Id)+af.Id from RoleFunction irf), 0, @AdminRoleId, af.AppFunctionId, 1,1,1,1,1 from #af af
"

ExecuteSQLQuery -ConnectionString $connectionString -Query $query -OutputLabel " adding services roles"
Write-Host
