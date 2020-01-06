function ValidateParams {
    [CmdletBinding()]
	param (
        [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]   [hashtable]$configs
    )

    $isValid = $true
    $results = @()

    ## Test the Primary / Secondary installation option
    $results += $( IsValidPrimarySecondary -string $configs.Get_Item("ServerRole") -source "ServerRole" )

    ## Test if SourceRootPath exists
    $results += $( IsValidString -string $configs.Get_Item("SourceRootPath") -source "SourceRootPath" )
    $results += $( IsValidPath -path $configs.Get_Item("SourceRootPath") -source "SourceRootPath" -mustExists $true)

    ## Test if the destination drive for service exists
    $results += $( IsValidString $configs.Get_Item("ServiceRootPath") -source "ServiceRootPath")
    $results += $( IsValidDrive $configs.Get_Item("ServiceRootPath") -source "ServiceRootPath")

    if($configs.Get_Item("EnableSearch"))
    {
        ## Test if the destination drive for the win service exist / test if security allow Write
		$results += $( IsValidString $configs.Get_Item("WinServicePath") -source "WinServicePath")
		$results += $( IsValidDrive $configs.Get_Item("WinServicePath") -source "WinServicePath")
		$results += $( IsValidPath -path $configs.Get_Item("WinServicePath") -source "WinServicePath" -mustExists $false)
    }

    ## Test if ExternalURI is set
    if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
        $results += $( IsValidString $configs.Get_Item("ExternalUriDNSName") -source "ExternalUriDNSName")
        ## -- ExternalURI most not contains http:// or https://
        if($configs.Get_Item("ExternalUriDNSName") -like 'http://*' -OR $configs.Get_Item("ExternalUriDNSName") -like 'http://*'){
            $results += @($false), @($source), @("ExternalUriDNSName cannot contains http:// or https://, use ExternalUriUseHTTPS to configure the desired protocol.")
        }

        if([string]::IsNullOrEmpty($configs.Get_Item("ExternalUriUseHTTPS"))){
            $results += @($false), @($source), @("ExternalUriUseHTTPS is required when ExternalUriDNSName is set.")
        }

        ## -- if usehttp is set to use https, externaluriusehttps must be true
        If (!$configs.Get_Item("UseHTTP") ) {
            if(!$configs.Get_Item("ExternalUriUseHTTPS")){
                $results += @($false), @($source), @("If UseHttps is set to true, ExternalUriUseHTTPS must also be set to true.")
            }
        }

    }

    ## Check if a ASPNetCore var is prodided, should be by default
    $results += $( IsValidString $configs.Get_Item("AspNetCoreEnvVariable") -source "AspNetCoreEnvVariable")

    ## Test if server name is reachable and up
    $results += $( IsValidString $configs.Get_Item("ServerName") -source "ServerName")

    ## Test if Aumentum root path exists
    $results += $( IsValidString $configs.Get_Item("AumentumAppRootPath") -source "AumentumAppRootPath")
    $results += $( IsValidPath $configs.Get_Item("AumentumAppRootPath") -source "AumentumAppRootPath" -mustExists $true)
    $results += $( IsValidWebsite $configs.Get_Item("AumentumAppRootPath") -source "AumentumAppRootPath")

    ## Test if Web Application exists and is properly configured
    $results += $( IsValidString $configs.Get_Item("WebSiteName") -source "WebSiteName")

    ## Test database components and connection server is up
    $results += $( IsValidString $configs.Get_Item("connectionString") -source "connectionString")

    ## test if aumentum workflow path is valid
    ## aditionnal tests ?

    if($configs.Get_Item("IsPrimary")){
        ## Test is SQLServer is reachable, only if running migrations.
        $results += $( IsSqlServerReachable -connectionString $configs.Get_Item("connectionString") -source "SqlServerAvailable" -erroraction 'silentlycontinue' )

        if($configs.Get_Item("EnableBVS")){
            ## Test if the LumenBVS AppSetting exists
            $results += $( IsLumenBVSAppSettingPresent -connectionString $configs.Get_Item("connectionString") -source "LumenBVSAppSetting" -erroraction 'silentlycontinue')
        }

        if($configs.Get_Item("EnableSearch")){
            ## Test if full-textsearch is enabled in DB
            $results += $( IsFullTextSearchInstalled -connectionString $configs.Get_Item("connectionString") -source "FullTextSearchEnabled" -erroraction 'silentlycontinue')
        }
    }

    for($i=0; $i -le ($results.Length -1); $i += 3){
        if(!$results[$i]){
            $isValid = $false
            $msg += "Invalid Configuration for: " + $results[$i+1] + " | " + $results[$i+2] + "`r`n"
        }
    }

    if(!$isValid){
        Write-Host "Validations Errors: " -BackgroundColor Red
        Write-Host $msg -ForegroundColor Red
    }
    return $isValid
}

function IsValidPrimarySecondary{
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$string
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]     [string]$source
    )

    $out = @()

    if([string]::IsNullOrEmpty($string)){
        $out += @($false), @($source), @("The parameter cannot be an empty string. Using configuration value: $path")
    }
    elseif( !( $string.ToLower() -eq "primary" -Or $string.ToLower() -eq "secondary") ){
        $out += @($false), @($source), @("The parameter needs to be either 'Primary' or 'Secondary': $path")
    }
    else{
        $out += @($true), @($source), @("")
    }

    return $out
}

