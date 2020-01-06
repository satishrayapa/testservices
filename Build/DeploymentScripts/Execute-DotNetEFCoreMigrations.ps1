<#
.SYNOPSIS
This script runs EF Core Migrations.

.DESCRIPTION

.PARAMETER ServicePath
The full path to the service's binary files.

.PARAMETER ServiceName
The service name and name of the service's directory.

.PARAMETER AspNetCoreEnvVariable
Sets machine's ASPNETCORE_ENVIRONMENT environment variable to this value and ensures only appsettings.json and appsettings.$AspNetCoreEnvVariable.json files are included in installation.

.EXAMPLE
.\Execute-DotNetEFCoreMigrations.ps1 -ServicePath c:\Lumen\Services\MyService -ServiceName MyService -AspNetCoreEnvVariable QA
#>
param(
        [Parameter(Mandatory=$true)]    [string]$ServicePath,
        [Parameter(Mandatory=$true)]    [string]$ServiceName,
        [Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
)

$ErrorActionPreference = "Stop"
Write-Host "Running EF Core migration for : $ServiceName..." #-NoNewline

# set environment
$env:ASPNETCORE_ENVIRONMENT = $AspNetCoreEnvVariable;
[Environment]::SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", $AspNetCoreEnvVariable, "Machine")
set-item "env:ASPNETCORE_ENVIRONMENT" $AspNetCoreEnvVariable

# run EF Core migrations
$startTime = (get-date)


    try
    {
    #Getting default settings for Windows Error Reporting
        $WER=Get-WindowsErrorReporting

    }
    catch
    {
		Write-host "WER default value not found. Disiabling Windows Error Reporting value"
        Disable-WindowsErrorReporting
        $WER=Get-WindowsErrorReporting
    }


try
{

    #Disabling Windows Error Reporting value if it is enbled
    if ($WER -eq  "Enabled")
    {
        Disable-WindowsErrorReporting
    }

    Push-Location
    $operationsPath = Join-Path $ServicePath -ChildPath "Operations"
    Set-Location $($operationsPath)

    $service = Get-ChildItem $operationsPath -Filter "*.Operations.dll"
    $operationService = $service.Name | ? { $_ -ne "TAGov.Common.Operations.dll"} 
    dotnet $operationService --ef-migrate --appsettings-directory $ServicePath
	


	Pop-Location

}
catch {
    Write-host "EF Core Migrations FAILED!" -ForegroundColor Red
    Write-host $_.Exception.Message -ForegroundColor Red
    throw
}
finally
{
    #Enabling Windows Error Reporting value if it was enbleded before EF Core migratrion
    if ($WER -eq  "Enabled")
    {
        Enable-WindowsErrorReporting
    }
}



$diff = (new-timespan -Start $startTime -End (get-date))
Write-Host "Elapsed time: $diff"
Write-Host
