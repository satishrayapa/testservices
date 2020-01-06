<#
.SYNOPSIS
This script will remove the environment variables for the provided environment

.DESCRIPTION

.PARAMETER AspNetCoreEnvVariable
Remove the machine's ASPNETCORE_ENVIRONMENT environment variable from this value

.EXAMPLE
.\Configure-RemoveEnvironmentVariables.ps1 -AspNetCoreEnvVariable DEV

#>
param(
     [Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
)

[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_common.resourceLocator.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_aumentumSecurity.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_common.security.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.assessmentevent.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.basevaluesegment.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.grmevent.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.legalparty.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.legalpartysearch.connectionString", $null, "Machine")
[Environment]::SetEnvironmentVariable($AspNetCoreEnvVariable + "_service.revenueobject.connectionString", $null, "Machine")

Write-Host "Done removing environment variable for environement: $AspNetCoreEnvVariable"
