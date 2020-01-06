<#
.SYNOPSIS
This script update the appsetting.environment.json config file on ALL Lumen Micro Services included in the ServiceNames list.

.DESCRIPTION

.PARAMETER ServiceRootPath
The root path to all services' binary files to install.

.PARAMETER AspNetCoreEnvVariable
Sets machine's ASPNETCORE_ENVIRONMENT environment variable to this value and ensures only appsettings.json and appsettings.$AspNetCoreEnvVariable.json files are included in installation.

.PARAMETER ServerIP
The server IP Adress to update, if not provided, it will attemp to auto-configure

.PARAMETER ServiceNames
A list of ALL service names to install. The service must be directly under the $SourceRootPath and will be used as the deployment directory service name.
Default: List of known service names.

.EXAMPLE
.\ConfigureAppSettingsWithServerIP.ps1 -AspNetCoreEnvVariable DEV

#>
param(
     [Parameter(Mandatory=$true)]    [string]$ServiceRootPath
    ,[Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]    [string]$ServerName
    ,[Parameter(Mandatory=$true)]    [string]$aumentumDbConnectionString
    ,[Parameter(Mandatory=$true)]    [string]$AumentumDbCommandTimeout
    ,[Parameter(Mandatory=$true)]    [string]$AumentumDbName
    ,[Parameter(Mandatory=$true)]    [string]$Protocol
    ,[Parameter(Mandatory=$false)]   [string]$AumentumSiteName
    ,[Parameter(Mandatory=$True)]    [string[]]$ServiceNames
    ,[Parameter(Mandatory=$false)]   [switch]$IncludeServiceNamesInEndpoint
    ,[Parameter(Mandatory=$false)]   [switch]$StoreConnectionStringInEnvironmentVariable
)

$deployConfigFilename = "appsettings.deploy.json"
$environmentConfigFilename = "appsettings.$AspNetCoreEnvVariable.json"

Write-Host "Updating $environmentConfigFilename files"
Write-Host "serverName: " $serverName

