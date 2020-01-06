<#
.SYNOPSIS
This script configures the required values in the Resource Locator table in SQL Server by call

.DESCRIPTION

.PARAMETER ServerName
Url of the server to set as a configuration

.PARAMETER Partition
The partition to set in the database. Ex: QA.

.PARAMETER ResourceLocatorEndPoint
Url of the resource locator

.PARAMETER $protocol
This switch sets protocol to http or https

#>
param(
     [Parameter(Mandatory=$true)]    [string]  $ResourceLocatorURI
    ,[Parameter(Mandatory=$true)]    [string]  $ServerName
    ,[Parameter(Mandatory=$true)]    [string]  $Partition
    ,[Parameter(Mandatory=$true)]    [string]  $protocol
    ,[Parameter(Mandatory=$true)]    [boolean] $EnableBVS
    ,[Parameter(Mandatory=$true)]    [boolean] $EnableSearch
    ,[Parameter(Mandatory=$false)]   [string]  $AumentumSiteName
)

## Add check that if Nest Switch is on then AumentumSite is required

if(![string]::IsNullOrEmpty($AumentumSiteName) ){
    $ServerName = "$ServerName/$AumentumSiteName"
}

$tempfileFeature = "tempFeature.json"
$tempfileUrlService = "tempUrlService.json"
$templatefileFeatures="resourceLocatorFeatureValues.json"
$templatefileUrlServices="resourceLocatorUrlServicesValues.json"

If (Test-Path $tempfileFeature){
	Remove-Item $tempfileFeature
}

If (Test-Path $tempfileUrlService){
	Remove-Item $tempfileUrlService
}

## Test if ResourceLocator if up before trying to update
$isAvailable = $false
$sleepTimer = 30
$maxRetries = 3
$nbRetries = 0
$healthCheckEndpoint = "$ResourceLocatorURI/hc"

Write-Host "Confirming ResourceLocator availability at $healthCheckEndpoint ..."
while( !$isAvailable -AND $nbRetries -lt $maxRetries){
    try{
        $status=Invoke-RestMethod -Method Get -Uri $healthCheckEndpoint -TimeoutSec 45

        if ($status.status -eq "Healthy"){
            write-host "ResourceLocator is available"
            $isAvailable = $true
        }
        else{
            write-host "ResourceLocator is unhealty, retrying..."
            $nbRetries = $nbRetries + 1
            Start-Sleep -s $sleepTimer
        }
    }
    catch{
        write-host "ResourceLocator is unavailable, retrying..."
        $nbRetries = $nbRetries + 1
        Start-Sleep -s $sleepTimer
    }
}


if($isAvailable){
    Write-Host "Applying Updating ResourceLocator values"
    $endPoint = "$ResourceLocatorURI/v1/Resources?partition=urlservices:$partition"
    Write-Host "endpoint: $endPoint"

    (Get-Content $templatefileFeatures).replace("%server%", $ServerName).Replace("%partition%", $partition).Replace("%protocol%",$protocol).Replace("%bvsfeature%", $EnableBVS).Replace("%searchfeature%", $EnableSearch) | Set-Content $tempfileFeature
    (Get-Content $templatefileUrlServices).replace("%server%", $ServerName).Replace("%partition%", $partition).Replace("%protocol%",$protocol) | Set-Content $tempfileUrlService

    try{

        Invoke-RestMethod $endPoint -Method Put -InFile $tempfileFeature -ContentType "application/json"
        Invoke-RestMethod $endPoint -Method Put -InFile $tempfileUrlService -ContentType "application/json"

        Remove-Item $tempfileFeature
        Remove-Item $tempfileUrlService

        Write-Host "Updated ResourceLocator values:"
        Write-Host "Success" -ForegroundColor Green
    }
    catch{

        if($_.Exception.Response -ne $null){
            $result = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($result)
            $reader.BaseStream.Position = 0
            $reader.DiscardBufferedData()
            $responseBody = $reader.ReadToEnd();
            Write-Verbose "Unable to create/update resource(s) - $responseBody"
            Write-Host "Unable to create/update resource(s)" -ForegroundColor Red
        }
        throw
    }
}
else{
    write-host "ResourceLocator is unavailable, unable to update values, services may not work properly"
}

Write-Host
