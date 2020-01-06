<#
.SYNOPSIS
This script install and configure the lumen Microservices

.DESCRIPTION

.PARAMETER configs
Dictionnary containing values required for configuration

#>
param(
    [Parameter(Mandatory=$true)]   [hashtable]$configs
)
## Enable logging
$logFileName = $PSScriptRoot + "\" + (Get-ChildItem ("$PSScriptRoot\" + $MyInvocation.MyCommand.Name) | ForEach-Object {$_.BaseName}) + "-" + (Get-Date -Format "yyyy-MM-dd-hhmmss") + ".log"
Start-Transcript -Path $logFileName -Append -Force

Set-StrictMode -Version latest

# Set current dir
Set-location  $PSScriptRoot
Import-Module $PSScriptRoot\lib\LogHelper.psm1 -Force
Import-Module $PSScriptRoot\lib\configBuilder.psm1 -Force
Import-Module $PSScriptRoot\lib\ValidationHelper.psm1 -Force

$newLine = "`r`n"

try{
    $systemInformation = Get-CimInstance Win32_OperatingSystem | Select-Object  Caption, InstallDate, ServicePackMajorVersion, OSArchitecture, BuildNumber, CSName | Format-List
    $networkInformation = (Get-NetIPConfiguration).IPv4Address.IpAddress
    Write-Host "System Information: "
    Write-Host ($systemInformation | Format-List | Out-String )
    Write-Host "Server IP: " ($networkInformation | Format-List | Out-String )
}
catch{
    Write-Host "Impossible to get system informations"
}

$configs = BuildConfigurations -configs $configs

## Common variables setup
$CommonService = @('Common.ResourceLocator', 'Common.Security', 'Service.LegalParty')
$BVSService = @('Facade.AssessmentHeader', 'Facade.BaseValueSegment', 'Service.AssessmentEvent', 'Service.BaseValueSegment', 'Service.GrmEvent', 'Service.RevenueObject')
$SearchService = @('Service.LegalPartySearch')
$WorkListService = @('Service.MyWorklistSearch')

###########################################################
## Run validations
$isValid = ValidateParams $configs

$windowsServiceWarning =  $( IsWinServiceOutsideWWW $configs.Get_Item("WinServicePath") )
if(![string]::IsNullOrEmpty($windowsServiceWarning)){
    Write-Host
    Write-Host "---------------------------------------!!! WARNING !!!---------------------------------------" -ForegroundColor Black -BackgroundColor Yellow
    Write-Host
    Write-Host $windowsServiceWarning -ForegroundColor Yellow
    Write-Host
    Write-Host "---------------------------------------------------------------------------------------------" -ForegroundColor Black -BackgroundColor Yellow
    Write-Host
}

