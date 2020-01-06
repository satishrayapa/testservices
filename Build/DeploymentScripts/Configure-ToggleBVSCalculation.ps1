param(
     [Parameter(Mandatory=$true)]   [boolean]$State
    ,[Parameter(Mandatory=$true)]   [string]$ConnectionString
)

Import-Module $PSScriptRoot\lib\SqlHelpers.psm1

$query = "
    declare @bvsID int;
    declare @bvsSettingID int;
    declare @bvsDate datetime;
    declare @newEffDate datetime;
    declare @appInstance nvarchar(100) = N'';

    set @newEffDate = dateadd(s, 1, cast(cast(getdate() as varchar(12)) as datetime));
    declare @p1 int = 0;

    -- Get AppSettings ID
    SELECT @bvsID=Id FROM AppSetting WHERE ShortDescr = 'LumenBVS';

    -- Get the effective dates to prevent dirty read logic
    SELECT @bvsDate=BegEffDate, @bvsSettingID=id, @appInstance=isnull(AppInstance, '')
    FROM dbo.EffDateSetting as eds
    WHERE eds.AppSettingId = @bvsID AND eds.BegEffDate = (SELECT MAX(sub.BegEffDate) FROM EffDateSetting sub WHERE sub.Id = eds.Id AND DATEDIFF(DAY, sub.BegEffDate, '12/31/2999') >= 0);

    if(@bvsSettingID is null)
    BEGIN
        select @bvsSettingID = max(id)+1 from dbo.EffDateSetting;
    END

    declare @value nvarchar(10) = lower('$State')

    if exists ( select * from dbo.EffDateSetting where BegEffDate = @newEffDate and AppSettingId = @bvsID)
    begin
        select @newEffDate = dateadd(ss, 1, max(BegEffDate))
        from dbo.EffDateSetting
        where BegEffDate >= @newEffDate and AppSettingId = @bvsID
    end

    -- update bvsflag
    exec EDS_U @p_returnCode=@p1 output,@p_newEffDate=@newEffDate,@p_newTaxYear=2013,@p_newEffStatus=N'A',@p_Id=@bvsSettingID,@p_BegEffDate=@bvsDate,@p_TranId=374474574,@p_AppSettingId=@bvsID,@p_Setting=@value,@p_AppInstance=@appInstance;
"

ExecuteSQLQuery -ConnectionString $connectionString -Query $query -OutputLabel "Toggle BVS Calculation ($State)"