function IsValidString{
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$string
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]     [string]$source
    )

    $out = @()

    if([string]::IsNullOrEmpty($string)){
        write-host "invalid"
        $out += @($false), @($source), @("The parameter cannot be an empty string. Using configuration value: $path")
    }
    else{
        $out += @($true), @($source), @("")
    }

    return $out
}

function IsValidDrive(){
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$path
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$source
    )

    $out = @()

    $volumes = @(Get-Volume | ForEach-Object {$_.DriveLetter})
    $drive = $path.Substring(1)
    if(! $volumes -contains $drive){
        $out += @($false), @($source), @("The destination drive do not exists. Using configuration value: $path")
    }
    else{
        $out += @($true), @($source), @("")
    }

    return $out
}

function IsValidPath(){
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$path
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$source
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=2)]    [bool]$mustExists
    )

    $isValid = $true

    $out = @()
    if(!$(Test-Path -path $path -IsValid)){
        $out += @($false), @($source), @("The destination path is invalid. Using configuration value: $path")
        $isValid = $false
    }

    ## testing if we have :\ in the path since powershell test-path function consider c\somefolder valid
    ## but scripts will fail later on since it wont resolve the right path...
    if( $path.IndexOf(':\') -eq -1){
        $out += @($false), @($source), @("The destination path is invalid. Using configuration value: $path")
        $isValid = $false
    }

    if($mustExists){
        if(!$(Test-Path -path $path)){
            $out += @($false), @($source), @("The destination path do not exists. Using configuration value: $path")
            $isValid = $false
        }
    }

    if($isValid){
        $out += @($true), @($source), @("")
    }

    return $out
}

function IsWinServiceOutsideWWW{
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$string
        #,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]     [string]$source
    )

    $warning = ""
    if( ($string.IndexOf('wwwroot') -ne -1) -OR ($string.IndexOf('inetpub') -ne -1) ) {
        $warning = "Installation of Windows services in a folder under IIS (i.e., inetpub) is strongly discouraged for security reasons as this may expose both the service and Aumentum to undue risk and/or stability. Support for installing Aumentum Windows services under IIS may not be supported with future releases of the product."
    }
    return $warning

}

function IsValidWebsite{
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$AumentumAppRootPath
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]     [string]$source
    )

    $out = @()

    $webConfigPath = Join-Path $AumentumAppRootPath -ChildPath "web.config"

    if(! $(Test-Path -Path $webConfigPath) ) {
        $out += @($false), @($source), @("Unable to confirm existance of Aumentum web application, please check your value.('INSTALLDIR' if using xml config, 'AumentumAppRootPath' if using parameters). Using configuration value: $AumentumAppRootPath or Web.config file not exists in $AumentumAppRootPath location.")
    }
    else{
        $out += @($true), @($source), @("")
    }

    return $out
}

function IsSqlServerReachable(){
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$connectionString
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$source
    )

    try{
        $ds = QuerySqlServer -connectionString $connectionString -query "select 1 as available"

        $result = $ds.Tables[0].Rows | %{$_.available}
        if( $result -eq 1){
            $out += @($true), @($source), @("")
        }
        else{
            $out += @($false), @($source), @("Error while fetching SQL Server result, unexpected result.")
        }
    }
    catch {
        $out += @($false), @($source), @("Unable to reach SQL Server: $PSItem.Exception.Message")
    }

    return $out
}

function IsFullTextSearchInstalled(){
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$connectionString
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$source
    )

    try{
        $ds = QuerySqlServer -connectionString $connectionString -query "SELECT FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')  as fulltextinstalled"

        $result = $ds.Tables[0].Rows | %{$_.fulltextinstalled}
        if( $result -eq 1){
            $out += @($true), @($source), @("")
        }
        else{
            $out += @($false), @($source), @("SQL Server FullTextSearch need to be installed in order to install search services.")
        }
    }
    catch {
        $out += @($false), @($source), @("Unable to reach SQL Server: $PSItem.Exception.Message" )
    }

    return $out
}

function IsLumenBVSAppSettingPresent(){
    [CmdletBinding()]
    param (
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$connectionString
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$source
    )

    try{
        $ds = QuerySqlServer -connectionString $connectionString -query "SELECT count(*) as nb FROM AppSetting WHERE ShortDescr = 'LumenBVS'"

        $result = $ds.Tables[0].Rows | %{$_.nb}

        if( $result -eq 1){
            $out += @($true), @($source), @("")
        }
        else{
            $out += @($false), @($source), @("The 'LumenBVS' AppSetting do not exists, BVS cannot be configured")
        }
    }
    catch {
        $out += @($false), @($source), @("Unable to reach SQL Server: $PSItem.Exception.Message" )
    }

    return $out
}

function QuerySqlServer(){
    [CmdLetBinding()]
    param(
         [Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=0)]    [string]$connectionString
        ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true, Position=1)]    [string]$query
    )

    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connectionString
    $conn.Open()

    $cmd = New-Object System.Data.SqlClient.SqlCommand($query, $conn)
    $cmd.CommandTimeout = 30

    $ds = New-Object System.Data.DataSet
    $da = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    [void]$da.fill($ds)

    $conn.Close()
    return $ds
}
