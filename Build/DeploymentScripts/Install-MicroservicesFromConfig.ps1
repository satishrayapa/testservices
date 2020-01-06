<#
.SYNOPSIS
This script install and configure the lumen Microservices based on the Aumentum Configuration file

.DESCRIPTION

.PARAMETER SourceRootPath
Path to the folder containing the Aumentum XML Configuration file

.PARAMETER PropertiesEnvironmentName
The Environment Variable to use, this will set the environment variable and create the required files
#>

param(
    [Parameter(Mandatory=$true)]   [string]$PropertyFileName
   ,[Parameter(Mandatory=$true)]   [string]$PropertiesEnvironmentName
   ,[Parameter(Mandatory=$false)]  [string]$AspNetCoreEnvVariable = ""
)

# Set current dir
Set-location $PSScriptRoot
Import-Module $PSScriptRoot\lib\SqlHelpers.psm1

# $mapper = { configName, {configValue, isBoolean} }
$mapper = @{}
$mapper.Add("INSTALLDIR", $( @{ name ='AumentumAppRootPath', $false}) ) ## NOT SURE FOR THIS GUY
$mapper.Add("WEBSITE", $( @{ name ='WebSiteName', $false}) )
$mapper.Add("WEBAPPLICATION", $( @{ name ='AumentumSiteName', $false}) )
$mapper.Add("USEHTTPS", $( @{ name ='UseHTTP', $true}) ) ## need to switch value
$mapper.Add("SERVICES_SOURCEROOTPATH", $( @{ name ='SourceRootPath', $false}) )
$mapper.Add("SERVICES_SERVICEROOTPATH", $( @{ name ='ServiceRootPath', $false}) )
$mapper.Add("SERVICES_WINSERVICEPATH", $( @{ name ='WinServicePath', $false}) )
$mapper.Add("SERVICES_SERVERNAME", $( @{ name ='ServerName', $false}) )
$mapper.Add("SERVICES_SECURECONNECTIONSTRING", $( @{ name ='StoreConnectionStringInEnvironmentVariable', $true}) )
$mapper.Add("SERVICES_ENABLEBVS", $( @{ name ='EnableBVS', $true}) )
$mapper.Add("SERVICES_ENABLESEARCH", $( @{ name ='EnableSearch', $true}) )
$mapper.Add("SERVICES_USELOCALRESOURCELOCATOR", $( @{ name ='UseLocalResourceLocator', $true}) )
$mapper.Add("SERVICES_USESHAREDSIGNATURE", $( @{ name ='UseSharedSignature', $true}) )
$mapper.Add("SERVICES_SERVERROLE", $( @{ name ='ServerRole', $false}) )
$mapper.Add("SERVICES_DBCOMMANDTIMEOUT", $( @{ name ='DbCommandTimeout', $false}) )
$mapper.Add("SERVICES_ENABLEHEALTHCHECK", $( @{ name ='EnableHealthCheck', $true}) )
$mapper.Add("SERVICES_ASPNETCOREREQUESTTIMEOUT", $( @{ name ='AspNetCoreRequestTimeout', $false}) )
$mapper.Add("SERVICES_ENABLEWORKLIST", $( @{ name ='EnableWorklist', $true}) )
$mapper.Add("SERVICES_ENABLEDNSRESOLUTION", $( @{ name ='EnableDNSResolution', $true}) )

$mapper.Add("SERVICES_EXTERNALURI_DNSNAME", $( @{ name ='ExternalUriDNSName', $false}) )
$mapper.Add("SERVICES_EXTERNALURI_USEHTTPS", $( @{ name ='ExternalUriUseHTTPS', $true}) )


$dbMapper = @{}
$dbMapper.Add("DS", "Server")
$dbMapper.Add("DB", "DbName")
$dbMapper.Add("DBUSER", "User")
$dbMapper.Add("DBPASSWORD", "Password")
$dbMapper.Add("DBTIMEOUT", "TimeOut")
$dbMapper.Add("DBTRUSTED", "UseIntegratedSecurity")


$configs = @{}
## Add defaults:
$configs.Add("AspNetCoreEnvVariable", $AspNetCoreEnvVariable)
$configs.Add("UpdateWebConnectionString", $false)
$configs.Add("AumentumWorkflowURL", "")
$configs.Add("AssignAdminRoles", $false)

$dbConfigs = @{}

$isValidConfig = $true

## Open and read xml file
[xml] $propertiesXml = Get-Content -Path $propertyFileName
$environmentProperties = $propertiesXml.PropertyGroup.Environment | Where-Object { $_.Name -eq $propertiesEnvironmentName} | Select-Object -Property Property

try{
    # Read all config
    $environmentProperties.Property | ForEach-Object {
        if ($mapper.Contains($_.Name))
        {
            $value = $_.InnerXml
            if($mapper.Item($_.Name).Item("name").Item(1)){
                if ( [string]::IsNullOrEmpty($value) -and ($_.Name -eq "SERVICES_EXTERNALURI_USEHTTPS") ) {
                    #skip if parameter is blank and keep reverse compatibility for other parameters
                }
                else{
                    $value = [System.Convert]::ToBoolean($value)
                }
            }
            $configs.Add( $mapper.Item($_.Name).Item("name").Item(0), $value)
        }
    }
}
Catch{
    $isValidConfig = $false
    Write-Error -Message $PSItem.ToString() -ErrorAction Stop
}

try{
    # Recreate connectionstring
    $environmentProperties.Property | ForEach-Object {
        if ($dbMapper.Contains($_.Name))
        {
            $value = $_.InnerXml
            if($_.Name -eq "DBTRUSTED"){
                $value = [System.Convert]::ToBoolean($value)
            }
            $dbConfigs.Add( $dbMapper.Get_Item($_.Name), $value)
        }
    }
}
catch{
    $isValidConfig = $false
    Write-Error -Message $PSItem.Exception.Message -ErrorAction Stop
}

$useIS = [boolean]$dbConfigs.Get_Item("UseIntegratedSecurity")
$connectionString = BuildConnectionString -Server $dbConfigs.Get_Item("server") -DbName $dbConfigs.Get_Item("DbName") -User $dbConfigs.Get_Item("User") -Password $dbConfigs.Get_Item("Password") -TimeOut $dbConfigs.Get_Item("TimeOut") -UseIntegratedSecurity:$useIS
$configs.Add("connectionString", $connectionString)
$configs.Add("DbName", $dbConfigs.Get_Item("DbName"))

## Reverse use http
$configs.Set_Item("UseHTTP", !$configs.Get_Item("UseHTTP"))

## Fix Aumentum App Root Path
$fullPath = Join-Path $configs.Get_Item("AumentumAppRootPath") -ChildPath $configs.Get_Item("AumentumSiteName")
$configs.Set_Item("AumentumAppRootPath", $fullPath)

#$configs.GetEnumerator() | Sort-Object Name

if($isValidConfig){
    .\execute-install.ps1 -configs $configs
}
else{
    Write-Host "Invalid configuration detected, unable to continue" -ForegroundColor Red
}
