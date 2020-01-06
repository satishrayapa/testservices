Param(
	[switch]${start-service},
	[switch]${stop-service},
	[switch]${rebuild-all},
	[switch]${crawl-progress},
	[string]${service-path},
    [string]$instance
)


$ErrorActionPreference = "stop"
 
function StopService {
	param([string]${service-path})
	Write-Host "Stopping service"
	pushd ${service-path}
	.\TAGov.Process.Sync.LegalPartySearch.Coordinator.exe stop -instance $instance
    popd
}


function StartService {
   Write-Host "Starting service"
	pushd ${service-path}
	.\TAGov.Process.Sync.LegalPartySearch.Coordinator.exe start -instance $instance 
    popd
}

if(${start-service}){
	StartService -service-path ${service-path}
}

if(${stop-service}){
	StopService -service-path ${service-path}
}

if(${rebuild-all}){
	pushd ..\
	$location = Get-Location
	popd
	Write-Host $location
	StopService -service-path ${service-path}
	dotnet .\TAGov.Services.Core.LegalPartySearch.Operations.dll --legalpartysearch-rebuild-all --appsettings-directory $location
	StartService -service-path ${service-path}
}

if(${crawl-progress}){
	pushd ..\
	$location = Get-Location
	popd
	Write-Host $location
	Write-Host "Crawl Status"
	dotnet .\TAGov.Services.Core.LegalPartySearch.Operations.dll --legalpartysearch-crawl-progress --appsettings-directory $location
}

