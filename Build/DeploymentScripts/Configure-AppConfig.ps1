##############################
#.SYNOPSIS
# Add or Update key/value of the search windows services
#
#.DESCRIPTION
# Add or Update key/value of the search windows services
#
#.PARAMETER ServiceRootPath
# Path where the services are installed
#
#.PARAMETER AspNetCoreEnvVariable
# Environment variable to use for the resource locator
#
#.PARAMETER SecurityURI
# URI for the Common.Security service
#
#.PARAMETER ResourceLocatorURI
# URI for the Resource Locator
#
#.PARAMETER ConnectionString
# DB Connection string
#
#.PARAMETER EnableSSL
# Switch to indicate if HTTPS is used or not
#
#.PARAMETER UpdaterCRONSchedule
# CRON format value to define when the update job should be running
#
# Ref: https://github.com/HangfireIO/Cronos
# Cron expression is a mask to define fixed times, dates and intervals. The mask consists of second (optional), minute, hour, day-of-month, month and day-of-week fields.
# All of the fields allow you to specify multiple values, and any given date/time will satisfy the specified Cron expression, if all the fields contain a matching value.
#
#  ┌───────────── second (optional)       0-59              * , - /
#  │ ┌───────────── minute                0-59              * , - /
#  │ │ ┌───────────── hour                0-23              * , - /
#  │ │ │ ┌───────────── day of month      1-31              * , - / L W ?
#  │ │ │ │ ┌───────────── month           1-12 or JAN-DEC   * , - /
#  │ │ │ │ │ ┌───────────── day of week   0-6  or SUN-SAT   * , - / # L ?                Both 0 and 7 means SUN
#  │ │ │ │ │ │
#  * * * * * *
#
#
#.EXAMPLE
# .\Configure-AppConfig -ServiceRootPath $WinServicePath -AspNetCoreEnvVariable $AspNetCoreEnvVariable -SecurityURI $SecurityURI -ResourceLocatorURI $ResourceLocatorURI -ConnectionString $ConnectionString -EnableSSL:$enableSSL
#
#.NOTES
#
##############################
param(
     [Parameter(Mandatory=$true)]    [string] $ServiceRootPath
    ,[Parameter(Mandatory=$true)]    [string] $AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]    [string] $SecurityURI
    ,[Parameter(Mandatory=$true)]    [string] $ResourceLocatorURI
    ,[Parameter(Mandatory=$true)]    [string] $ConnectionString
    ,[Parameter(Mandatory=$false)]   [Switch] $EnableSsl
    ,[Parameter(Mandatory=$false)]   [string] $UpdaterCRONSchedule = "0 0 * * *"
)

Import-Module $PSScriptRoot\lib\ConfigurationFileHelper.psm1

Write-Host "Updating App.config..."
if($EnableSsl.IsPresent){
    $sslEnabled = $true
}else{
    $sslEnabled = $false
}
try{
    ## LegalPartySearch.Coordinator
    $configName = "TAGov.Process.Sync.LegalPartySearch.Coordinator.exe.config"
    $WindowsServicePath = Join-Path $ServiceRootPath -ChildPath "Process.Sync.LegalPartySearch.Coordinator"
    write-host "Updating: $WindowsServicePath : $configName"

    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "TAGov.Common.Security.Authority" -value "$($SecurityURI.ToLower())"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "TAGov.Common.Security.ServiceClientId" -value "service.tagov.search"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "TAGov.Common.Security.ServiceClientPassword" -value "password"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "TAGov.Common.Security.ServiceClientScope" -value "api.common.resourcelocator api.service.legalpartysearch"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "TAGov.Common.Security.DisableRequireHttps" -value "$(!$sslEnabled)"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "Common.ResourceLocator.ExternalUri" -value "$ResourceLocatorURI"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "Common.ResourceLocator.Uri" -value "$ResourceLocatorURI"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "Common.ResourceLocator.Partition" -value "$AspNetCoreEnvVariable"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "useProdValue" -value "True"
    AddOrUpdateAppConfigAppSetting -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "dailyupdaterruncron" -value "$UpdaterCRONSchedule"

    AddOrUpdateAppConfigConnectionString -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "aumentum" -value $ConnectionString

    # ## LegalPartySearch
    # write-host "Updating: $WindowsServicePath"
    # $configName = "TAGov.Process.Sync.LegalPartySearch.exe.config"
    # $WindowsServicePath = Join-Path $ServiceRootPath -ChildPath "Process.Sync.LegalPartySearch"

    # AddOrUpdateAppConfigConnectionString -ConfigFilePath $WindowsServicePath -ConfigFileName $configName -key "aumentum" -value $ConnectionString
    Write-Host "Done updating App.config." -ForegroundColor Green
}
catch{
    Write-Host "Failed updating Services App.Config" -ForegroundColor Red
    throw
}


