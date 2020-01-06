param($ServiceClientId, $ServiceClientPassword, $ServicesDeployStackName)

$securityRoot = "https://{0}common-security.azurewebsites.net"
$securityRoot = $securityRoot.Replace("{0}", $ServicesDeployStackName)

$tokenEndpoint = "$securityRoot/connect/token"

$obj = @{"client_id" = "$ServiceClientId"; "client_secret" = "$ServiceClientPassword"; "grant_type" = "client_credentials"; "scope" = "api.common.resourcelocator" }

try {
    $payload = Invoke-RestMethod -Uri $tokenEndpoint -Body $obj -Method 'POST'
    $token = $payload.access_token
}
catch {
    
    $statusCode = $_.Exception.Response.StatusCode.value__
  
    $result = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($result)
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd();
  
    Write-Host "Status code: $statusCode Response: $responseBody"

    throw $responseBody
}

try {
    $headers = @{ "Authorization" = "Bearer $token" }

    $resourceLocatorRoot = "https://{0}common-resourcelocator.azurewebsites.net"
    $resourceLocatorRoot = $resourceLocatorRoot.Replace("{0}", $ServicesDeployStackName)
    $services = Invoke-RestMethod -Uri "$resourceLocatorRoot/v1/Resources?partition=urlservices:prod" -Headers $headers -ContentType "application/json" -Method 'GET'

    $services | ForEach-Object {
        $svc = $_
        $svc.value = "https://" + $ServicesDeployStackName + $svc.key.Replace(".", "-").Replace("-assessmentheader", "-assessmentevent") + ".azurewebsites.net"
    }

    $body = $services | ConvertTo-Json
    $body 
    Invoke-RestMethod -Uri "$resourceLocatorRoot/v1/Resources?partition=urlservices:prod" -Headers $headers -ContentType "application/json" -Method 'PUT' -Body $body 
}
catch {
    
    $statusCode = $_.Exception.Response.StatusCode.value__
  
    $result = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($result)
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd();

    Write-Host "Status code: $statusCode Response: $responseBody"
}