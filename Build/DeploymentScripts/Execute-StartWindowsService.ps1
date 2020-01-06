param(
     [Parameter(Mandatory=$true)]    [string]$DeployPath
    ,[Parameter(Mandatory=$true)]    [string]$ServiceFolder
    ,[Parameter(Mandatory=$true)]    [string]$ServiceName
    ,[Parameter(Mandatory=$true)]    [string]$WindowServiceName
)

$serviceDeployPath = Join-Path $DeployPath -ChildPath $ServiceFolder
$service = Join-Path $serviceDeployPath -ChildPath $ServiceName

& $service start -instance $WindowServiceName
