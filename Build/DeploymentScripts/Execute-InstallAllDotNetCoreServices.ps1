<#
.SYNOPSIS
This script installs ALL Lumen Micro Services included in the ServiceNames list using the InstallDotNetCoreService.ps1 script.

.DESCRIPTION

The description is usually a longer, more detailed explanation of what the script or function does. Take as many lines as you need.
.PARAMETER SourceRootPath
The root path to all services' binary files to install.

.PARAMETER DeployRootPath
The root path where all services will be installed.

.PARAMETER AspNetCoreEnvVariable
Sets machine's ASPNETCORE_ENVIRONMENT environment variable to this value and ensures only appsettings.json and appsettings.$AspNetCoreEnvVariable.json files are included in installation.

.PARAMETER WebSiteName
The IIS web site where all service applications will be installed.

.PARAMETER AumentumSiteName
The IIS web application to use if the application is nested under the top level website.

.PARAMETER AppPoolName
The IIS AppPool name used to run the services.

.PARAMETER ServiceNames
A list of ALL service names to install. The service must be directly under the $SourceRootPath and will be used as the deployment directory service name.

.PARAMETER StepName
The step name to append the log under.

.PARAMETER UseSharedSignature
Use the same json cert file on load balancer scenario

.EXAMPLE
.\InstallAllDotNetCoreServices.ps1 -SourceRootPath .\serv_latest -DeployRootPath .\out1 -AspNetCoreEnvVariable DEV

#>
param(
     [Parameter(Mandatory=$true)]    [string]$SourceRootPath
    ,[Parameter(Mandatory=$true)]    [string]$DeployRootPath
    ,[Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]    [string]$WebSiteName
    ,[Parameter(Mandatory=$false)]   [string]$AumentumSiteName
    ,[Parameter(Mandatory=$true)]    [string]$AppPoolName
    ,[Parameter(Mandatory=$true)]    [string[]]$ServiceNames
    ,[Parameter(Mandatory=$false)]   [string]$StepName
    ,[Parameter(Mandatory=$false)]   [switch]$UseSharedSignature
)

Write-Host
Write-Host "Installing services: $StepName"
Write-Host "From $SourceRootPath to $DeployRootPath"
Write-Host "AspNetCoreEnvVariable: $AspNetCoreEnvVariable"
Write-Host "WebSiteName: $WebSiteName"
Write-Host "Aumentum Site Name: $AumentumSiteName"
Write-Host "AppPool Name: $AppPoolName"


# stop IIS to free the dll
iisreset -stop

foreach ($service in $ServiceNames)
{
    $source = Join-path $SourceRootPath -ChildPath $service
    .\Execute-InstallDotNetCoreService.ps1 -SourcePath $source -DeployPath $DeployRootPath -ServiceName $service -WebSiteName $WebSiteName -AumentumSiteName $AumentumSiteName -AppPoolName $AppPoolName -AspNetCoreEnvVariable $AspNetCoreEnvVariable -UseSharedSignature:$UseSharedSignature
}
# IIS need to be reset in order for the env variable to be reloaded
iisreset.exe -start

# Ensure application pool has access to folder
Write-Host "Granting AppPool access to: $DeployRootPath ... "  -NoNewline
Import-Module WebAdministration

$appPoolSid = (Get-ItemProperty IIS:\AppPools\$AppPoolName).applicationPoolSid
$identifier = New-Object System.Security.Principal.SecurityIdentifier $appPoolSid
$user = $identifier.Translate([System.Security.Principal.NTAccount])
$username = $user.Value
icacls.exe $DeployRootPath /q /t /grant $username`:F

Write-Host "Done!" -ForegroundColor Green

Write-Host