try{
    foreach ($service in $ServiceNames)
    {
        Write-Host "Updating $service"

        $source = Join-path $ServiceRootPath -ChildPath $service

        $fullpath = Join-path $source -ChildPath $deployConfigFilename


        if(![string]::IsNullOrEmpty($AumentumSiteName) ){
            $currentService = "/$AumentumSiteName/$service"
            $securityService = "/$AumentumSiteName/common.security"
            $AssessmentEventService = "/$AumentumSiteName/Service.AssessmentEvent/"
            $RevenueObjectService = "/$AumentumSiteName/Service.RevenueObject/"
            $LegalPartyService = "/$AumentumSiteName/Service.LegalParty/"
            $BaseValueSegmentService = "/$AumentumSiteName/Service.BaseValueSegment/"
            $GrmEventService = "/$AumentumSiteName/Service.GrmEvent/"
        }
        elseif($IncludeServiceNamesInEndpoint.IsPresent){
            $currentService = "/$service"
            $securityService = "/common.security"
            $AssessmentEventService = "/Service.AssessmentEvent/"
            $RevenueObjectService = "/Service.RevenueObject/"
            $LegalPartyService = "/Service.LegalParty/"
            $BaseValueSegmentService = "/Service.BaseValueSegment/"
            $GrmEventService = "/Service.GrmEvent/"
        }
        else{
            $currentService = "/"
            $securityService = ""
            $AssessmentEventService = "/"
            $RevenueObjectService = "/"
            $LegalPartyService = "/"
            $BaseValueSegmentService = "/"
            $GrmEventService = "/"
        }

        #Common value
        $valueAumentumDbConnectionString = $aumentumDbConnectionString.Replace("\", "\\")
        $valueIdentityDbConnectionString = $aumentumDbConnectionString.Replace("Database=$AumentumDbName","Database=$AumentumDbName-identity").Replace("\", "\\")

        (Get-Content $fullpath).replace('{{serverIP}}', $serverName) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{currentService}}', $currentService) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{aumentumDbCommandTimeout}}', $AumentumDbCommandTimeout) | Set-Content $fullpath
        if($StoreConnectionStringInEnvironmentVariable.IsPresent){
            (Get-Content $fullpath).replace('{{aumentumDbConnectionString}}', '') | Set-Content $fullpath
            (Get-Content $fullpath).replace('{{identityDbConnectionString}}', '') | Set-Content $fullpath
        }
        else{
            (Get-Content $fullpath).replace('{{aumentumDbConnectionString}}', $valueAumentumDbConnectionString) | Set-Content $fullpath
            (Get-Content $fullpath).replace('{{identityDbConnectionString}}', $valueIdentityDbConnectionString) | Set-Content $fullpath
        }
        (Get-Content $fullpath).replace('{{protocol}}', $Protocol) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{securityService}}', $securityService.ToLower()) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{AssessmentEventService}}', $AssessmentEventService) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{RevenueObjectService}}', $RevenueObjectService) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{LegalPartyService}}', $LegalPartyService) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{BaseValueSegmentService}}', $BaseValueSegmentService) | Set-Content $fullpath
        (Get-Content $fullpath).replace('{{GrmEventService}}', $GrmEventService) | Set-Content $fullpath

        #fix double / issues
        (Get-Content $fullpath).replace('//swagger/', "/swagger/") | Set-Content $fullpath

        # rename file to match environment
        $environmentFullpath = Join-path $source -ChildPath $environmentConfigFilename
        Rename-Item $fullpath $environmentFullpath
    }

    if($StoreConnectionStringInEnvironmentVariable.IsPresent){

        $Env:ASPNETCORE_ENVIRONMENT = $AspNetCoreEnvVariable
        [Environment]::SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", $AspNetCoreEnvVariable, "Machine")
        set-item "env:ASPNETCORE_ENVIRONMENT" $AspNetCoreEnvVariable

        #Common.ResourceLocator
        $nameResourceLocator = $AspNetCoreEnvVariable + "_common.resourceLocator.connectionString"
        set-item "env:$nameResourceLocator" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameResourceLocator, $valueAumentumDbConnectionString, "Machine")

        #Common.Security
        $nameAumentumSecurity = $AspNetCoreEnvVariable + "_aumentumSecurity.connectionString"
        $nameCommonSecurity = $AspNetCoreEnvVariable + "_common.security.connectionString"
        set-item "env:$nameAumentumSecurity" $valueAumentumDbConnectionString
        set-item "env:$nameCommonSecurity"   $valueIdentityDbConnectionString
        [Environment]::SetEnvironmentVariable($nameAumentumSecurity, $valueAumentumDbConnectionString, "Machine")
        [Environment]::SetEnvironmentVariable($nameCommonSecurity, $valueIdentityDbConnectionString, "Machine")

        #Service.AssessmentEvent
        $nameAssessmentevent = $AspNetCoreEnvVariable + "_service.assessmentevent.connectionString"
        set-item "env:$nameAssessmentevent" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameAssessmentevent, $valueAumentumDbConnectionString, "Machine")

        #Service.BaseValueSegment
        $nameBaseValueSegment = $AspNetCoreEnvVariable + "_service.basevaluesegment.connectionString"
        set-item "env:$nameBaseValueSegment" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameBaseValueSegment, $valueAumentumDbConnectionString, "Machine")

        #Service.GRMEvent
        $nameGrmEvent = $AspNetCoreEnvVariable + "_service.grmevent.connectionString"
        set-item "env:$nameGrmEvent" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameGrmEvent, $valueAumentumDbConnectionString, "Machine")

        #Service.LegalParty
        $nameLegalParty = $AspNetCoreEnvVariable + "_service.legalparty.connectionString"
        set-item "env:$nameLegalParty" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameLegalParty, $valueAumentumDbConnectionString, "Machine")

        #Service.LegalPartySearch
        $nameLegalPartySearch = $AspNetCoreEnvVariable + "_service.legalpartysearch.connectionString"
        set-item "env:$nameLegalPartySearch" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameLegalPartySearch, $valueAumentumDbConnectionString, "Machine")

        #Service.RevenueObject
        $nameRevenueObject = $AspNetCoreEnvVariable + "_service.revenueobject.connectionString"
        set-item "env:$nameRevenueObject" $valueAumentumDbConnectionString
        [Environment]::SetEnvironmentVariable($nameRevenueObject, $valueAumentumDbConnectionString, "Machine")

        #remove lines in config files
        # $(get-content $fullpath | select-string -pattern '{{aumentumDbConnectionString}}' -notmatch )| Set-Content $fullpath
        # $(get-content $fullpath | select-string -pattern '{{identityDbConnectionString}}' -notmatch )| Set-Content $fullpath
    }

    Write-Host "Done updating environment config." -ForegroundColor Green
}
catch{
    Write-Host "Failed updating environment config." -ForegroundColor Red
    throw
}
Write-host
