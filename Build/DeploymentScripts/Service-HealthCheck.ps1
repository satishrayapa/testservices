<#
.SYNOPSIS
This script perform health check for lumen Microservices

.DESCRIPTION

.PARAMETER ServiceNames
A list of ALL service names to perform health check

.PARAMETER Servername
Server name where lumen services are installed

.PARAMETER Protocol
Http protocol information (http or https)

.PARAMETER AumentumSiteName
IIS site name for Aumentum

#>

param(
     [Parameter(Mandatory=$true)]   [array]$ServiceNames
    ,[Parameter(Mandatory=$true)]   [string]$Servername
    ,[Parameter(Mandatory=$true)]   [string]$Protocol
    ,[Parameter(Mandatory=$false)]   [string]$AumentumSiteName
)

Write-Host ""
Write-Host "Services health check" -ForegroundColor Green

for ($i=0;$i -lt $ServiceNames.length; $i++) {
    if([string]::IsNullOrEmpty($AumentumSiteName))
    {
        $resource = "${protocol}://$Servername/$($ServiceNames[$i])/hc"
    }
    else
    {
        $resource = "${protocol}://$Servername/$AumentumSiteName/$($ServiceNames[$i])/hc"
    }
    Write-Host ""
    Write-Host "Checking health for service : $($ServiceNames[$i]) - [$resource]"

    try {

        $status=Invoke-RestMethod -Method Get -Uri $resource -TimeoutSec 45

        if ($status.status -eq "Healthy")
        {
           Write-host $status.status -ForegroundColor Green
        }
        else
        {
            Write-host $status.status -ForegroundColor Red
        }
    }
    catch
    {
        Write-Host "Error:$($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
        Write-Host "Re-Checking health for service : $($ServiceNames[$i]) - [$resource]" -ForegroundColor Cyan
        Start-Sleep 15
        try {
            $status=Invoke-RestMethod -Method Get -Uri $resource -TimeoutSec 45

            if ($status.status -eq "Healthy")
            {
               Write-host $status.status -ForegroundColor Green
            }
            else
            {
                Write-host $status.status -ForegroundColor Red
            }
        }
        catch
        {
            Write-Host "Error:$($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

Write-Host ""
Write-Host "Services health check Done!" -ForegroundColor Green
Write-Host ""
