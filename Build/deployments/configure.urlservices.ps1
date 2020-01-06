Param(
    [string]$sslEnabled,    
    [string]$partition,
    [string]$resourceLocatorEndPoint
)

$tempfile = "temp.json"
$templatefile="urlservices-template.json"

If (Test-Path $tempfile){
	Remove-Item $tempfile
}

If ($sslEnabled -eq "Y") {
    $protocol = "https"
}else{
    $protocol = "http"
}

(Get-Content $templatefile).replace("%server%", $server).Replace("%partition%", $partition).Replace("%protocol%",$protocol) | Set-Content $tempfile


$endPoint = $protocol + "://" + $resourceLocatorEndPoint + "/v1/Resources?partition=urlservices:" + $partition


try{

    Invoke-RestMethod $endPoint -Method Put -InFile $tempfile -ContentType "application/json"
}
catch{
    
    Write-Output $error[0].Exception.Message

    $result = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($result)
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd();
    Write-Output "Unable to create/update resource(s) - $responseBody"  Write-Output $error[0].Exception.Message
      
}