param(
     [Parameter(Mandatory=$true)]    [string]$SourcePath
    ,[Parameter(Mandatory=$true)]    [string]$DeployPath
    ,[Parameter(Mandatory=$true)]    [string]$ServiceFolder
    ,[Parameter(Mandatory=$true)]    [string]$ServiceName
    ,[Parameter(Mandatory=$true)]    [string]$AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]    [string]$WindowServiceName
)

$serviceDeployPath = Join-Path $DeployPath -ChildPath $ServiceFolder
$service = Join-Path $serviceDeployPath -ChildPath $ServiceName

Write-Host "Installing Windows service: $service"
Write-Host "Installing from: $SourcePath\$ServiceFolder"
Write-Host "Installing at: $serviceDeployPath"
try{
    if(Test-Path $service){

        try{
            & $service -instance $AspNetCoreEnvVariable stop
            & $service -instance $AspNetCoreEnvVariable uninstall
        }
        catch{}

        try{
            & $service -instance $WindowServiceName stop
            & $service -instance $WindowServiceName uninstall
        }
        catch{}

        remove-item $serviceDeployPath -recurse -force
    }


    if($ServiceName -ne "TAGov.Process.Sync.LegalPartySearch.exe"){

        if(!(Test-Path $DeployPath)){
            mkdir $DeployPath
        }

        # copy source path to deploy path
        Copy-Item -Path $SourcePath\$ServiceFolder\  -Destination $DeployPath  -recurse -force

        & $service -instance $WindowServiceName install
        Write-Host "Successfully installed service" -ForegroundColor Green
    }
}
catch{
    Write-Host "Failed to install service" -ForegroundColor Red
    Throw
}

