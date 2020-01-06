<#
.SYNOPSIS
This script installs the Lumen Micro Service named by $ServiceName as an application under the IIS Web Site named by $WebSiteName and runs EF Core Migrations.

.DESCRIPTION
Additionally, it:
* ensure app pool is setup
* removes any appsettings.*.json file that does not specify the AppNetCoreEnvVariable value

.PARAMETER SourcePath
The full path to the service's binary files to install.

.PARAMETER DeployPath
The full path where service will be installed.

.PARAMETER ServiceName
The service name and name of the service's directory.

.PARAMETER WebSiteName
The IIS web site where the service's application will be installed.

.PARAMETER AspNetCoreEnvVariable
Sets machine's ASPNETCORE_ENVIRONMENT environment variable to this value and ensures only appsettings.json and appsettings.$AspNetCoreEnvVariable.json files are included in installation.

.EXAMPLE
.\InstallDotNetCoreService.ps1 -SourcePath c:\Lumen\Services\MyService -DeployPath c:\iissites\Lumen\Services\MyService -ServiceName MyService -WebSiteName "Default Web Site" -AspNetCoreEnvVariable QA
#>
param(
         [Parameter(Mandatory=$true)]    [string]$SourcePath
        ,[Parameter(Mandatory=$true)]    [string]$DeployPath
        ,[Parameter(Mandatory=$true)]    [string]$ServiceName
        ,[Parameter(Mandatory=$true)]    [string]$WebSiteName
        ,[Parameter(Mandatory=$True)]    [string]$AppPoolName
        ,[Parameter(Mandatory=$false)]   [string]$AumentumSiteName
        ,[Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
        ,[Parameter(Mandatory=$false)]   [switch]$UseSharedSignature
)

$ErrorActionPreference = "Stop"
Write-Host "Installing service: $ServiceName..."

$virtualDirPath = "IIS:\Sites\$WebSiteName"

$fullWebSitePath = Join-Path $DeployPath -ChildPath $ServiceName

# Determine if should be installed as the top level website or nested
if(![string]::IsNullOrEmpty($AumentumSiteName) ){
    $serviceAppName = "$AumentumSiteName/$ServiceName"
    $serviceVirtualDirPath = "$virtualDirPath\$AumentumSiteName\$ServiceName"
}
else{
    $serviceAppName = $ServiceName
    $serviceVirtualDirPath = "$virtualDirPath\$ServiceName"
}

if(!(Test-Path $DeployPath)){
    mkdir $DeployPath
}

# copy source path to deploy path
Copy-Item -Path $SourcePath  -Destination $DeployPath  -recurse -Force

# create log folder
$serviceDestination = Join-Path $DeployPath -ChildPath $ServiceName
$serviceLogFolder = Join-Path $serviceDestination -ChildPath "logs"
if(!(Test-Path $serviceLogFolder)){
    mkdir $serviceLogFolder
}

if($UseSharedSignature.IsPresent){
    if($ServiceName -eq "Common.Security"){
        $signFile = Join-Path $PSScriptRoot -ChildPath "sign.json"
        Copy-Item -Path $signFile  -Destination $serviceDestination  -recurse -Force
    }
}

# set environment
$env:ASPNETCORE_ENVIRONMENT = $AspNetCoreEnvVariable;
[Environment]::SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", $AspNetCoreEnvVariable, "Machine")
set-item "env:ASPNETCORE_ENVIRONMENT" $AspNetCoreEnvVariable

# get IIS manager object
try {
    import-Module IISAdministration
    $newIISFeatureEnabled = $true
}
catch {
    $newIISFeatureEnabled = $false
}

#$newIISFeatureEnabled = $false

try{

    if($newIISFeatureEnabled){
        #Write-host "Using newest IIS powershell module"
        # use newest IIS powershell module
        $manager = Get-IISServerManager

        # create app pool
        $pool = $manager.ApplicationPools[$AppPoolName]

        if ($pool -eq $null)
        {
            $pool = $manager.ApplicationPools.Add($appPoolName)
        }

        $pool.ManagedPipelineMode = "Integrated"
        $pool.ManagedRuntimeVersion = ""
        $pool.AutoStart = $true
        #enable AlwaysRunning to avoid cold start issue
        $pool.StartMode = "AlwaysRunning"
        $pool.ProcessModel.IdentityType = "ApplicationPoolIdentity"
        $manager.CommitChanges()


        # create web site
        $site = $manager.Sites[$WebSiteName]

        if ($site -eq $null) {
            # Site does not exist, create it...
            throw "Web Site provided $WebSiteName does not exists."
        }

        # Check if application exists, if yes, try to clean it up
        if($site.Applications["/$serviceAppName"] -ne $null)
        {
            $app = $site.Applications["/$serviceAppName"]
            $previousPath = $app.VirtualDirectories.PhysicalPath

            if($fullWebSitePath -ne $previousPath){
                try{
                    if( Test-Path $previousPath ){
                        Write-Host "Previous services found in a different installation folder, trying to cleanup old files"
                        remove-item $previousPath -Recurse -Force
                    } else{
                        Write-Host "Previous services found in a different installation folder but folder do not exists anymore, no cleanup required"
                    }
                }
                catch{
                    Write-Host "Impossible to cleanup previous service installation"
                }
            }

            Write-Host "Removing previous webapplication $app"
            $site.Applications.Remove($app)
            $manager.CommitChanges()
            $site = $manager.Sites[$WebSiteName]
        }

        # re-create Application
        Write-Host "Creating webapplication $serviceAppName at location: $fullWebSitePath"
        $app = $site.Applications.Add("/$serviceAppName", $fullWebSitePath)
        $manager.CommitChanges()

        # Assign to an application in a virtual directory
        $website = $manager.Sites[$WebSiteName]

        $website.Applications["/$serviceAppName"].ApplicationPoolName = $appPoolName
        #enable preload to avoid cold start issue
        $website.Applications["/$serviceAppName"].Attributes["preloadEnabled"].Value = $true

        $manager.CommitChanges()
    }
    else{
        #Write-host "Using older IIS powershell module"
        # use older IIS powershell module
        Import-Module WebAdministration

        $appPoolPath = Join-Path "IIS:\AppPools\" -ChildPath $AppPoolName
        # check if app pool exist, otherwise create it
        if ((Test-Path $appPoolPath) -eq $False) {
            # create app pool

            New-Item -Path "IIS:\AppPools" -Name $appPoolName -Type AppPool
        }

        Set-ItemProperty -Path $appPoolPath -name "managedRuntimeVersion" -value ""
        Set-ItemProperty -Path $appPoolPath -name "autoStart" -value $true
        Set-ItemProperty -Path $appPoolPath -name "processModel" -value @{identitytype="ApplicationPoolIdentity"}
        Set-ItemProperty -Path $appPoolPath -name "managedPipelineMode" -value 0
        #enable AlwaysRunning to avoid cold start issue
        if ([Environment]::OSVersion.Version -ge (new-object 'Version' 6,2)) {
            Set-ItemProperty -Path $appPoolPath -name "startMode" -value "AlwaysRunning"
        }

        if ((Test-Path $virtualDirPath) -eq $False) {
            throw "Web Site provided $WebSiteName does not exists."
        }

        #create the new virtual directory
        New-Item -Type Application -Path $serviceVirtualDirPath -physicalPath $fullWebSitePath -force
        #assign the app pool
        Set-ItemProperty -Path $serviceVirtualDirPath -name "applicationPool" -value $appPoolName -force
        #enable preload to avoid cold start issue
        Set-ItemProperty -Path $serviceVirtualDirPath -name "preloadEnabled" -value $true -force
    }


    # delete non relevant appsettings
    Get-ChildItem $fullWebSitePath -filter "appsettings.*.json" -Recurse | `
    where-object {$_.name -ne 'appsettings.deploy.json'} | `
        ForEach-Object {Remove-Item $_.FullName}

    Write-Host "Done!" -ForegroundColor Green
}
catch{
    Write-Host "Failed." -ForegroundColor Red
    throw
}
