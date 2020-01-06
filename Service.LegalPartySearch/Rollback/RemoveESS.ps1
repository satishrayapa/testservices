param(
      [Parameter(Mandatory=$true)]    [string]$WinServicePath
     ,[Parameter(Mandatory=$true)]    [string]$WebSiteName
)

$serviceFolder = "Process.Sync.LegalPartySearch.Coordinator"
$serviceName = "TAGov.Process.Sync.LegalPartySearch.Coordinator.exe"
$serviceDeployPath = Join-Path $WinServicePath -ChildPath $ServiceFolder
$service = Join-Path $serviceDeployPath -ChildPath $ServiceName

try{
    & $service -instance $WebSiteName stop
    & $service -instance $WebSiteName uninstall

    remove-item $serviceDeployPath -recurse -force
}
catch{
    Write-Host "Unable to remove service"
}



