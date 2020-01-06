
<#
.SYNOPSIS
This script will add the requestTimeout attribute in web.config file of all services

.DESCRIPTION

.PARAMETER ServiceNames
A list of ALL service names for adding requestTimeout

.PARAMETER ServiceRootPath
Path where the microservices will be deployed

.PARAMETER AspNetCoreRequestTimeout
For setting aspNetCore request timeout

#>

param(
     [Parameter(Mandatory=$true)]   [array]$ServiceNames
    ,[Parameter(Mandatory=$true)]   [string]$ServiceRootPath
    ,[parameter(Mandatory=$false)]  [string]$AspNetCoreRequestTimeout
)



try
{
    for ($i=0;$i -lt $ServiceNames.length; $i++){
        Write-Host ""
        Write-Host "Adding aspNetcore requestTimeout for $($ServiceNames[$i])"

        $Path = "$ServiceRootPath\$($ServiceNames[$i])\web.config"
        $xml = [xml] (Get-Content $Path)

        $XPath = "/configuration/system.webServer/aspNetCore"
        $node = $xml.SelectSingleNode($XPath)

        $attrib = $node.OwnerDocument.CreateAttribute("requestTimeout")
        $attrib.Value = $AspNetCoreRequestTimeout
        $node.Attributes.Append($attrib) | out-null

        $xml.Save($Path)

        Write-Host "Done!" -ForegroundColor Green
    }
}
catch
{
     Write-Host "Error while adding aspNetcore requestTimeout for $($ServiceNames[$i]) : $($_.Exception.Message)" -ForegroundColor Red
}
