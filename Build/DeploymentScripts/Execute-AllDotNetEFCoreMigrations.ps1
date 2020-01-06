<#
.SYNOPSIS
This script run EF Core Migration on ALL Lumen Micro Services included in the ServiceNames list using the Execute-DotNetEFCoreMigrations.ps1 script.

.DESCRIPTION

.PARAMETER ServiceRootPath
The root path to all services' binary files to install.

.PARAMETER ServiceNames
A list of ALL service names to install. The service must be directly under the $SourceRootPath and will be used as the deployment directory service name.

Default: List of known service names.

.PARAMETER AspNetCoreEnvVariable
Sets machine's ASPNETCORE_ENVIRONMENT environment variable to this value and ensures only appsettings.json and appsettings.$AspNetCoreEnvVariable.json files are included in installation.

.EXAMPLE
.\InstallAllDotNetCoreServices.ps1 -SourceRootPath .\serv_latest -DeployRootPath .\out1 -AspNetCoreEnvVariable DEV

#>
param(
     [Parameter(Mandatory=$true)]    [string]$ServiceRootPath
    ,[Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$false)]   [string]$AumentumSiteName
    ,[Parameter(Mandatory=$true)]    [string[]]$ServiceNames
    ,[Parameter(Mandatory=$false)]   [string]$StepName
)

Write-Host "Installing services: $StepName"
Write-Host "From $ServiceRootPath"
Write-Host "AspNetCoreEnvVariable: $AspNetCoreEnvVariable"
Write-Host

foreach ($service in $ServiceNames)
{
    # if(![string]::IsNullOrEmpty($AumentumSiteName) ){
    #   $source = Join-path $ServiceRootPath -ChildPath $AumentumSiteName
    #     $source = Join-path $source -ChildPath $service
    # }
    # else{
    #     $source = Join-path $ServiceRootPath -ChildPath $service
    # }

    $source = Join-path $ServiceRootPath -ChildPath $service

    .\Execute-DotNetEFCoreMigrations.ps1 -ServicePath $source -ServiceName $service -AspNetCoreEnvVariable $AspNetCoreEnvVariable
}

Write-Host "Finished EF Core Migrations"
Write-Host
