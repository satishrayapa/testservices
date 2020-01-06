Param(
    [string]$sslEnabled,
    [string]$key,
    [string]$partition,
    [string]$value,
    [string]$resourceLocatorEndPoint
)

Import-Module .\tools\Logger.psm1

$tempfile = "temp.json"
$templatefile="urlservice-template.json"

If (Test-Path $tempfile){
	Remove-Item $tempfile
}

if (!$sslEnabled) {
    LogIt "Parameter: sslEnabled must be set."
    exit -1
}

if (!$partition) {
    LogIt "Parameter: partition must be set."
    exit -1
}

if (!$key) {
    LogIt "Parameter: key must be set."
    exit -1
}

if (!$value) {
    LogIt "Parameter: value must be set."
    exit -1
}

if (!$resourceLocatorEndPoint) {
    LogIt "Parameter: resourceLocatorEndPoint must be set."
    exit -1
}

If ($sslEnabled -eq "Y") {
    $protocol = "https"
}else{
    $protocol = "http"
}

$endPoint = $protocol + "://$resourceLocatorEndPoint/v1/Resources/$partition/$key"

$errorCount = 0

while ($errorCount -lt 10) {

    try {
        $exist = Invoke-RestMethod $endPoint
        break
    }
    catch {

        $errorMessage = $error[0].Exception.Message

        if ($errorMessage -eq "Unable to connect to the remote server") {
            $errorCount += 1
            LogIt "Wait 5 seconds before trying again."
            Start-Sleep -Seconds 5
        }        
        else {

            if ($errorMessage -eq "The remote server returned an error: (404) Not Found."){
                $exist = "brandnew"                
            }

            LogIt $errorMessage
            break

        }  
    }
}

if ($exist){

    $body = @{key="$key";partition="$partition";value="$value"} | ConvertTo-Json -Compress

    if ($exist -eq "brandnew"){
        LogIt "Creating resource..."
        try {
        
            $endPoint = $protocol + "://$resourceLocatorEndPoint/v1/Resources/"
            Invoke-RestMethod $endPoint -Method Post -Body $body -ContentType "application/json"
            LogIt "Successfully created resource."
        }
        catch {
    
          LogIt $error[0].Exception.Message
        }
    }
    else{
        LogIt "Updating resource..."
        try {      
            Invoke-RestMethod $endPoint -Method Put -Body $body -ContentType "application/json"
            LogIt "Successfully updated resource."
        }
        catch {
    
          LogIt $error[0].Exception.Message
        }
    }
}
else {
    LogIt "Unable to create or update resource."
}

