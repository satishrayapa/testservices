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

try{
   if(Test-Path $service){
       Write-Host "Uninstalling Windows service: $service"
      
       try{
           & $service -instance $WindowServiceName stop
           & $service -instance $WindowServiceName uninstall
       }
       catch{}

       remove-item $serviceDeployPath -recurse -force
   }

}
catch{
   Write-Host "Failed to install service" -ForegroundColor Red
   Throw
}