if(!$isValid){
    Write-Error "Validation failed. Fix your configuration file before going further."
    $LASTEXITCODE = -1
}
else{
    ###########################################################
    $msgConfigServerRole = "Install as server role: " + $configs.Get_Item("ServerRole")
    $msgConfigServername = "Using servername: " + $configs.Get_Item("ServerDNSName")
    $msgConfigEnvVariable = "Using environment: " + $configs.Get_Item("AspNetCoreEnvVariable")
    $msgConfigProtocol = "Using Protocol: " + $configs.Get_Item("protocol")
    $msgConfigBVS = "BVS is enabled:" + $configs.Get_Item("EnableBVS")
    $msgConfigSearch = "Search is enabled: " +  $configs.Get_Item("EnableSearch")
    $msgConfigWorkList = "Worklist Search is enabled: " +  $configs.Get_Item("EnableWorklist")


    $currentConfig = $msgConfigServerRole + $newLine + $msgConfigServername + $newLine  + $msgConfigEnvVariable + $newLine  + $msgConfigProtocol + $newLine  +  $msgConfigBVS + $newLine + $msgConfigSearch + $newLine + $msgConfigWorkList

    WriteEventLogHelper -Message "Aumentum Microservices installation Started"
    WriteEventLogHelper -Message $currentConfig

    Write-Host $msgConfigServerRole
    Write-Host $msgConfigServername
    Write-Host $msgConfigEnvVariable
    Write-Host $msgConfigProtocol
    Write-Host
    Write-Host "Deploying Microservices: "
    if( $configs.Get_Item("EnableBVS") ){
        Write-Host "-BVS Services"
    }

    if( $configs.Get_Item("EnableSearch")){
        Write-Host "-Search Services"
    }

    if( $configs.Get_Item("EnableWorklist")){
        Write-Host "-Worklist Search Services"
    }

    Write-Host

    ##############################################
    # Configure workflow URL
    ##############################################
    if(![string]::IsNullOrEmpty( $configs.Get_Item("aumentumWorkflowURL")) -AND $configs.Get_Item("IsPrimary")){
        Write-Host "Configuring Aumentum Workflow using URL: " $configs.Get_Item("aumentumWorkflowURL")
        .\Configure-SetWorkflowRootUrl.ps1 -workflowURL $configs.Get_Item("aumentumWorkflowURL") -ConnectionString $configs.Get_Item("connectionString")
    }

    ##############################################
    ## Common Services Install
    ##############################################
    .\Execute-InstallAllDotNetCoreServices.ps1 -SourceRootPath $configs.Get_Item("SourceRootPath") -DeployRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName") -AppPoolName $configs.Get_Item("AppPoolName") -ServiceNames $CommonService -StepName "Common Services" -UseSharedSignature:$configs.Get_Item("UseSharedSignature")
    .\Configure-EnvironmentAppSettings.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServerName $configs.Get_Item("ServerDNSName") -aumentumDbConnectionString $configs.Get_Item("connectionString") -AumentumDbCommandTimeout $configs.Get_Item("DbCommandTimeout") -AumentumDbName $configs.Get_Item("DbName") -Protocol $configs.Get_Item("Protocol") -ServiceNames $CommonService -IncludeServiceNamesInEndpoint -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StoreConnectionStringInEnvironmentVariable:$configs.Get_Item("StoreConnectionStringInEnvironmentVariable")
    if($configs.Get_Item("IsPrimary")){
        .\Execute-AllDotNetEFCoreMigrations.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServiceNames $CommonService -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StepName "Common Services"
    }

    ##############################################
    ## BVS Services Install
    ##############################################
    if($configs.Get_Item("EnableBVS"))
    {
        .\Execute-InstallAllDotNetCoreServices.ps1 -SourceRootPath $configs.Get_Item("SourceRootPath") -DeployRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName") -AppPoolName $configs.Get_Item("AppPoolName") -ServiceNames $BVSService -StepName "BVS Services"
        .\Configure-EnvironmentAppSettings.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServerName $configs.Get_Item("ServerDNSName") -aumentumDbConnectionString $configs.Get_Item("connectionString") -AumentumDbCommandTimeout $configs.Get_Item("DbCommandTimeout") -AumentumDbName $configs.Get_Item("DbName") -Protocol $configs.Get_Item("Protocol") -ServiceNames $BVSService -IncludeServiceNamesInEndpoint -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StoreConnectionStringInEnvironmentVariable:$configs.Get_Item("StoreConnectionStringInEnvironmentVariable")
        if($configs.Get_Item("IsPrimary")){
            .\Configure-ToggleBVSCalculation.ps1 -state $configs.Get_Item("EnableBVS") -ConnectionString $configs.Get_Item("connectionString")
            .\Execute-AllDotNetEFCoreMigrations.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServiceNames $BVSService -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StepName "BVS Services"
        }
    }

    ##############################################
    ## Search Services Install
    ##############################################
	 if([string]::IsNullOrEmpty($configs.Get_Item("AumentumSiteName"))){
            $windowServiceName = $configs.Get_Item("WebSiteName")
        }
        else{
            $windowServiceName = $configs.Get_Item("AumentumSiteName")
        }

    if( $configs.Get_Item("EnableSearch")){
        .\Execute-InstallAllDotNetCoreServices.ps1 -SourceRootPath $configs.Get_Item("SourceRootPath") -DeployRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName") -AppPoolName $configs.Get_Item("AppPoolName") -ServiceNames $SearchService -StepName "Search Services"
        .\Configure-EnvironmentAppSettings.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServerName $configs.Get_Item("ServerDNSName") -aumentumDbConnectionString $configs.Get_Item("connectionString") -AumentumDbCommandTimeout $configs.Get_Item("DbCommandTimeout") -AumentumDbName $configs.Get_Item("DbName") -Protocol $configs.Get_Item("Protocol") -ServiceNames $SearchService -IncludeServiceNamesInEndpoint -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StoreConnectionStringInEnvironmentVariable:$configs.Get_Item("StoreConnectionStringInEnvironmentVariable")

		if($configs.Get_Item("IsPrimary")){
            .\Execute-AllDotNetEFCoreMigrations.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServiceNames $SearchService -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StepName "Search Services"

			#Install windows service is in only primary server
			# Install windows service
			.\Execute-InstallWindowsService -SourcePath $configs.Get_Item("SourceRootPath") -DeployPath $configs.Get_Item("WinServicePath") -ServiceFolder $configs.Get_Item("LegalPartySearchCoordinatorFolder") -ServiceName $configs.Get_Item("LegalPartySearchCoordinatorExecutable") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WindowServiceName $windowServiceName

			 ## update services app.config files
			.\Configure-AppConfig.ps1 -ServiceRootPath $configs.Get_Item("WinServicePath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -SecurityURI $configs.Get_Item("SecurityURI") -ResourceLocatorURI $configs.Get_Item("ResourceLocatorURI") -ConnectionString $configs.Get_Item("ConnectionString") -EnableSSL:$configs.Get_Item("enableSSL")

			## start windows service
			.\Execute-StartWindowsService.ps1 -DeployPath $configs.Get_Item("WinServicePath") -ServiceFolder $configs.Get_Item("LegalPartySearchCoordinatorFolder") -ServiceName $configs.Get_Item("LegalPartySearchCoordinatorExecutable") -WindowServiceName $windowServiceName
			}
			else
			{
				# Uninstall windows service in secondary server
				.\Execute-UnInstallWindowsService -SourcePath $configs.Get_Item("SourceRootPath") -DeployPath $configs.Get_Item("WinServicePath") -ServiceFolder $configs.Get_Item("LegalPartySearchCoordinatorFolder") -ServiceName $configs.Get_Item("LegalPartySearchCoordinatorExecutable") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WindowServiceName $windowServiceName
			}
    }
    else
    {
        # Uninstall windows service when Search is disabled
        .\Execute-UnInstallWindowsService -SourcePath $configs.Get_Item("SourceRootPath") -DeployPath $configs.Get_Item("WinServicePath") -ServiceFolder $configs.Get_Item("LegalPartySearchCoordinatorFolder") -ServiceName $configs.Get_Item("LegalPartySearchCoordinatorExecutable") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WindowServiceName $windowServiceName
    }

     ##############################################
    ## Worklist Services Install
    ##############################################
    if( $configs.Get_Item("EnableWorklist")){
        .\Execute-InstallAllDotNetCoreServices.ps1 -SourceRootPath $configs.Get_Item("SourceRootPath") -DeployRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName") -AppPoolName $configs.Get_Item("AppPoolName") -ServiceNames $WorkListService -StepName "Worklist Services"
        .\Configure-EnvironmentAppSettings.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServerName $configs.Get_Item("ServerDNSName") -aumentumDbConnectionString $configs.Get_Item("connectionString") -AumentumDbCommandTimeout $configs.Get_Item("DbCommandTimeout") -AumentumDbName $configs.Get_Item("DbName") -Protocol $configs.Get_Item("Protocol") -ServiceNames $WorkListService -IncludeServiceNamesInEndpoint -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StoreConnectionStringInEnvironmentVariable:$configs.Get_Item("StoreConnectionStringInEnvironmentVariable")
        if($configs.Get_Item("IsPrimary")){
            .\Execute-AllDotNetEFCoreMigrations.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -ServiceNames $WorkListService -AumentumSiteName $configs.Get_Item("AumentumSiteName") -StepName "Worklist Services"
        }
    }

    ##############################################
    ## Post-Install Steps
    ##############################################

    ## Update web.config
    if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
        Write-Host "Using ExternalURI: " + $configs.Get_Item("ResourceLocatorExternalURI")
        .\Configure-WebConfig.ps1 -AumentumAppRootPath $configs.Get_Item("AumentumAppRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -SecurityURI $configs.Get_Item("SecurityURI") -ResourceLocatorURI $configs.Get_Item("ResourceLocatorURI") -ResourceLocatorExternalURI $configs.Get_Item("ResourceLocatorExternalURI") -ClientScope $configs.Get_Item("clientScope") -UpdateWebConnectionString:$configs.Get_Item("UpdateWebConnectionString") -ConnectionString $configs.Get_Item("ConnectionString") -EnableSSL:$configs.Get_Item("enableSSL") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName")
    }
    else{
        .\Configure-WebConfig.ps1 -AumentumAppRootPath $configs.Get_Item("AumentumAppRootPath") -AspNetCoreEnvVariable $configs.Get_Item("AspNetCoreEnvVariable") -SecurityURI $configs.Get_Item("SecurityURI") -ResourceLocatorURI $configs.Get_Item("ResourceLocatorURI") -ClientScope $configs.Get_Item("clientScope") -UpdateWebConnectionString:$configs.Get_Item("UpdateWebConnectionString") -ConnectionString $configs.Get_Item("ConnectionString") -EnableSSL:$configs.Get_Item("enableSSL") -WebSiteName $configs.Get_Item("WebSiteName") -AumentumSiteName $configs.Get_Item("AumentumSiteName")
    }

    ## Push Configurations to resource locator endpoint
    if($configs.Get_Item("IsPrimary")){

        if($configs.Get_Item("UseLocalResourceLocator")){
            $rsURI = $configs.Get_Item("BypassedResourceLocatorURI")
        }
        else{
            $rsURI = $configs.Get_Item("ResourceLocatorURI")
        }

        if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
            .\Configure-ResourceLocatorTable.ps1 -ResourceLocatorURI $rsURI -ServerName $configs.Get_Item("ExternalUriDNSName") -Partition $configs.Get_Item("AspNetCoreEnvVariable") -Protocol $configs.Get_Item("externaluriprotocol") $configs.Get_Item("EnableBVS") -EnableSearch $configs.Get_Item("EnableSearch") -AumentumSiteName $configs.Get_Item("AumentumSiteName")
        }
        else{
            .\Configure-ResourceLocatorTable.ps1 -ResourceLocatorURI $rsURI -ServerName $configs.Get_Item("ServerDNSName") -Partition $configs.Get_Item("AspNetCoreEnvVariable") -Protocol $configs.Get_Item("protocol") -EnableBVS $configs.Get_Item("EnableBVS") -EnableSearch $configs.Get_Item("EnableSearch") -AumentumSiteName $configs.Get_Item("AumentumSiteName")
        }
    }

    ##############################################
    # Add Service role - need to be after the ef core migrations
    ##############################################
    if($configs.Get_Item("IsPrimary") -and $configs.Get_Item("AssignAdminRoles")){
        .\Configure-AddServicesRoles.ps1 -username admin -ConnectionString $configs.Get_Item("connectionString")
    }

    ## Adding Common services for setting aspNetCore requestTimeout
    $ServiceNames = @('Common.ResourceLocator', 'Common.Security')

    ## Checking and including Search services for setting aspNetCore requestTimeout
    if($configs.Get_Item("EnableSearch")){
        $ServiceNames += $SearchService
    }

     ## Checking and including BVS services for setting aspNetCore requestTimeout
     if($configs.Get_Item("EnableWorklist")){
        $ServiceNames += $WorkListService
    }

    ## Checking and including BVS services for setting aspNetCore requestTimeout
    if($configs.Get_Item("EnableBVS")){
        $ServiceNames += @('Service.LegalParty', 'Service.AssessmentEvent', 'Service.BaseValueSegment', 'Service.GrmEvent', 'Service.RevenueObject','Facade.BaseValueSegment','Facade.AssessmentHeader')
    }

       ## Setting aspNetCore requestTimeout in web.config for all services
    .\Configure-AspNetCoreRequestTimeout.ps1 -ServiceRootPath $configs.Get_Item("ServiceRootPath") -ServiceNames $ServiceNames -AspNetCoreRequestTimeout $configs.Get_Item("AspNetCoreRequestTimeout")

    ## force IISReset
    iisreset

    write-host "Healthcheck status: " $configs.Get_Item("EnableHealthCheck")
    ## Service Health Check
    if($configs.Get_Item("EnableHealthCheck"))
    {
        if($configs.Get_Item("UseLocalResourceLocator")){
            $healthcheckServer = "localhost"
        }else{
            $healthcheckServer = $configs.Get_Item("ServerDNSName")
        }

        if([string]::IsNullOrEmpty($configs.Get_Item("AumentumSiteName"))){
            .\Service-HealthCheck.ps1 -ServiceNames $ServiceNames -Servername $healthcheckServer -Protocol $configs.Get_Item("protocol")
        }
        else{
            .\Service-HealthCheck.ps1 -ServiceNames $ServiceNames -Servername $healthcheckServer -Protocol $configs.Get_Item("protocol") -AumentumSiteName $configs.Get_Item("AumentumSiteName")
        }
    }

    ## Finish and log
    WriteEventLogHelper -Message "Aumentum Microservices installation ended"
}

Stop-Transcript
