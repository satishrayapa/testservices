Param(
    [string]$sslEnabled,
    [string]$partition,
    [string]$resourceLocatorEndPoint
)

#Import-Module .\tools\Logger.psm1

$tempfile = "temp.json"
$templatefile="features-template.json"

If (Test-Path $tempfile){
	Remove-Item $tempfile
}

#$sslEnabled = Read-Host -Prompt "Enter if SSL is enabled Y for Yes (Other input will equals No)"
#$partition = Read-Host -Prompt "Enter partition"
#$resourceLocatorEndPoint = Read-Host -Prompt "Enter resourceLocator EndPoint"

If ($sslEnabled -eq "Y") {
    $protocol = "https"
}else{
    $protocol = "http"
}

(Get-Content $templatefile).Replace("%partition%", $partition) | Set-Content $tempfile


$endPoint = $protocol + "://" + $resourceLocatorEndPoint + "/v1/Resources?partition=urlservices:" + $partition

$errorCount = 0
while ($errorCount -lt 10){
    try {

        Invoke-RestMethod $endPoint -Method Put -InFile $tempfile -ContentType "application/json"
        #LogIt "Successfully invoked features."
        break
    }
    catch {
    
        $errorMessage = $error[0].Exception.Message

        
        if ($errorMessage -eq "Unable to connect to the remote server") {
            $errorCount += 1
            #LogIt "Wait 5 seconds before trying again."
            Start-Sleep -Seconds 5
        }        
        else {

            #LogIt $errorMessage
            break

        }      
    }
}