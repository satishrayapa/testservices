param(
     [Parameter(Mandatory=$true)]   [string]$WorkflowURL
    ,[Parameter(Mandatory=$true)]   [string]$ConnectionString
)

Import-Module $PSScriptRoot\lib\SqlHelpers.psm1

$query = "
    declare @workflowID int;
    declare @workflowSettingID int;
    declare @workflowDate datetime;
    declare @newEffDate datetime;
    declare @appInstance nvarchar(100) = N'';

    set @newEffDate = dateadd(s, 1, cast(cast(getdate() as varchar(12)) as datetime));
    declare @p1 int = 0;

    -- Get AppSettings ID
    SELECT @workflowID=Id FROM AppSetting WHERE ShortDescr = 'WFServiceURI' ;

    -- Get the effective dates to prevent dirty read logic
    SELECT @workflowDate=BegEffDate, @workflowSettingID = id, @appInstance=isnull(AppInstance, '')
    FROM dbo.EffDateSetting as eds
    WHERE eds.AppSettingId = @workflowID AND eds.BegEffDate = (SELECT MAX(sub.BegEffDate) FROM EffDateSetting sub WHERE sub.Id = eds.Id AND DATEDIFF(DAY, sub.BegEffDate, '12/31/2999') >= 0)

    if(@workflowSettingID is null)
    BEGIN
        select @workflowSettingID = max(id)+1 from dbo.EffDateSetting;
    END

    if(@workflowSettingID is null)
    BEGIN
        select @workflowSettingID = max(id)+1 from dbo.EffDateSetting;
    END

    if exists ( select * from dbo.EffDateSetting where BegEffDate = @newEffDate and AppSettingId = @workflowID)
    begin
        select @newEffDate = dateadd(ss, 1, max(BegEffDate))
        from dbo.EffDateSetting
        where BegEffDate >= @newEffDate and AppSettingId = @workflowID
    end

    -- update workflow url
    exec EDS_U @p_returnCode=@p1 output,@p_newEffDate=@newEffDate,@p_newTaxYear=2013,@p_newEffStatus=N'A',@p_Id=@workflowSettingID,@p_BegEffDate=@workflowDate,@p_TranId=374474574,@p_AppSettingId=@workflowID,@p_Setting=N'$WorkflowURL',@p_AppInstance=@appInstance;

"

ExecuteSQLQuery -ConnectionString $ConnectionString -Query $query -OutputLabel "Workflow Root URL"
