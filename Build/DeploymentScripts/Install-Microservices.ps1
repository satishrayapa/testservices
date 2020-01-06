<#
.SYNOPSIS
This script install and configure the lumen Microservices

.DESCRIPTION

.PARAMETER SourceRootPath
Path to the folder containing microservices binaries

.PARAMETER ServiceRootPath
Path where the microservices will be deployed

.PARAMETER WinServicePath
Path where the Windows Services will be deployed and installed

.PARAMETER AspNetCoreEnvVariable
The ASP.Net Core Environment Variable to use, this will set the environment variable and create the required files

.PARAMETER ServerName
The server name or IP address, if an IP address is provided, the installation will try to convert it to the server DNS name to ensure proper configurations

.PARAMETER AumentumAppRootPath
The installation path of the Aumentum Web Site

.PARAMETER DbConnectionString
The connection string of the SQL Database server to use

.PARAMETER WebSiteName
The WebSite Name under which Aumentum App is installed
Default : Default Web Site

.PARAMETER AumentumSiteName
The Aumentum Application name if nested under a website, defined by the WebSiteName, otherwise, do not set this value

.PARAMETER aumentumWorkflowURL
The URL of the Aumentum Workflow

.PARAMETER enableBVS
Switch to indicate if BVS should be installed and enabled

.PARAMETER enableSearch
Switch to indicate if search should be installed and enabled

.PARAMETER UpdateWorkflowURL
Switch to indicate if workflow URL should be updated

.PARAMETER RunEFCoreMigration
Switch to indicate if EF Core migrations should be run or not

.PARAMETER UseHTTP
Switch to use HTTP instead of the default HTTPS

.PARAMETER EnableDNSResolution
Use the public DNS name instead of the provided IP address or servername

.PARAMETER UseLocalResourceLocator
Use the localhost to update resource values when the DNS is not reachable from the current machine (for use in a load balancer scenario to have only the last server commit those values)

.PARAMETER AssignAdminRoles
Grant Admin roles to Microservices

.PARAMETER UseSharedSignature
Ensure the same json signature file is used accross all server (for use in a load balancer scenario to have only the last server commit those values)

.PARAMETER EnableHealthCheck
For enabling health check for services (on by default, left there for backward compatibility)

.PARAMETER DisableHealthCheck
For disabling health check for services

.PARAMETER EnableWorklist
Switch to indicate if Worklist should be installed and enabled

.PARAMETER AspNetCoreRequestTimeout
For setting aspNetCore request timeout

.PARAMETER ServerRole
Indicate wheter services should be installed as Primary or Secondary on this instance (for use in a load balancer scenario to have only the Primary server commit values)

#>
param(
     [Parameter(Mandatory=$true)]   [string]$SourceRootPath
    ,[Parameter(Mandatory=$true)]   [string]$ServiceRootPath
    ,[Parameter(Mandatory=$false)]  [string]$WinServicePath
    ,[Parameter(Mandatory=$false)]  [string]$AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]   [string]$ServerName
    ,[Parameter(Mandatory=$true)]   [string]$AumentumAppRootPath
    ,[Parameter(Mandatory=$true)]   [string]$WebSiteName
    ,[Parameter(Mandatory=$false)]  [string]$AumentumSiteName
    ,[Parameter(Mandatory=$true)]   [string]$DbConnectionString
    ,[Parameter(Mandatory=$false)]  [switch]$UpdateWebConnectionString
    ,[Parameter(Mandatory=$false)]  [string]$DbCommandTimeout = "300"  # database command timeout in seconds--default to 5 minutes
    ,[Parameter(Mandatory=$false)]  [switch]$StoreConnectionStringInEnvironmentVariable
    ,[Parameter(Mandatory=$false)]  [switch]$EnableBVS
    ,[Parameter(Mandatory=$false)]  [switch]$EnableSearch
    ,[Parameter(Mandatory=$false)]  [string]$AumentumWorkflowURL
    ,[Parameter(Mandatory=$false)]  [switch]$UseHTTP
    ,[Parameter(Mandatory=$false)]  [switch]$EnableDNSResolution
    ,[Parameter(Mandatory=$false)]  [switch]$AssignAdminRoles
    ,[Parameter(Mandatory=$false)]  [switch]$UseLocalResourceLocator
    ,[Parameter(Mandatory=$false)]  [switch]$UseSharedSignature
    ,[Parameter(Mandatory=$true)]   [ValidateSet("Primary","Secondary")] [string]$ServerRole
    ,[Parameter(Mandatory=$false)]  [switch]$EnableHealthCheck
    ,[Parameter(Mandatory=$false)]  [switch]$DisableHealthCheck
    ,[Parameter(Mandatory=$false)]  [switch]$EnableWorklist
    ,[parameter(Mandatory=$false)]  [string]$AspNetCoreRequestTimeout = "00:10:00" # AspNetCore request time out -- default to 10 minutes
)

# Set current dir
Set-location $PSScriptRoot

##############################################
## Start Configurations
##############################################
$configs = @{}

$configs.Add("SourceRootPath", $SourceRootPath)
$configs.Add("ServiceRootPath", $ServiceRootPath)
$configs.Add("WinServicePath", $WinServicePath)
$configs.Add("AspNetCoreEnvVariable", $AspNetCoreEnvVariable)
$configs.Add("ServerName", $ServerName)
$configs.Add("AumentumAppRootPath", $AumentumAppRootPath)
$configs.Add("WebSiteName", $WebSiteName)
$configs.Add("AumentumSiteName", $AumentumSiteName)
$configs.Add("connectionString", $DbConnectionString)
$configs.Add("UpdateWebConnectionString", $UpdateWebConnectionString.IsPresent)
$configs.Add("DbCommandTimeout", $DbCommandTimeout)
$configs.Add("StoreConnectionStringInEnvironmentVariable", $StoreConnectionStringInEnvironmentVariable.IsPresent)
$configs.Add("EnableBVS", $EnableBVS.IsPresent)
$configs.Add("EnableSearch", $EnableSearch.IsPresent)
$configs.Add("AumentumWorkflowURL", $AumentumWorkflowURL)
$configs.Add("UseHTTP", $UseHTTP.IsPresent)
$configs.Add("EnableDNSResolution", $EnableDNSResolution.IsPresent)
$configs.Add("AssignAdminRoles", $AssignAdminRoles.IsPresent)
$configs.Add("UseLocalResourceLocator", $UseLocalResourceLocator.IsPresent)
$configs.Add("UseSharedSignature", $UseSharedSignature.IsPresent)
$configs.Add("ServerRole", $ServerRole)
$configs.Add("AspNetCoreRequestTimeout", $AspNetCoreRequestTimeout)
$configs.Add("EnableWorklist", $EnableWorklist.IsPresent)
if($DisableHealthCheck.IsPresent){
    $configs.Add("EnableHealthCheck", $false)
}
else{
    $configs.Add("EnableHealthCheck", $true)
}

# Configure Database connection string
if($configs.Get_Item("connectionString") -ne ""){

    try{
        $cs = New-Object System.Data.Common.DbConnectionStringBuilder
        $cs.set_ConnectionString($configs.Get_Item("connectionString"))
        $configs.Add("DbName", $cs.Database)
    }
    catch{}
}

##############################################
## End Configurations
##############################################

.\execute-install.ps1 -configs $configs
